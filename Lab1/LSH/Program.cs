using System;
using System.Collections;
using System.Diagnostics;
using AVSP.Lab1a;

namespace AVSP.Lab1b
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            int lineCount = int.Parse(Console.ReadLine());
            var hashes = new BitArray[lineCount];
            using (var simHash = new SimHash())
                for (int i = 0; i < hashes.Length; i++)
                    hashes[i] = simHash.ComputeHash(Console.ReadLine());

            var lsh = new LSH(hashes);

            int queryCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < queryCount; i++)
            {
                var parts = Console.ReadLine().Split(' ');
                var targetLine = int.Parse(parts[0]);
                var maxDistance = int.Parse(parts[1]);

                var count = lsh.GetSimilarCount(targetLine, maxDistance);

                Console.WriteLine(count);
            }
            sw.Stop();
            Console.Error.WriteLine(sw.Elapsed);
        }
    }
}
