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

        private static CascadeClassifier cc = new CascadeClassifier("cascade.xml");

        public ChipFinder(Emgu.CV.Image<Bgr, Byte> source, Emgu.CV.Image<Bgr,Byte> graph)
        {
            sourceImage = source;
            graphImage = graph;
        }

        public void findChips(StackRegistry reg)
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
                    reg.stackFound(rect);
                }

                reg.nextFrame(sourceImage, graphImage);


                foreach (Rectangle rect in rects)
                {
                    diagnosticImage.Draw(rect, new Bgr(255, 255, 255), 2);
                }
                foreach (Stack stack in reg.listPotentialStacks())
                {
                    diagnosticImage.Draw(stack.location, new Bgr(0, 255, 255), 2);
                }
                foreach (Stack stack in reg.listEstablishedStacks())
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
                }
                foreach (Stack stack in reg.listDroppingStacks())
                {
                    diagnosticImage.Draw(stack.location, new Bgr(255, 255, 0), 2);
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
        public int consecutiveFrames;
        public int missedFrames;
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

    public class StackRegistry
    {
        private static int FRAMES_TO_ESTABLISH = 10;
        private static int FRAMES_TO_DROP = 5;

        private ISet<Stack> stacks = new HashSet<Stack>();
        private ISet<Stack> foundThisFrame = new HashSet<Stack>();
        private ISet<Stack> potentialStacks = new HashSet<Stack>();
        private Dictionary<Stack, List<Rectangle>> rawFinds = new Dictionary<Stack, List<Rectangle>>();
    
        public void stackFound(Rectangle rect)
        {
            foreach(Stack stack in stacks)
            {
                if (rect.IntersectsWith(stack.location))
                {
                    Rectangle check = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                    check.Intersect(stack.location);
                    if ((check.Width > stack.location.Width * 0.5) && (check.Height > stack.location.Height * 0.5))
                    {
                        foundThisFrame.Add(stack);
                        if (!rawFinds.ContainsKey(stack)) rawFinds.Add(stack, new List<Rectangle>());
                        rawFinds[stack].Add(rect);
                        return;
                    }
                }
            }
            foreach(Stack stack in potentialStacks)
            {
                if (rect.IntersectsWith(stack.location))
                {
                    Rectangle check = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                    check.Intersect(stack.location);
                    if ((check.Width > stack.location.Width * 0.5) && (check.Height > stack.location.Height * 0.5))
                    {
                        foundThisFrame.Add(stack);
                        if (!rawFinds.ContainsKey(stack)) rawFinds.Add(stack, new List<Rectangle>());
                        rawFinds[stack].Add(rect);
                        return;
                    }
                }
            }
            Stack newStack = new Stack();
            newStack.location = rect;
            newStack.consecutiveFrames = 1;
            potentialStacks.Add(newStack);
        }

        public void nextFrame(Image<Bgr,Byte> img, Image<Bgr,Byte> graph)
        {
            ISet<Stack> missed = new HashSet<Stack>(stacks);
            missed.UnionWith(potentialStacks);
            missed.ExceptWith(foundThisFrame);
            foreach(Stack stack in missed)
            {
                if(stack.missedFrames >= FRAMES_TO_DROP)
                {
                    if(stacks.Contains(stack))
                    {
                        graph.FillConvexPoly(
                            new Point[]{
                                new Point(0,0),
                                new Point(graph.Width,0),
                                new Point(graph.Width,graph.Height),
                                new Point(0,graph.Height)
                            }, new Bgr(Color.Black));
                    }
                    stacks.Remove(stack);
                    potentialStacks.Remove(stack);
                }
                else
                {
                    stack.missedFrames += 1;
                    stack.consecutiveFrames = 0;
                }
            }
            foreach(Stack stack in foundThisFrame)
            {
                stack.consecutiveFrames = stack.consecutiveFrames>=256?256:stack.consecutiveFrames+1;
                stack.missedFrames = 0;
                if(stack.consecutiveFrames > FRAMES_TO_ESTABLISH)
                {
                    stacks.Add(stack);
                    potentialStacks.Remove(stack);
                    Color c = img.Bitmap.GetPixel(stack.location.X + stack.location.Width / 2, stack.location.Y + stack.location.Height / 2);

                    graph.Draw(new CircleF(new PointF((graph.Width / 360) * c.GetHue(), graph.Height - (graph.Height * c.GetSaturation())), 2), new Bgr(c.GetBrightness()*255, c.GetBrightness()*255, c.GetBrightness()*255), 2);

                    if (c.GetBrightness() < 0.25 && c.GetSaturation() < 0.3)
                    {
                        stack.color = Stack.Color.Black;
                    } else if(c.GetBrightness() > 0.7)
                    {
                        stack.color = Stack.Color.White;
                    }
                    else
                    {
                        if (c.GetHue() > 330 || c.GetHue() < 30)
                        {
                            stack.color = Stack.Color.Red;
                        }
                        else if (c.GetHue() >= 210 && c.GetHue() <= 270 && c.GetSaturation() > 0.5)
                        {
                            stack.color = Stack.Color.Blue;
                        }
                        else if (c.GetHue() >= 90 && c.GetHue() <= 200)
                        {
                            stack.color = Stack.Color.Green;
                        }
                        else if (c.GetHue() > 180 && c.GetHue() < 215)
                        {
                            stack.color = Stack.Color.White;
                        }
                    }
                }
            }
            foundThisFrame.Clear();
            foreach(Stack stack in rawFinds.Keys)
            {
                int count = rawFinds[stack].Count;
                int widthTotal = 0;
                int heightTotal = 0;
                int centerXTotal = 0;
                int centerYTotal = 0;
                foreach(Rectangle rect in rawFinds[stack])
                {
                    widthTotal+=rect.Width;
                    heightTotal+=rect.Height;
                    centerXTotal+=rect.X+(rect.Width/2);
                    centerYTotal+=rect.Y+(rect.Height/2);
                }
                stack.location.Width = widthTotal / count;
                stack.location.Height = heightTotal / count;
                stack.location.X = (centerXTotal / count) - (stack.location.Width / 2);
                stack.location.Y = (centerYTotal / count) - (stack.location.Height / 2);
            }
            rawFinds.Clear();
        }

        public ISet<Stack> listPotentialStacks()
        {
            ISet<Stack> ret = new HashSet<Stack>();
            foreach (Stack stack in potentialStacks)
            {
                ret.Add(stack);
            }
            return ret;
        }

        public ISet<Stack> listEstablishedStacks()
        {
            ISet<Stack> ret = new HashSet<Stack>();
            foreach (Stack stack in stacks)
            {
                if (stack.missedFrames == 0)
                    ret.Add(stack);
            }
            return ret;
        }

        public ISet<Stack> listDroppingStacks()
        {
            ISet<Stack> ret = new HashSet<Stack>();
            foreach (Stack stack in stacks)
            {
                if (stack.missedFrames > 0)
                    ret.Add(stack);
            }
            return ret;
        }
    }
}