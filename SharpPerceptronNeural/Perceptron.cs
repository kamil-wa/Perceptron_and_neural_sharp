using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpPerceptronNeural
{
    public class Perceptron
    {
        public double[] weights;
        public double learningRate;
        public bool biOrUni;

        public Perceptron(int numWeights, double learningRate, bool biOrUni)
        {
            this.weights = new double[numWeights];
            this.learningRate = learningRate;
            this.biOrUni = biOrUni;
        }

        public double calculateS(int[] inputs)
        {
            double sum = 0;
            for (int i = 0; i < inputs.Length; i++)
            { 
                sum += inputs[i] * weights[i];
            }

            return sum;
        }

        public int calculateYt(int[] inputs)
        {
            if(biOrUni) return (calculateS(inputs) >= 0) ? 1 : -1;
            else return (calculateS(inputs) >= 0) ? 1 : 0;
        }

        public void trainingThePerceptron(int[] inputs, double expectedOutput)
        {
            double output = calculateYt(inputs);

            weights[0] += learningRate * (expectedOutput - output);
            for (int i = 1; i < inputs.Length; i++)
            {
                weights[i] += learningRate * (expectedOutput - output) * inputs[i];
            }
        }

        public double[] generateDT(int[][] inputData, double a, double b)
        {
            double[] dt = new double[inputData.Length];

            for (int i = 0; i < inputData.Length; i++)
            {
                double x1 = inputData[i][1];
                double x2 = inputData[i][2];

                double output;
                
                if(biOrUni) output = (x2 > a * x1 + b) ? 1 : -1; 
                else output = (x2 > a * x1 + b) ? 1 : 0;

                dt[i] = output;
            }

            return dt;
        }

        public int[][] generateL(int N)
        {
            Random rnd = new Random();


            int[][] L = new int[N][];

            List<HashSet<int>> arr = new List<HashSet<int>>();

            HashSet<List<int>> setk = new HashSet<List<int>>(); 

            while(setk.Count != N)
            {
                List<int> k = new List<int>();
                k.Add(1);
                k.Add(rnd.Next(-10, 10));
                k.Add(rnd.Next(-10, 10));
                setk.Add(k);
            }

            int i = 0;
            foreach (var j in setk)
            {
                L[i] = new int[] { j[0], j[1], j[2] };
                i++;
            }


            /*
            int x2 = 0;
            int x1 = 0;

            for (int i = 0; i < N; i++)
            {
                HashSet<int> set = new HashSet<int>();

                    do
                    {
                        x1 = rnd.Next(-10, 10);
                        x2 = rnd.Next(-10, 10);
                    } while (!arr[x1].Add(x2));
                    arr[x1].Add(x2);

                L[i] = new int[] { 1, x1, x2};
            }*/
            return L;
        }
    }
}
