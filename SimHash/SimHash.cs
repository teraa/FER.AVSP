﻿using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

#nullable enable // Remove when done

namespace AVSP.Lab1a
{
    public class SimHash : IDisposable
    {
        private readonly MD5 _md5;

        public SimHash()
        {
            _md5 = MD5.Create();
        }

        public Encoding Encoding { get; init; } = Encoding.UTF8;

        public BitArray ComputeHash(string input)
        {
            int[] sh = new int[_md5.HashSize];
            string[] words = input.Split(' ');

            foreach (var word in words)
            {
                byte[] wordBytes = Encoding.GetBytes(word);
                byte[] wordHash = _md5.ComputeHash(wordBytes);
                BitArray wordHashBits = new BitArray(wordHash);

                for (int i = 0; i < wordHashBits.Length; i++)
                {
                    if (wordHashBits[i])
                        sh[i]++;
                    else
                        sh[i]--;
                }
            }

            BitArray result = new BitArray(_md5.HashSize);
            for (int i = 0; i < sh.Length; i++)
                result[i] = sh[i] >= 0;

            return result;
        }

        public void Dispose()
        {
            ((IDisposable)_md5).Dispose();
        }

        static byte GetDistance(BitArray a, BitArray b)
        {
            byte result = 0;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] ^ b[i])
                    result++;
            }

            return result;
        }
    }
}
