using System;
using System.Collections;
using System.Diagnostics;

namespace AVSP.Lab1a
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

            var analyzer = new Analyzer(hashes);

            int queryCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < queryCount; i++)
            {
                var parts = Console.ReadLine().Split(' ');
                var targetLine = int.Parse(parts[0]);
                var maxDistance = int.Parse(parts[1]);

                var count = analyzer.GetSimilarCount(targetLine, maxDistance);

                Console.WriteLine(count);
            }

            sw.Stop();
            Console.Error.WriteLine(sw.Elapsed);
        }
    }
}
