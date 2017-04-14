using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KinectChipCounter
{
    public class ChipFinder
    {
        private Emgu.CV.Image<Bgr, Byte> sourceImage;
        public Emgu.CV.Image<Bgr, Byte> diagnosticImage;
        private Emgu.CV.Image<Bgr, Byte> graphImage;
        private Stack.Color? foundChipColor;

        private static CascadeClassifier cc = new CascadeClassifier("cascade.xml");

        public ChipFinder(Emgu.CV.Image<Bgr, Byte> source, Emgu.CV.Image<Bgr, Byte> graph)  : this(source, graph, null)
        {
        }

        public ChipFinder(Emgu.CV.Image<Bgr, Byte> source, Emgu.CV.Image<Bgr, Byte> graph, Stack.Color? foundColor)
        {
            sourceImage = source;
            graphImage = graph;
            foundChipColor = foundColor;
        }

        public void findChips()
        {
            try
            {
                //CudaImage<Bgr,Byte> gpuImg = new CudaImage<Bgr,Byte>(sourceImage);
                //VectorOfGpuMat outVec = new VectorOfGpuMat();
                //CudaGaussianFilter gaussFilter = new CudaGaussianFilter(DepthType.new Size(11,11),0.8,0.8,BorderType.Constant,BorderType.Constant);
                //gaussFilter.Apply(gpuImg, gpuImg, null);
                diagnosticImage = sourceImage.Clone();
                Rectangle[] rects = cc.DetectMultiScale(sourceImage.Convert<Gray, Byte>(), 1.1, 4, new System.Drawing.Size(10, 10));
                //cc.ScaleFactor = 1.1;
                //cc.MinNeighbors = 4;
                //cc.MinObjectSize = new Size(10, 10);
                //cc.DetectMultiScale(gpuImg.Convert<Gray, Byte>(), outVec);
                //Rectangle[] rects = cc.Convert(outVec);

                foreach (Rectangle rect in rects)
                {
                    StackRegistry.stackFound(rect);
                }

                List<Stack> processedStacks = StackRegistry.processFrame(sourceImage);

                if (foundChipColor != null)
                {
                    if (processedStacks.Count == 1)
                    {
                        ColorFinder.trainChip(sourceImage, processedStacks[0].samplePoints, (Stack.Color)foundChipColor);
                    }
                }

                foreach (Stack stack in processedStacks)
                {
                    Bgr col = new Bgr(Color.Magenta);
                    switch(stack.color)
                    {
                        case Stack.Color.Black:
                            col = new Bgr(Color.Black);
                            break;
                        case Stack.Color.Blue:
                            col = new Bgr(Color.Blue);
                            break;
                        case Stack.Color.Red:
                            col = new Bgr(Color.Red);
                            break;
                        case Stack.Color.Green:
                            col = new Bgr(Color.Green);
                            break;
                        case Stack.Color.White:
                            col = new Bgr(Color.White);
                            break;

                    }
                    diagnosticImage.Draw(stack.location, col, 2);
                    foreach (Point sample in stack.samplePoints)
                    {
                        diagnosticImage.Draw(new Rectangle(sample, new Size(1, 1)), new Bgr(System.Drawing.Color.Yellow), 2);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }

        }
    }

    public class Stack
    {
        public Rectangle location;
        public Point[] samplePoints;
        public Color color;

        public enum Color
        {
            White,
            Red,
            Blue,
            Green,
            Black
        }
    }

    public sealed class StackRegistry
    {
        private static volatile StackRegistry instance;
        private static object sync = new Object();

        private static StackRegistry Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                            instance = new StackRegistry();
                    }
                }
                return instance;
            }
        }

        private ISet<Rectangle> foundThisFrame = new HashSet<Rectangle>();

        private StackRegistry() { }

        public static void stackFound(Rectangle rect)
        {
            Instance.foundThisFrame.Add(rect);
        }

        private static bool correlateRectangles(Rectangle rect, Dictionary<Stack, List<Rectangle>> correlatedFinds)
        {
            foreach (Stack stack in correlatedFinds.Keys)
            {
                if (rect.IntersectsWith(stack.location))
                {
                    Rectangle check = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                    check.Intersect(stack.location);
                    if ((check.Width > stack.location.Width * 0.5) && (check.Height > stack.location.Height * 0.5))
                    {
                        correlatedFinds[stack].Add(rect);
                        stack.location = averageRectangles(correlatedFinds[stack]);
                        return true;
                    }
                }
            }
            return false;
        }

        public static Rectangle averageRectangles(List<Rectangle> correlatedRectangles)
        {
            Rectangle newLocation = new Rectangle();
            int count = correlatedRectangles.Count;
            int widthTotal = 0;
            int heightTotal = 0;
            int centerXTotal = 0;
            int centerYTotal = 0;
            foreach (Rectangle rect in correlatedRectangles)
            {
                widthTotal += rect.Width;
                heightTotal += rect.Height;
                centerXTotal += rect.X + (rect.Width / 2);
                centerYTotal += rect.Y + (rect.Height / 2);
            }
            newLocation.Width = widthTotal / count;
            newLocation.Height = heightTotal / count;
            newLocation.X = (centerXTotal / count) - (newLocation.Width / 2);
            newLocation.Y = (centerYTotal / count) - (newLocation.Height / 2);
            return newLocation;
        }

        private static void trimRange(Stack stack, List<Rectangle> rectangles)
        {
            foreach(Rectangle rect in rectangles)
            {
                stack.location.Intersect(rect);
            }
        }

        public static List<Stack> processFrame(Image<Bgr, Byte> img)
        {
            Dictionary<Stack, List<Rectangle>> correlatedFinds = new Dictionary<Stack, List<Rectangle>>();
            foreach (Rectangle rect in Instance.foundThisFrame)
            {
                if(!correlateRectangles(rect,correlatedFinds))
                {
                    Stack stack = new Stack();
                    stack.location = rect;
                    correlatedFinds[stack] = new List<Rectangle>();
                    correlatedFinds[stack].Add(rect);
                }
            }
            foreach (Stack stack in correlatedFinds.Keys)
            {
                List<Bgr> colorSamples = new List<Bgr>();
                trimRange(stack, correlatedFinds[stack]);
                stack.samplePoints = pickColorPoints(stack.location);
                foreach (Point point in stack.samplePoints)
                {
                    colorSamples.Add(img[point]);
                }
                stack.color = ColorFinder.guessColor(colorSamples.ToArray());
            }
            Instance.foundThisFrame.Clear();
            return new List<Stack>(correlatedFinds.Keys);
        }

        private static Point[] pickColorPoints(Rectangle foundChip)
        {
            Point[] ret = new Point[5];
            ret[0] = new Point(foundChip.Left + (int)(foundChip.Width * .5), foundChip.Top + (int)(foundChip.Height * .5));
            ret[1] = new Point(foundChip.Left + (int)(foundChip.Width * .85), foundChip.Top + (int)(foundChip.Height * .5));
            ret[2] = new Point(foundChip.Left + (int)(foundChip.Width * .84), foundChip.Top + (int)(foundChip.Height * .58));
            ret[3] = new Point(foundChip.Left + (int)(foundChip.Width * .5), foundChip.Top + (int)(foundChip.Height * .85));
            ret[4] = new Point(foundChip.Left + (int)(foundChip.Width * .58), foundChip.Top + (int)(foundChip.Height * .84));
            return ret;
        }
    }
}