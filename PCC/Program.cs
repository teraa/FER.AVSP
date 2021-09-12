using System;
using System.Linq;
using static System.Console;

// const int targetIdx = 0;
// double[][] m =
// {
//     new double[]{1,0,3,0,0,5,0,0,5,0,4,0},
//     new double[]{0,0,5,4,0,0,4,0,0,2,1,3},
//     new double[]{2,4,0,1,2,0,3,0,4,3,5,0},
//     new double[]{0,2,4,0,5,0,0,4,0,0,2,0},
//     new double[]{0,0,4,3,4,2,0,0,0,0,2,5},
//     new double[]{1,0,3,0,3,0,0,2,0,0,4,0},
// };

const int targetIdx = 6;

double[][] m =
{
    new double[] {4, 1, 3, 0, 4, 5, 0, 0},
    new double[] {4, 2, 1, 4, 1, 1, 4, 5},
    new double[] {3, 3, 5, 5, 5, 0, 0, 1},
    new double[] {2, 0, 0, 4, 4, 1, 2, 4},
    new double[] {4, 0, 2, 3, 0, 0, 4, 3},
    new double[] {0, 1, 5, 5, 1, 4, 1, 2},
    new double[] {5, 1, 5, 0, 2, 1, 0, 4},
    new double[] {2, 3, 2, 0, 1, 3, 0, 0}
};
Transpose(m);

WriteLine("input:");
Print(m);

double[] avgs = new double[m.Length];

WriteLine("\navg:");
for (int i = 0; i < m.Length; i++)
{
    double avg = m[i].Where(x => x != 0).Average();
    avg = Math.Round(avg, 3);
    avgs[i] = avg;
    WriteLine($"{i + 1}: {avg,5:f3}");
}

WriteLine("\nnormalized:");
double[][] normalized = new double[m.Length][];
for (int i = 0; i < m.Length; i++)
{
    normalized[i] = new double[m[i].Length];

    for (int j = 0; j < m[i].Length; j++)
    {
        ref var value = ref m[i][j];

        normalized[i][j] = value == 0
            ? 0
            : value - avgs[i];
    }
}
Print(normalized);

WriteLine("\npearson sim:");
for (int i = 0; i < normalized.Length; i++)
{
    double sum = 0, sumj = 0, sumk = 0;

    for (int j = 0; j < normalized[i].Length; j++)
    {
        double rik = normalized[targetIdx][j];
        double rij = normalized[i][j];

        sum += rij * rik;
        sumk += Math.Pow(rik, 2);
        sumj += Math.Pow(rij, 2);
    }

    double sim = sum / Math.Sqrt(sumk * sumj);

    const int pad = 7;
    WriteLine($"{i + 1}: sum: {sum,pad:f4}, sumk: {sumk,pad:f4}, sumj: {sumj,pad:f4}, sim: {sim,pad:f4}");
}

static void Transpose<T>(T[][] array)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (array.Length != array[i].Length)
            throw new ArgumentException();

        for (int j = 0; j < i; j++)
        {
            T tmp = array[i][j];
            array[i][j] = array[j][i];
            array[j][i] = tmp;
        }
    }
}

static void Print(double[][] m)
{
    const int precision = 3;
    const int pad = precision + 4;
    const int colOffset = 5;

    Write(new string(' ', colOffset));

    for (int i = 0; i < m.Length; i++)
        Write($"[{i + 1}]".PadLeft(pad));

    WriteLine();

    for (int i = 0; i < m.Length; i++)
    {
        Write($"[{i + 1}]".PadLeft(colOffset));

        for (int j = 0; j < m[i].Length; j++)
        {
            var value = Math.Round(m[i][j], 3);
            Write(value.ToString().PadLeft(pad));
        }

        WriteLine();
    }
}
