using Emgu.CV.Structure;
using NeuralNetworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectChipCounter
{
    class ColorFinder
    {
        private List<DataSet> trainingData = new List<DataSet>();
        private NeuralNetworks.BackPropogationNetwork ann = new NeuralNetworks.BackPropogationNetwork(15, 5, 15, 1);

        public Stack.Color guessColor(Bgr[] colors)
        {
            ann.ApplyInput(convertBgrListToInputArray(colors));
            ann.CalculateOutput();
            IEnumerable<double> output = ann.ReadOutput();
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

        private double[] convertBgrListToInputArray(Bgr[] samples)
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

        private double[] convertChipColorToOutput(Stack.Color chipColor)
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

        public void addTrainingPoint(Bgr[] samples, Stack.Color correctColor)
        {
            DataSet dataSet = new DataSet();
            dataSet.Inputs = convertBgrListToInputArray(samples);
            dataSet.Outputs = convertChipColorToOutput(correctColor);
            trainingData.Add(dataSet);
            retrain();
        }

        public void retrain()
        {
            ann.BatchBackPropogate(trainingData.ToArray(), 1000, 0.1, 0.9);
        }
    }
}
