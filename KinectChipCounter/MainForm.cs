﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Kinect;
using Emgu.CV;
using Emgu.CV.Structure;

namespace KinectChipCounter
{
    public partial class MainForm : Form
    {
        private KinectSensor kinect;
        private bool videoToggle;

        private bool handlingDepthFrame = false;
        private bool handlingColorFrame = false;

        public MainForm()
        {
            InitializeComponent();
            foreach(KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if(sensor.Status == KinectStatus.Connected)
                {
                    kinect = sensor;
                    break;
                }
            }
            if (kinect != null)
            {
                kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                kinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                kinect.SkeletonStream.Enable();
                kinect.Start();
                kinect.ElevationAngle = 0;
                lblAngle.Text = "Angle: " + kinect.ElevationAngle;
            }
        }

        private void btnTiltUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (kinect.ElevationAngle < kinect.MaxElevationAngle)
                {
                    kinect.ElevationAngle = kinect.MaxElevationAngle;
                    lblAngle.Text = "Angle: " + kinect.ElevationAngle;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }
        }

        private void btnTiltDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (kinect.ElevationAngle > kinect.MinElevationAngle)
                {
                    kinect.ElevationAngle = kinect.MinElevationAngle;
                    lblAngle.Text = "Angle: " + kinect.ElevationAngle;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }
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
                ret[0] = 255;
                ret[1] = (byte)value;
                ret[2] = 0;
                ret[3] = 0;
            } else if(value < 512)
            {
                ret[0] = (byte)(255-(value-256));
                ret[1] = 255;
                ret[2] = (byte)(value-256);
                ret[3] = 0;
            } else if(value < 768)
            {
                ret[0] = 0;
                ret[1] = (byte)(255-(value-512));
                ret[2] = 255;
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

        private void depthImageReady(object sender, DepthImageFrameReadyEventArgs args)
        {
            if (handlingDepthFrame) return;
            handlingDepthFrame = true;
            try
            {

                DepthImageFrame depthFrame = args.OpenDepthImageFrame();
                if(depthFrame==null)
                {
                    handlingDepthFrame = false;
                    return;
                }
                Bitmap depthBmp = new Bitmap(640, 480);
                BitmapData depthBmpData = depthBmp.LockBits(new Rectangle(0, 0, 640, 480), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                byte[] rawData = new byte[640 * 480 * sizeof(int)];
                int pos = 0;
                int min = depthFrame.MinDepth + (int)((float)(depthFrame.MaxDepth - depthFrame.MinDepth) * ((float)trackBar1.Value / (float)1000));
                int max = depthFrame.MinDepth + (int)((float)(depthFrame.MaxDepth - depthFrame.MinDepth) * ((float)trackBar2.Value / (float)1000));
                int res = (max - min) / 256;
                foreach (DepthImagePixel dp in depthFrame.GetRawPixelData())
                {
                    byte[] colors = colorsForDepthValue(dp.Depth, min, max);
                    rawData[pos++] = colors[0];
                    rawData[pos++] = colors[1];
                    rawData[pos++] = colors[2];
                    rawData[pos++] = colors[3];
                }
                lblAngle.Text = "<: " + depthFrame.MinDepth + ",>: " + depthFrame.MaxDepth;
                System.Runtime.InteropServices.Marshal.Copy(rawData, 0, depthBmpData.Scan0, (int)(640 * 480 * sizeof(int)));
                depthBmp.UnlockBits(depthBmpData);
                depthFrame.Dispose();
                Emgu.CV.Image<Bgr, Byte> depthImg = new Emgu.CV.Image<Bgr, Byte>(depthBmp);
                depthImg = depthImg.Resize(400, 300, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                depthImg = depthImg.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
                imageBox2.Image = depthImg;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }
            handlingDepthFrame = false;
        }

        private void colorImageReady(object sender, ColorImageFrameReadyEventArgs args)
        {
            if (handlingColorFrame) return;
            handlingColorFrame = true;
            try
            {
                ColorImageFrame frame = args.OpenColorImageFrame();
                if(frame==null)
                {
                    handlingColorFrame = false;
                    return;
                }
                Bitmap bmp = new Bitmap(640, 480);
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, 640, 480), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                System.Runtime.InteropServices.Marshal.Copy(frame.GetRawPixelData(), 0, bmpData.Scan0, (int)(640 * 480 * sizeof(int)));
                bmp.UnlockBits(bmpData);
                frame.Dispose();
                Emgu.CV.Image<Bgr, Byte> img = new Emgu.CV.Image<Bgr, Byte>(bmp);
                img = img.Resize(400, 300, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                img = img.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
                imageBox1.Image = img;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }
            handlingColorFrame = false;
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if(!videoToggle)
            {
                kinect.DepthFrameReady += depthImageReady;
                kinect.ColorFrameReady += colorImageReady;
                videoToggle = !videoToggle;
            }
            else
            {
                kinect.DepthFrameReady -= depthImageReady;
                kinect.ColorFrameReady -= colorImageReady;
                videoToggle = !videoToggle;
            }
        }
    }
}