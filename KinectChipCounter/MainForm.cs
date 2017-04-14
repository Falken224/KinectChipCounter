using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Kinect;
using Emgu.CV;
using Emgu.CV.Structure;

using DirectShowLib;

namespace KinectChipCounter
{
    public partial class MainForm : Form
    {
        private bool videoToggle;
        private Capture cap;

        private bool handlingColorFrame = false;
        
        private Stack.Color? nextChipColor = null;

        public MainForm()
        {
            InitializeComponent();
            
            DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            cap = new Capture(0);
            cap.ImageGrabbed += webcamImageReady;
        }

        private byte[] colorsForDepthValue(short value,int min, int max)
        {
            byte[] ret = new byte[4];
            if(value<min || value > max)
            {
                ret[0] = 0;
                ret[1] = 0;
                ret[2] = 0;
                ret[3] = 0;
                return ret;
            }
            value = (short)(value - (short)min);
            value = (short)((double)(value) * ((double)1024/((double)max-(double)min)));
            if(value<256)
            {
                ret[0] = (byte)value;
                ret[1] = 0;
                ret[2] = 0;
                ret[3] = 0;
            } else if(value < 512)
            {
                ret[0] = (byte)(255-(value-256));
                ret[1] = (byte)(value-256);
                ret[2] = 0;
                ret[3] = 0;
            } else if(value < 768)
            {
                ret[0] = 0;
                ret[1] = (byte)(255-(value-512));
                ret[2] = (byte)(value - 512);
                ret[3] = 0;
            } else if(value < 1024)
            {
                ret[0] = 0;
                ret[1] = 0;
                ret[2] = (byte)(255-(value-768));
                ret[3] = 0;
            }
            return ret;
        }

        private void webcamImageReady(object sender, EventArgs args)
        {
            if (handlingColorFrame) return;
            try
            {
                handlingColorFrame = true;
                Thread webcamThread = new Thread(new ThreadStart(handleWebCamImage));
                webcamThread.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }
        }

        public void handleWebCamImage()
        {
            Image<Bgr, Byte> img = cap.QueryFrame().ToImage<Bgr,Byte>();
            Image<Bgr, Byte> graphImage = (Image<Bgr, Byte>)hueGraph.Image;
            ChipFinder cf = new ChipFinder(img, graphImage, nextChipColor);
            cf.findChips();
            analysisImgBox.Image = cf.diagnosticImage.Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
            img = img.Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
            imageBox1.Image = img.Convert<Gray, Byte>();
            hueGraph.Image = graphImage;
            handlingColorFrame = false;
            nextChipColor = null;
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if(!videoToggle)
            {
                nextChipColor = null;
                Image<Bgr,Byte> graphImage = new Image<Bgr, Byte>(640,480);
                graphImage.FillConvexPoly(
                    new Point[]{
                                new Point(0,0),
                                new Point(640,0),
                                new Point(640,480),
                                new Point(0,480)
                            }, new Bgr(Color.Black));
                hueGraph.Image = graphImage;
                cap.Start();
                videoToggle = !videoToggle;
            }
            else
            {
                cap.Stop();
                videoToggle = !videoToggle;
            }
        }

        private void btnWhite_Click(object sender, EventArgs e)
        {
            nextChipColor = Stack.Color.White;
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            nextChipColor = Stack.Color.Red;
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            nextChipColor = Stack.Color.Blue;

        }

        private void btnGreen_Click(object sender, EventArgs e)
        {
            nextChipColor = Stack.Color.Green;

        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            nextChipColor = Stack.Color.Black;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ColorFinder.retrain();
        }
    }
}
