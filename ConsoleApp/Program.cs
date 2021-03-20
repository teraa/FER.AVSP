using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AVSP.Lab1a;

namespace AVSP
{
    class Program
    {
        static void Main(string[] args)
        {
            using var simHash = new SimHash();
            var hash = simHash.ComputeHash("fakultet elektrotehnike i racunarstva");
            var bytes = new byte[hash.Length / 8];
            hash.CopyTo(bytes, 0);
            Console.WriteLine(BitConverter.ToString(bytes));
        }
    }
}
