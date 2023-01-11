using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpPerceptronNeural
{
    public class Wine
    {
        //Attribute Information:
        /*
             1) Alcohol
 	         2) Malic acid
 	         3) Ash
	         4) Alcalinity of ash  
 	         5) Magnesium
	         6) Total phenols
 	         7) Flavanoids
 	         8) Nonflavanoid phenols
 	         9) Proanthocyanins
	         10)Color intensity
 	         11)Hue
 	         12)OD280/OD315 of diluted wines
 	         13)Proline           
         */

        public double Alcohol { get; set; }
        public double MalicAcid { get; set; }
        public double Ash { get; set; }
        public double AlcalinityOfAsh { get; set; }
        public double Magnesium { get; set; }
        public double TotalPhenols { get; set; }
        public double Flavanoids { get; set; }
        public double NonflavanoidPhenols { get; set; }
        public double Proanthocyanins { get; set; }
        public double ColorIntensity { get; set; }
        public double Hue { get; set; }
        public double OdOfDilutedWines { get; set; }
        public double Proline { get; set; }
        public int ClassType { get; set; }

        public Wine(){}

        public Wine(string a)
        {
            ReadFromString(a);
        }

        public void ReadFromString(string a)
        {
            string[] wineData = a.Split(',');
            if (wineData.Length < 14)
                throw new InvalidDataException($"Wrong data string: {a}");
            Alcohol = double.Parse(wineData[1].Replace(".", ","));
            MalicAcid = double.Parse(wineData[2].Replace(".", ","));
            Ash = double.Parse(wineData[3].Replace(".", ","));
            AlcalinityOfAsh = double.Parse(wineData[4].Replace(".", ","));
            Magnesium = double.Parse(wineData[5].Replace(".", ","));
            TotalPhenols = double.Parse(wineData[6].Replace(".", ","));
            Flavanoids = double.Parse(wineData[7].Replace(".", ","));
            NonflavanoidPhenols = double.Parse(wineData[8].Replace(".", ","));
            Proanthocyanins = double.Parse(wineData[9].Replace(".", ","));
            ColorIntensity = double.Parse(wineData[10].Replace(".", ","));
            Hue = double.Parse(wineData[11].Replace(".", ","));
            OdOfDilutedWines = double.Parse(wineData[12].Replace(".", ","));
            Proline = double.Parse(wineData[13].Replace(".", ","));

            ClassType = int.Parse(wineData[0]);
        }
    }
}
