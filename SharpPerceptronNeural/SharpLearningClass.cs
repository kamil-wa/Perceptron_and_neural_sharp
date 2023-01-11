using SharpLearning.Containers.Matrices;
using SharpLearning.Neural;
using SharpLearning.Neural.Layers;
using SharpLearning.Neural.Learners;
using SharpLearning.Neural.Loss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpPerceptronNeural
{
    class SharpLearningClass
    {
        public static void WineClassificationWithTest()
        {
            List<Wine> wines = UploadWineData(@".\wine.data");
            int amountOfData = wines.Count;
            int amountOfAttributes = 12; 
            int amountOfClasses = 3;


            F64Matrix observations = new F64Matrix(amountOfData, amountOfAttributes);
            double[] targets = new double[amountOfData];

            Wine refWine;
            for (int i = 0; i < amountOfData; i++)
            {
                refWine = wines[i];
                observations[i, 0] = refWine.Alcohol;
                observations[i, 1] = refWine.MalicAcid;
                observations[i, 2] = refWine.Ash;
                observations[i, 3] = refWine.AlcalinityOfAsh;
                observations[i, 4] = refWine.Magnesium;
                observations[i, 5] = refWine.TotalPhenols;
                observations[i, 6] = refWine.Flavanoids;
                observations[i, 7] = refWine.NonflavanoidPhenols;
                observations[i, 8] = refWine.Proanthocyanins;
                observations[i, 9] = refWine.ColorIntensity;
                observations[i, 10] = refWine.Hue;
                observations[i, 11] = refWine.OdOfDilutedWines;
                //observations[i, 12] = refWine.Proline;
                targets[i] = refWine.ClassType;
            }

            List<Wine> testingSetList = TakeRandomPartOfLearningSet(wines, 0.25);
            F64Matrix testingSet = new F64Matrix(testingSetList.Count, amountOfAttributes);
            double[] testingTargets = new double[testingSetList.Count];

            for (int i = 0; i < testingSetList.Count; i++)
            {
                refWine = testingSetList[i];
                testingSet[i, 0] = refWine.Alcohol;
                testingSet[i, 1] = refWine.MalicAcid;
                testingSet[i, 2] = refWine.Ash;
                testingSet[i, 3] = refWine.AlcalinityOfAsh;
                testingSet[i, 4] = refWine.Magnesium;
                testingSet[i, 5] = refWine.TotalPhenols;
                testingSet[i, 6] = refWine.Flavanoids;
                testingSet[i, 7] = refWine.NonflavanoidPhenols;
                testingSet[i, 8] = refWine.Proanthocyanins;
                testingSet[i, 9] = refWine.ColorIntensity;
                testingSet[i, 10] = refWine.Hue;
                testingSet[i, 11] = refWine.OdOfDilutedWines;
                //testingSet[i, 12] = refWine.Proline;
                testingTargets[i] = refWine.ClassType;
            }

            F64Matrix testingObservations = new F64Matrix(2, amountOfAttributes);
            //2,12.37,.94,1.36,10.6,88,1.98,.57,.28,.42,1.95,1.05,1.82,520
            testingObservations[0, 0] = 12.37;
            testingObservations[0, 1] = 0.94;
            testingObservations[0, 2] = 1.36;
            testingObservations[0, 3] = 10.6;
            testingObservations[0, 4] = 88;
            testingObservations[0, 5] = 1.98;
            testingObservations[0, 6] = 0.57;
            testingObservations[0, 7] = 0.28;
            testingObservations[0, 8] = 0.42;
            testingObservations[0, 9] = 1.95;
            testingObservations[0, 10] = 1.05;
            testingObservations[0, 11] = 1.82;
            //testingObservations[0, 12] = 520;

            //1,14.23,1.71,2.43,15.6,127,2.8,3.06,.28,2.29,5.64,1.04,3.92,1065
            testingObservations[1, 0] = 14.23;
            testingObservations[1, 1] = 1.71;
            testingObservations[1, 2] = 2.43;
            testingObservations[1, 3] = 15.6;
            testingObservations[1, 4] = 127;
            testingObservations[1, 5] = 2.8;
            testingObservations[1, 6] = 3.06;
            testingObservations[1, 7] = 0.28;
            testingObservations[1, 8] = 2.29;
            testingObservations[1, 9] = 5.64;
            testingObservations[1, 10] = 1.04;
            testingObservations[1, 11] = 3.92; 
            //testingObservations[1, 12] = 1065;

            NeuralNet neuralNet = new NeuralNet();

            ILayer layer0 = new InputLayer(amountOfAttributes);
            neuralNet.Add(layer0);
            ILayer layer1 = new DenseLayer(3);
            neuralNet.Add(layer1);
            ILayer layer2 = new DenseLayer(2);
            neuralNet.Add(layer2);
            ILayer layer3 = new SoftMaxLayer(amountOfClasses);
            neuralNet.Add(layer3);

            int netIterations = 10000;
            ILoss logLoss = new LogLoss();
            ClassificationNeuralNetLearner learner =
                new ClassificationNeuralNetLearner(
                    net: neuralNet,
                    loss: logLoss,
                    iterations: netIterations,
                    learningRate: 0.001);

            var model = learner.Learn(observations, targets, testingSet, testingTargets);

            var predictions = model.Predict(testingObservations);

            Console.WriteLine("Trening zakończony");

            for (int i = 0; i < predictions.Length; i++)
            {
                Console.WriteLine(new StringBuilder()
                    .Append("(")
                    .Append(testingObservations[i, 0])
                    .Append("; ")
                    .Append(testingObservations[i, 1])
                    .Append("; ")
                    .Append(testingObservations[i, 2])
                    .Append("; ")
                    .Append(testingObservations[i, 3])
                    .Append("; ")
                    .Append(testingObservations[i, 4])
                    .Append("; ")
                    .Append(testingObservations[i, 5])
                    .Append("; ")
                    .Append(testingObservations[i, 6])
                    .Append("; ")
                    .Append(testingObservations[i, 7])
                    .Append("; ")
                    .Append(testingObservations[i, 8])
                    .Append("; ")
                    .Append(testingObservations[i, 9])
                    .Append("; ")
                    .Append(testingObservations[i, 10])
                    .Append("; ")
                    .Append(testingObservations[i, 11])
                    //.Append("; ")
                    //.Append(testingObservations[i, 12])
                    .Append(")")
                    .Append(" -> ")
                    .Append(predictions[i])
                    .ToString());
            }

            Console.WriteLine("Porównanie ze zbiorem testowym");
            var predictionsForTestingSet = model.Predict(testingSet);
            for (int i = 0; i < testingSet.RowCount; i++)
            {
                Console.WriteLine(new StringBuilder()
                    .Append("(")
                    .Append(testingSet[i, 0])
                    .Append("; ")
                    .Append(testingSet[i, 1])
                    .Append("; ")
                    .Append(testingSet[i, 2])
                    .Append("; ")
                    .Append(testingSet[i, 3])
                    .Append("; ")
                    .Append(testingSet[i, 4])
                    .Append("; ")
                    .Append(testingSet[i, 5])
                    .Append("; ")
                    .Append(testingSet[i, 6])
                    .Append("; ")
                    .Append(testingSet[i, 7])
                    .Append("; ")
                    .Append(testingSet[i, 8])
                    .Append("; ")
                    .Append(testingSet[i, 9])
                    .Append("; ")
                    .Append(testingSet[i, 10])
                    .Append("; ")
                    .Append(testingSet[i, 11])
                    //.Append("; ")
                    //.Append(testingSet[i, 12])
                    .Append(")")
                    .Append(" -> ")
                    .Append(predictionsForTestingSet[i])
                    .Append("(real: ")
                    .Append(testingTargets[i])
                    .Append(")")
                    .Append(predictionsForTestingSet[i] != testingTargets[i] ? " err" : " OK")
                    .ToString());
            }
            Console.ReadLine();
        }

        public static List<Wine> UploadWineData(string path)
        {
            List<Wine> irisFlowers = new List<Wine>();
            Console.WriteLine(path);
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                        irisFlowers.Add(new Wine(line));
                }
            }
            else
            {
                throw new FileLoadException($"Problem with file {path} loading!");
            }
            return irisFlowers;
        }

        public static List<Wine> TakeRandomPartOfLearningSet(List<Wine> list, double part)
        {
            if (part < 0 || part > 1)
                throw new InvalidDataException();
            List<Wine> testingSetList = new List<Wine>();
            double amountOfTestingSet = list.Count * part;
            Random random = new Random();
            for (int i = 0; i < amountOfTestingSet; i++)
            {
                testingSetList.Add(list[random.Next(0, list.Count)]);
            }
            return testingSetList;
        }
    }
}
