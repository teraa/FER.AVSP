using System;
using System.Diagnostics;

namespace AVSP.Lab3
{
    class Program
    {
        private const char SEP = ' ';

        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            ReadOnlySpan<char> span = Console.ReadLine();
            int idx = span.IndexOf(SEP);
            int n = int.Parse(span.Slice(0, idx));  // <= 100, rows
            int m = int.Parse(span.Slice(idx + 1)); // <= 100, cols

            // read n x m matrix
            int[][] matrix = new int[n][];
            for (int x = 0; x < n; x++)
            {
                matrix[x] = new int[m];
                span = Console.ReadLine();
                for (int y = 0; y < m; y++)
                {
                    idx = span.IndexOf(SEP);
                    ReadOnlySpan<char> part;
                    if (idx == -1)
                    {
                        part = span;
                        span = ReadOnlySpan<char>.Empty;
                    }
                    else
                    {
                        part = span.Slice(0, idx);
                        span = span.Slice(idx + 1);
                    }

                    if (part.Length != 1 || part[0] != 'X')
                        matrix[x][y] = int.Parse(part);
                }
            }

            int q = int.Parse(Console.ReadLine()); // [1, 100], number of queries

            for (int x = 0; x < q; x++)
            {
                span = Console.ReadLine();
                int[] queryParts = new int[4];
                for (int y = 0; y < queryParts.Length; y++)
                {
                    idx = span.IndexOf(SEP);
                    ReadOnlySpan<char> part;
                    if (idx == -1)
                    {
                        part = span;
                        span = ReadOnlySpan<char>.Empty;
                    }
                    else
                    {
                        part = span.Slice(0, idx);
                        span = span.Slice(idx + 1);
                    }

                    queryParts[y] = int.Parse(part);
                }

                int i = queryParts[0]; // [1, N], item
                int j = queryParts[1]; // [1, M], user
                int t = queryParts[2]; // {0, 1}, algorithm type, 0 = item-item, 1 = user-user
                int k = queryParts[3]; // [1, min(N,M)]
            }

            sw.Stop();
            Console.Error.WriteLine(sw.Elapsed);
        }
    }
}
