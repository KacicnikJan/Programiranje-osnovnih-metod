using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;


namespace kacicnik_vaja1
{
    class Program
    {
        public static List<string[]> readCSV(String filename)
        {
            List<string[]> result = new List<string[]>();
            try
            {
                string[] vrstice = File.ReadAllLines(filename);
                foreach (string vrstica in vrstice)
                {
                    string[] vrednosti = vrstica.Split(',');
                    result.Add(vrednosti);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Napaka pri branju: '{0}'", e);
            }
            return result;
        }
        public static string optimis(List<string[]> podatki){

            double optimisticni = double.MinValue;
            string opt_str = null;

            for (int i = 0; i < podatki.Count; i++)
            {
                if (i > 0)
                {
                    for (int j = 0; j < podatki.ElementAt(i).Length; j++)
                    {
                        if (j > 0)
                        {
                            double trenutni = double.Parse(podatki.ElementAt(i)[j]);
                            if (trenutni > optimisticni){
                                optimisticni = trenutni;
                                opt_str = podatki.ElementAt(0)[j];
                            }
                        }

                    }
                }

            }   
            return String.Format("{0, -22}", "Optimist:") + opt_str + "(" + optimisticni + ")";
        }


        public static string pesimist(List<string[]> podatki)
        {

            double pesimisticni = double.MaxValue;
            string pes_str = null;

            List<Double> manjse = new List<double>(); 

            for (int i = 0; i < podatki.Count - 1; i++)
            {
                if (i > 0)
                {
                    for (int j = 0; j < podatki.ElementAt(i).Length; j++)
                    {
                        if (j > 0)
                        {
                            double trenutni = double.Parse(podatki.ElementAt(i)[j]);
                            double naslednji = double.Parse(podatki.ElementAt(i + 1)[j]);
                            if (trenutni < naslednji)
                                manjse.Add(trenutni);
                            else
                                manjse.Add(naslednji);
                        }

                    }
                }
            }
            pesimisticni = manjse.Max();
            pes_str = podatki.ElementAt(0)[manjse.IndexOf(pesimisticni)+1];

            return String.Format("{0, -22}", "Pesimist:") + pes_str + "(" + pesimisticni + ")";
        }

        public static string laplace(List<string[]> podatki)
        {

            double laplace_max = double.MaxValue;
            string lap_str = null;

            List<Double> povp_list = new List<double>();

            for (int i = 0; i < podatki.Count - 1; i++)
            {
                if (i > 0)
                {
                    for (int j = 0; j < podatki.ElementAt(i).Length; j++)
                    {
                        if (j > 0)
                        {
                            double trenutni = double.Parse(podatki.ElementAt(i)[j]);
                            double naslednji = double.Parse(podatki.ElementAt(i + 1)[j]);
                            double povprecje = (trenutni + naslednji) / 2;
                            povp_list.Add(povprecje);
                        }

                    }
                }
            }
            laplace_max = povp_list.Max();
            lap_str = podatki.ElementAt(0)[povp_list.IndexOf(laplace_max) + 1];

            return String.Format("{0, -22}", "Laplace:") + lap_str + "(" + laplace_max + ")";
        }

        public static string najmanjse_obzalovanje(List<string[]> podatki)
        {

            double najmanjse_min = double.MaxValue;
            string naj_str = null;
            double najvecji_vrstica = double.MinValue;
            int max_podatkov = podatki.ElementAt(0).Length;

            int stolpci = podatki.Count - 1;
            int vrstice = podatki.ElementAt(0).Length - 1;

            double [,] obzalovanje_razlika = new double [stolpci, vrstice];

            List<double> vecji = new List<double>();

            for (int i = 0; i < podatki.Count ; i++)
            {
                if (i > 0)
                {
                    for (int j = 0; j < podatki.ElementAt(i).Length; j++)
                    {
                        if (j > 0)
                        {
                            double trenutni = double.Parse(podatki.ElementAt(i)[j]);
                            if (najvecji_vrstica < trenutni)
                            {
                                najvecji_vrstica = trenutni;
                            }
                        }
                    }


                    for (int x = 0; x < podatki.ElementAt(i).Length; x++)
                    {
                        if (x > 0)
                        {
                            obzalovanje_razlika[i-1, x - 1] = najvecji_vrstica - double.Parse(podatki.ElementAt(i)[x]);
                        }
                    }
                    najvecji_vrstica = double.MinValue;
                }
            }

            for (int i = 0; i < stolpci - 1; i++) {
                for (int j = 0; j < vrstice; j++) {
                    if (obzalovanje_razlika[i, j] > obzalovanje_razlika[i + 1, j]) 
                        vecji.Add(obzalovanje_razlika[i, j]);
                    else
                        vecji.Add(obzalovanje_razlika[i + 1, j]);

                }
            }
            najmanjse_min = vecji.Min();
            naj_str = podatki.ElementAt(0)[vecji.IndexOf(najmanjse_min) + 1];

            return String.Format("{0, -22}", "Najmanjse obzalovanje:") + naj_str + "(" + najmanjse_min + ")";
        }

        public static List<double[]> Hurwitz(List<string[]> podatki)
        {

            List<double> razlike_list = new List<double>();
            List<double> minimumi = new List<double>();
            List<double[]> vrednosti = new List<double[]>();
            double povecava = 0;

            double[] vmesne_v = new double [podatki.ElementAt(0).Length];
            

            for (int i = 0; i < podatki.Count; i++)
            {
                if (i > 0)
                {
                    for (int j = 0; j < podatki.ElementAt(i).Length; j++)
                    {
                        if (j > 0 && podatki.Count - 1 > i)
                        {
                            double razlika = 0;
                            double trenutni = double.Parse(podatki.ElementAt(i)[j]);
                            double naslednji = double.Parse(podatki.ElementAt(i + 1)[j]);
                            if (trenutni > naslednji)
                            {
                                razlika = trenutni - naslednji;
                                minimumi.Add(naslednji);
                            }
                            else
                            { 
                                razlika = naslednji - trenutni;
                                minimumi.Add(trenutni);
                            }
                            razlike_list.Add(razlika);
                        }
                    }
                }
            }


            while (povecava <= 1)
            {
                vmesne_v = new double[podatki.ElementAt(0).Length];
                vmesne_v[0] = povecava;
                for (int j = 0; j < podatki.ElementAt(0).Length - 1; j++)
                {
                    vmesne_v[j+1] = minimumi.ElementAt(j) + razlike_list.ElementAt(j) * povecava;
                }
                povecava += 0.1; 
                vrednosti.Add(vmesne_v);
            }

            return vrednosti;
        }

        static private void graf(List<double[]> podatki, List<string[]> prebrano)
        {
            Chart graf = new Chart();
            graf.Width = 1000;
            graf.Height = 600;
            graf.ChartAreas.Add(new ChartArea("Main"));
            graf.Titles.Add("Vrednosti s Hurwiczevim kriterijem");

            for (int i = 0; i < podatki.Count; i++)
            {               
                for (int j = 0; j < podatki.ElementAt(i).Length; j++)
                {
                 if(i == 0 && j > 0)
                 {
                        graf.Series.Add(prebrano.ElementAt(0)[j]);
                        graf.Series[prebrano.ElementAt(0)[j]].ChartType = SeriesChartType.Spline;
                 }
                  if(j > 0)
                    graf.Series[prebrano.ElementAt(0)[j]].Points.AddXY(podatki.ElementAt(i)[0],podatki.ElementAt(i)[j]);
                }
            }
            graf.Legends.Add(new Legend("legenda"));
            graf.SaveImage("mychart.png", System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
        }

        static void Main(string[] args)
        {
            List<string[]> datoteka = new List<string[]>();
            datoteka = readCSV("osnovne_metode1.csv");

            Console.WriteLine(optimis(datoteka));

            Console.WriteLine(pesimist(datoteka));

            Console.WriteLine(laplace(datoteka));

            Console.WriteLine(najmanjse_obzalovanje(datoteka));

            List<double[]> dobljeni_p = new List<double[]>();
            dobljeni_p = Hurwitz(datoteka);

            Console.WriteLine("\nHurwitzev kriterij:");

            for (int i = 0; i < datoteka.ElementAt(0)[0].Length; i++)
            {
                if(i > 0)
                    Console.Write(String.Format("{0, -12}", datoteka.ElementAt(0)[i]));
                else
                    Console.Write(String.Format("{0, -11}", "h"));
            }
            Console.WriteLine();
            for (int i = 0; i < dobljeni_p.Count; i++)
            {
                for (int j = 0; j < dobljeni_p.ElementAt(i).Length; j++)
                {
                    Console.Write(String.Format("{0, -12}", dobljeni_p.ElementAt(i)[j] ));
                }
                Console.WriteLine();

           }

            graf(dobljeni_p, datoteka);
            Console.ReadKey();

        }
    }
}
