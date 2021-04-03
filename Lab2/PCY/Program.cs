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

            int[][] košare = new int[brKošara][];
            for (int i = 0; i < brKošara; i++)
            {
                string line = Console.ReadLine();
                int[] košara = line.Split(' ').Select(int.Parse).ToArray();
                košare[i] = košara;
            }

            var brPredmetaMap = new Dictionary<int, int>();

            // prvi prolaz
            for (int i = 0; i < košare.Length; i++)
                for (int j = 0; j < košare[i].Length; j++)
                    if (!brPredmetaMap.TryAdd(košare[i][j], 1))
                        brPredmetaMap[košare[i][j]]++;

            // map to array
            int[] brPredmeta = new int[brPredmetaMap.Count];
            for (int i = 1; i <= brPredmeta.Length; i++)
                brPredmeta[i - 1] = brPredmetaMap[i];

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

            int p = parovi.Count;

            Console.WriteLine(a);
            Console.WriteLine(p);
            foreach (var x in parovi.Values.OrderByDescending(v => v))
                Console.WriteLine(x);

            sw.Stop();
            Console.Error.WriteLine(sw.Elapsed);
        }
    }
}
