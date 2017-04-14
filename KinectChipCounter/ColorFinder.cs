using Emgu.CV;
using Emgu.CV.Structure;
using NeuralNetworks;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace KinectChipCounter
{
    class ColorFinder
    {
        private static volatile ColorFinder instance;
        private static object sync = new Object();

        private static ColorFinder Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                            instance = new ColorFinder();
                    }
                }
                return instance;
            }
        }

        private List<DataSet> trainingData = new List<DataSet>();
        private NeuralNetworks.BackPropogationNetwork ann = new NeuralNetworks.BackPropogationNetwork(15, 5, 15, 1);

        private ColorFinder() { }

        public static Stack.Color guessColor(Bgr[] colors)
        {
            Instance.ann.ApplyInput(convertBgrListToInputArray(colors));
            Instance.ann.CalculateOutput();
            IEnumerable<double> output = Instance.ann.ReadOutput();
            int index = 0;
            int greatestIndex = -1;
            double greatest = -1;
            foreach(double outval in output)
            {
                if(outval>greatest)
                {
                    greatestIndex = index;
                    greatest = outval;
                }
                index++;
            }
            return (Stack.Color)(Enum.GetValues(typeof(Stack.Color)).GetValue(greatestIndex));
        }

        private static double[] convertBgrListToInputArray(Bgr[] samples)
        {
            List<double> colorChannelNormalized = new List<double>();
            foreach (Bgr sample in samples)
            {
                colorChannelNormalized.Add(sample.Red / 255.0);
                colorChannelNormalized.Add(sample.Green / 255.0);
                colorChannelNormalized.Add(sample.Blue / 255.0);
            }
            return colorChannelNormalized.ToArray();
        }

        private static double[] convertChipColorToOutput(Stack.Color chipColor)
        {
            List<double> correctOutputs = new List<double>();
            foreach(Stack.Color color in Enum.GetValues(typeof(Stack.Color)))
            {
                if(color.Equals(chipColor))
                {
                    correctOutputs.Add(1.0);
                } else
                {
                    correctOutputs.Add(0.0);
                }
            }
            return correctOutputs.ToArray();
        }

        public static void addTrainingPoint(Bgr[] samples, Stack.Color correctColor)
        {
            DataSet dataSet = new DataSet();
            dataSet.Inputs = convertBgrListToInputArray(samples);
            dataSet.Outputs = convertChipColorToOutput(correctColor);
            Instance.trainingData.Add(dataSet);
            retrain();
        }

        public static void retrain()
        {
            Instance.ann.BatchBackPropogate(Instance.trainingData.ToArray(), 1000, 0.1, 0.9);
        }

        public static void trainChip(Image<Bgr, Byte> img, Point[] samplePoints, Stack.Color color)
        {
            List<Bgr> colorSamples = new List<Bgr>();
            foreach (Point point in samplePoints)
            {
                colorSamples.Add(img[point]);
            }
            ColorFinder.addTrainingPoint(colorSamples.ToArray(), color);
        }
    }
}
