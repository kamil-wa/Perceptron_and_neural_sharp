using System;
using System.IO;

namespace SharpPerceptronNeural
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wpisz 1 aby wytrenować SharpNeural lub 2 aby uruchomić Perceptrona:");
            string percOrNeural = Console.ReadLine();


            if (int.Parse(percOrNeural) == 1)
            {
                try
                {
                    SharpLearningClass.WineClassificationWithTest();
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
            }
            else
            {
                //Wczytanie szybkości treningu Perceptrona
                Console.WriteLine("Wpisz szybkość treningu Perceptrona: ");
                string learningSpeedString = Console.ReadLine();
                double learningSpeedValue = double.Parse(learningSpeedString);

                //Wybranie sposobu wprowadzenia wag
                string givenNumber;
                do
                {
                    Console.WriteLine("Wpisz 1, aby wylosować wagi lub 2 by je podać:");
                    givenNumber = Console.ReadLine();
                }
                while (int.Parse(givenNumber) != 1 && int.Parse(givenNumber) != 2);
                string[] weightStrings = null;
                double[] weights = null;
                if (int.Parse(givenNumber) == 1)
                {
                    Console.WriteLine("Podaj przedział, z jakiego mają być wylosowane wagi (pierwsza liczba druga liczba):");
                    string[] weightClamp = Console.ReadLine().Split(' ');
                    int fn = int.Parse(weightClamp[0]);
                    int sn = int.Parse(weightClamp[1]);
                    weights = new double[3];

                    for (int i = 0; i < 3; i++)
                    {
                        Random rnd = new Random();
                        weights[i] = rnd.NextDouble() * (sn - fn) + fn;
                    }

                }
                else if (int.Parse(givenNumber) == 2)
                {
                    Console.WriteLine("Wpisz wartości wag perceptrona (w0 w1 w2):");
                    weightStrings = Console.ReadLine().Split(' ');
                    weights = new double[weightStrings.Length];

                    for (int i = 0; i < weightStrings.Length; i++)
                    {
                        weights[i] = double.Parse(weightStrings[i]);
                    }
                }

                //Wybranie, czy ma być rozpatrywana funkcja bipolarna czy unipolarna?
                Console.WriteLine("Podaj, jaka funkcja ma być rozpatrywana, bipolarna czy unipolarna? (1 - bi, 0 - uni): ");
                string biOrUniText = Console.ReadLine();
                bool biOrUni = false;
                if (biOrUniText == "0") biOrUni = false;
                else if (biOrUniText == "1") biOrUni = true;

                //Stworzenie obiektu perceptrona
                Perceptron p = new Perceptron(3, learningSpeedValue, biOrUni);
                p.weights = weights;

                //Wczytanie wartości a i b dla prostej
                Console.WriteLine("Wpisz wartości treningu funkcji prostej ax + b: ");
                string[] abStrings = Console.ReadLine().Split(' ');
                double a = double.Parse(abStrings[0]);
                double b = double.Parse(abStrings[1]);

                //Wybranie ilości punktów N
                Console.WriteLine("Wpisz, ile chcesz mieć punktów N: ");
                string countOfPoints = Console.ReadLine();
                int N = int.Parse(countOfPoints);

                //Generacja zbioru L lub podanie odgórnych danych - opcjonalne
                //Console.WriteLine("Generacja zbioru, czy podana z góry? (1 - pre, 2 - gen)");
                //string preDatasetOrGenrate = Console.ReadLine();

                int[][] inputData = null;
                //if (int.Parse(preDatasetOrGenrate) == 1)
                //{
                //    inputData = new int[N][];
                //    for (int i = 0; i < N; i++)
                //    {
                //        Console.WriteLine("Podaj " + i + " set danych: ");
                //        string[] setOfData = Console.ReadLine().Split(' ');
                //        int x0 = 1;
                //        int x1 = int.Parse(setOfData[0]);
                //        int x2 = int.Parse(setOfData[1]);
                //        inputData[i] = new int[] { x0, x1, x2 };
                //    }
                //}
                //else if (int.Parse(preDatasetOrGenrate) == 2)
                //{
                inputData = p.generateL(N);
                //}


                //Wygenerowanie d(t)
                double[] expectedOutputs = p.generateDT(inputData, a, b);

                //Utworzenie pliku
                StreamWriter results = new StreamWriter("training.txt");

                //Zapisanie zbioru L przyjaźnie do pliku
                Console.WriteLine("Zbiór uczący L: ");
                results.WriteLine("Zbiór uczący L: ");
                Console.WriteLine();
                results.WriteLine();

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Console.WriteLine("x" + j + " = " + inputData[i][j]);
                        results.WriteLine("x" + j + " = " + inputData[i][j]);
                    }

                    Console.WriteLine();
                    results.WriteLine();
                }

                //Pierwsza linia tabelki jako stałe zdanie
                Console.WriteLine("Epoka    |    t    |    x0(t)    |    x1(t)    |    x2(t)    |    d(t)    |    w0(t)    |    w1(t)    |    w2(t)    |    s(t)    |    y(t)    |    ok?");
                results.WriteLine("Epoka    |    t    |    x0(t)    |    x1(t)    |    x2(t)    |    d(t)    |    w0(t)    |    w1(t)    |    w2(t)    |    s(t)    |    y(t)    |    ok?");

                //Zainicjowanie 1 epocha i zmiennej przechowującej ilości ok na wyjściu, oraz boola wyłączającego trening
                int epoch = 1;
                int okOnOutput = 0;
                bool off = false;
                //Pętla treningu
                do
                {
                    off = true;
                    for (int t = 0; t < N; t++)
                    {
                        Console.WriteLine();
                        results.WriteLine();

                        Console.Write("Epoch: " + epoch + "    |    ");
                        results.Write("Epoch: " + epoch + "    |    ");
                        Console.Write("t: " + t + "    |    ");
                        results.Write("t: " + t + "    |    ");

                        Console.Write("x0(t) = " + inputData[t][0] + "    |    ");
                        results.Write(inputData[t][0] + "    |    ");
                        Console.Write("x1(t) = " + inputData[t][1] + "    |    ");
                        results.Write(inputData[t][1] + "    |    ");
                        Console.Write("x2(t) = " + inputData[t][2] + "    |    ");
                        results.Write(inputData[t][2] + "    |    ");

                        Console.Write("d(t) = " + expectedOutputs[t] + "    |    ");
                        results.Write(expectedOutputs[t] + "    |    ");

                        Console.Write("w0(t) = " + Math.Round(p.weights[0], 2) + "    |    ");
                        results.Write(Math.Round(p.weights[0], 2) + "    |    ");
                        Console.Write("w1(t) = " + Math.Round(p.weights[1], 2) + "    |    ");
                        results.Write(Math.Round(p.weights[1], 2) + "    |    ");
                        Console.Write("w2(t) = " + Math.Round(p.weights[2], 2) + "    |    ");
                        results.Write(Math.Round(p.weights[2], 2) + "    |    ");

                        double signalOutput = p.calculateS(inputData[t]);
                        double output = p.calculateYt(inputData[t]);

                        p.trainingThePerceptron(inputData[t], expectedOutputs[t]);

                        Console.Write("s(t) = " + Math.Round(signalOutput, 2) + "    |    ");
                        results.Write(Math.Round(signalOutput, 2) + "    |    ");
                        Console.Write("y(t) = " + output + "    |    ");
                        results.Write(output + "    |    ");

                        string okOrNotOk;

                        if (output == expectedOutputs[t])
                        {
                            okOrNotOk = "ok";
                            okOnOutput++;
                        }
                        else
                        {
                            okOrNotOk = "---";
                            okOnOutput = 0;
                        }

                        Console.Write("ok? " + okOrNotOk);
                        results.Write(okOrNotOk);

                        if (okOnOutput == N)
                        {
                            off = false;
                        }
                    }
                    epoch++;
                } while (off);
                Console.WriteLine("Program stworzony przez: Arkadiusz Józefczak, Bartosz Jurczyk i Kamil Wątor ");
                results.WriteLine("Program stworzony przez: Arkadiusz Józefczak, Bartosz Jurczyk i Kamil Wątor ");
                Console.WriteLine();
                results.WriteLine();
                Console.WriteLine("Oświadczamy, że kod napisany w tej aplikacji został stworzony samodzielnie i nie został skopiowany ze źródeł internetowych.");
                results.WriteLine("Oświadczamy, że kod napisany w tej aplikacji został stworzony samodzielnie i nie został skopiowany ze źródeł internetowych.");

                results.Close();
            }
        }
    }
}
