using System;
using System.Collections;
using System.Text;

namespace AVSP.Lab1a
{
    public static class HashUtils
    {
        public static int GetDistance(BitArray array1, BitArray array2)
        {
            if (array1.Length != array2.Length) throw new ArgumentException("Arrays must be of same size.");

            int result = 0;

            for (int i = 0; i < array1.Length; i++)
                if (array1[i] ^ array2[i])
                    result++;

            return result;
        }

        public static bool IsMaxDistance(BitArray array1, BitArray array2, int maxDistance)
        {
            if (array1.Length != array2.Length) throw new ArgumentException("Arrays must be of same size.");

            int result = 0;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] ^ array2[i])
                {
                    result++;

                    if (result > maxDistance)
                        break;
                }
            }

            return result <= maxDistance;
        }

        public static string HashToString(BitArray hash)
        {
            if (hash.Length % 8 != 0)
                throw new ArgumentException("Length is not a multiple of 8");

            var array = new byte[hash.Length / 8];
            hash.CopyTo(array, 0);

            return HashToString(array);
        }

        public static string HashToString(byte[] hash)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
