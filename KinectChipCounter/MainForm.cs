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
        private KinectSensor kinect;
        private bool videoToggle;
        private Capture cap;

        private bool handlingDepthFrame = false;
        private bool handlingColorFrame = false;

        private int ctrlMinDepth = 0;
        private int ctrlMaxDepth = 0;

        private StackRegistry stackReg = new StackRegistry();
        private Stack.Color? nextChipColor = null;

        public MainForm()
        {
            InitializeComponent();
            
            //KinectSensor sensor = KinectSensor.GetDefault();
            //if(sensor.IsAvailable)
            //{
            //    kinect = sensor;
            //}
            //if (kinect != null)
            //{
            //    kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            //    kinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            //    kinect.SkeletonStream.Enable();
            //    kinect.Start();
            //    kinect.ElevationAngle = 0;
            //    lblAngle.Text = "Angle: " + kinect.ElevationAngle;
            //}
            //else
            //{
                DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
                cap = new Capture(0);
                cap.ImageGrabbed += webcamImageReady;
            //}
        }

        private void btnTiltUp_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (kinect.ElevationAngle < kinect.MaxElevationAngle)
            //    {
            //        kinect.ElevationAngle = kinect.MaxElevationAngle;
            //        lblAngle.Text = "Angle: " + kinect.ElevationAngle;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.StackTrace.ToString());
            //}
        }

        private void btnTiltDown_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (kinect.ElevationAngle > kinect.MinElevationAngle)
            //    {
            //        kinect.ElevationAngle = kinect.MinElevationAngle;
            //        lblAngle.Text = "Angle: " + kinect.ElevationAngle;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.StackTrace.ToString());
            //}
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

        //private void depthImageReady(object sender, DepthImageFrameReadyEventArgs args)
        //{
        //    if (handlingDepthFrame) return;
        //    handlingDepthFrame = true;
        //    ctrlMinDepth = 400;//trkMinDepth.Value;
        //    ctrlMaxDepth = 8000;//trkMaxDepth.Value;
        //    Thread kinectDepthThread = new Thread(new ParameterizedThreadStart(handleKinectDepthImage));
        //    kinectDepthThread.Start(args);
        //}

        //private void handleKinectDepthImage(object obj)
        //{
        //    try
        //    {
        //        DepthImageFrameReadyEventArgs args = (DepthImageFrameReadyEventArgs)obj;
        //        DepthImageFrame depthFrame = args.OpenDepthImageFrame();
        //        if (depthFrame == null)
        //        {
        //            handlingDepthFrame = false;
        //            return;
        //        }
        //        Bitmap depthBmp = new Bitmap(640, 480);
        //        BitmapData depthBmpData = depthBmp.LockBits(new Rectangle(0, 0, 640, 480), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //        byte[] rawData = new byte[640 * 480 * sizeof(int)];
        //        int pos = 0;
        //        int min = depthFrame.MinDepth + (int)((float)(depthFrame.MaxDepth - depthFrame.MinDepth) * ((float)ctrlMinDepth / (float)1000));
        //        int max = depthFrame.MinDepth + (int)((float)(depthFrame.MaxDepth - depthFrame.MinDepth) * ((float)ctrlMaxDepth / (float)1000));
        //        int res = (max - min) / 256;
        //        foreach (DepthImagePixel dp in depthFrame.GetRawPixelData())
        //        {
        //            byte[] colors = colorsForDepthValue(dp.Depth, min, max);
        //            rawData[pos++] = colors[0];
        //            rawData[pos++] = colors[1];
        //            rawData[pos++] = colors[2];
        //            rawData[pos++] = colors[3];
        //        }
        //        System.Runtime.InteropServices.Marshal.Copy(rawData, 0, depthBmpData.Scan0, (int)(640 * 480 * sizeof(int)));
        //        depthBmp.UnlockBits(depthBmpData);
        //        depthFrame.Dispose();
        //        Emgu.CV.Image<Bgr, Byte> depthImg = new Emgu.CV.Image<Bgr, Byte>(depthBmp);
        //        depthImg = depthImg.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
        //        imageBox2.Image = depthImg;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.StackTrace.ToString());
        //    }
        //    handlingDepthFrame = false;
        //}

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
            cf.findChips(stackReg);
            analysisImgBox.Image = cf.diagnosticImage.Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
            img = img.Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
            imageBox1.Image = img.Convert<Gray, Byte>();
            hueGraph.Image = graphImage;
            handlingColorFrame = false;
            nextChipColor = null;
        }

        //private void colorImageReady(object sender, ColorImageFrameReadyEventArgs args)
        //{
        //    if (handlingColorFrame)
        //    {
        //        return;
        //    }
        //    handlingColorFrame = true;
        //    Thread kinectColorThread = new Thread(new ParameterizedThreadStart(handleColorKinectImage));
        //    kinectColorThread.Start(args);
        //}

        //private void handleColorKinectImage(object obj)
        //{
        //    try
        //    {
        //        ColorImageFrameReadyEventArgs args = (ColorImageFrameReadyEventArgs)obj;
        //        ColorImageFrame frame = args.OpenColorImageFrame();
        //        if (frame == null)
        //        {
        //            handlingColorFrame = false;
        //            return;
        //        }
        //        Bitmap bmp = new Bitmap(640, 480);
        //        BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, 640, 480), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //        System.Runtime.InteropServices.Marshal.Copy(frame.GetRawPixelData(), 0, bmpData.Scan0, (int)(640 * 480 * sizeof(int)));
        //        bmp.UnlockBits(bmpData);
        //        frame.Dispose();
        //        Emgu.CV.Image<Bgr, Byte> img = new Emgu.CV.Image<Bgr, Byte>(bmp);
        //        Emgu.CV.Image<Bgr, Byte> graphImage = (Image<Bgr, Byte>)hueGraph.Image;
        //        ChipFinder cf = new ChipFinder(img,graphImage);
        //        cf.findChips(stackReg);
        //        analysisImgBox.Image = cf.diagnosticImage.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
        //        img = img.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
        //        imageBox1.Image = img;
        //        hueGraph.Image = graphImage;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.StackTrace.ToString());
        //    }
        //    handlingColorFrame = false;
        //}

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
                //if (kinect != null)
                //{
                //    kinect.DepthFrameReady += depthImageReady;
                //    kinect.ColorFrameReady += colorImageReady;
                //}
                //else
                //{
                    cap.Start();
                //}
                videoToggle = !videoToggle;
            }
            else
            {
                //if(kinect !=null)
                //{
                //    kinect.DepthFrameReady -= depthImageReady;
                //    kinect.ColorFrameReady -= colorImageReady;
                //}
                //else
                //{
                    cap.Stop();
                //}
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
            stackReg.retrain();
        }
    }
}
