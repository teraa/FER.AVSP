using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
            int[,] matrix = new int[n, m];
            for (int x = 0; x < n; x++)
            {
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
                        matrix[x, y] = int.Parse(part);
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

                int i = queryParts[0] - 1; // [1, N] - 1, item
                int j = queryParts[1] - 1; // [1, M] - 1, user
                int t = queryParts[2]; // {0, 1}, algorithm type, 0 = item-item, 1 = user-user
                int k = queryParts[3]; // [1, min(N,M)]

                double result = Math.Round(Query(matrix, i, j, t, k), 3, MidpointRounding.AwayFromZero);
                Console.WriteLine(result.ToString("F3"));
            }

            sw.Stop();
            Console.Error.WriteLine(sw.Elapsed);
        }

        static T[,] Transpose<T>(T[,] input)
        {
            int rows = input.GetLength(0);
            int cols = input.GetLength(1);
            T[,] result = new T[cols, rows];
            for (int x = 0; x < rows; x++)
                for (int y = 0; y < cols; y++)
                    result[y, x] = input[x, y];

            return result;
        }

        static float Query(int[,] inputMatrix, int i, int j, int t, int k) // TODO: rename inputMatrix to matrix
        {
            if (t == 1)
            {
                inputMatrix = Transpose(inputMatrix);
                (i, j) = (j, i);
            }

            float[][] matrix = Normalize(inputMatrix);


            int rows = inputMatrix.GetLength(0);
            int cols = inputMatrix.GetLength(1);

            Dictionary<int, float> similarities = new Dictionary<int, float>();

            for (int x = 0; x < rows; x++)
                if (x != i)
                    similarities[x] = SimCosine(matrix[i], matrix[x]);

            int count = 0;
            float sumA = 0;
            float sumB = 0;

            foreach (var pair in similarities.OrderByDescending(x => x.Value))
            {
                int row = pair.Key;
                float similarity = pair.Value;

                var elem = inputMatrix[row, j];
                if (elem != 0 && similarity >= 0)
                {
                    sumA += similarity * elem;
                    sumB += similarity;

                    count++;
                    if (count == k) break;
                }
            }

            return sumA / sumB;
        }

        static float[][] Normalize(int[,] input)
        {
            int rows = input.GetLength(0);
            int cols = input.GetLength(1);
            float[][] result = new float[rows][];

            for (int x = 0; x < rows; x++)
            {
                result[x] = new float[cols];

                int sum = 0;
                int count = 0;

                for (int y = 0; y < cols; y++)
                {
                    if (input[x, y] != 0)
                    {
                        sum += input[x, y];
                        count++;
                    }
                }

                float avg = (float)sum / count;

                for (int y = 0; y < cols; y++)
                    if (input[x, y] != 0)
                        result[x][y] = input[x, y] - avg;
            }

            return result;
        }

        static float DotProduct(float[] a, float[] b)
        {
            float result = 0;

            for (int i = 0; i < a.Length; i++)
                result += a[i] * b[i];

            return result;
        }

        static float SimCosine(float[] a, float[] b)
        {
            return DotProduct(a, b) / (float)(Math.Sqrt(DotProduct(a, a) * DotProduct(b, b)));
        }
    }
}
