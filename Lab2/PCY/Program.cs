using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AVSP.Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            int brKošara = int.Parse(Console.ReadLine());
            float s = float.Parse(Console.ReadLine());
            float prag = s * brKošara;
            int brPretinaca = int.Parse(Console.ReadLine());

            int max = 0;
            int[][] košare = new int[brKošara][];
            for (int i = 0; i < brKošara; i++)
            {
                string[] parts = Console.ReadLine().Split(' ');
                int[] košara = new int[parts.Length];
                for (int j = 0; j < parts.Length; j++)
                {
                    košara[j] = int.Parse(parts[j]);
                    if (max < košara[j])
                        max = košara[j];
                }
                košare[i] = košara;
            }

            // predmeti su brojevi [1, max], spremljeni u polje s indeksima [0, max-1]
            int[] brPredmeta = new int[max];
            foreach (var košara in košare)
                foreach (var predmet in košara)
                    brPredmeta[predmet - 1]++;

            // drugi prolaz - sažimanje
            int[] pretinci = new int[brPretinaca];

            foreach (var košara in košare)
            {
                for (int i = 0; i < košara.Length - 1; i++)
                {
                    int pi = košara[i] - 1;

                    for (int j = i + 1; j < košara.Length; j++)
                    {
                        int pj = košara[j] - 1;

                        if (brPredmeta[pi] >= prag && brPredmeta[pj] >= prag)
                        {
                            int k = ((pi * brPredmeta.Length) + pj) % brPretinaca;
                            pretinci[k]++;
                        }
                    }
                }
            }

            // treći prolaz - brojanje parova
            var parovi = new Dictionary<(int, int), int>();

            foreach (var košara in košare)
            {
                for (int i = 0; i < košara.Length - 1; i++)
                {
                    int pi = košara[i] - 1;

                    for (int j = i + 1; j < košara.Length; j++)
                    {
                        int pj = košara[j] - 1;

                        if (brPredmeta[pi] >= prag && brPredmeta[pj] >= prag)
                        {
                            int k = ((pi * brPredmeta.Length) + pj) % brPretinaca;
                            if (pretinci[k] >= prag)
                            {
                                var tuple = (pi, pj);
                                if (!parovi.TryAdd(tuple, 1))
                                    parovi[tuple]++;
                            }
                        }
                    }
                }
            }

            int m = 0;
            for (int i = 0; i < brPredmeta.Length; i++)
                if (brPredmeta[i] >= prag)
                    m++;

            int a = m * (m - 1) / 2;

            Console.WriteLine(a);
            Console.WriteLine(parovi.Count);
            foreach (var x in parovi.Values.OrderByDescending(v => v))
                Console.WriteLine(x);

            sw.Stop();
            Console.Error.WriteLine(sw.Elapsed);
        }
    }
}
