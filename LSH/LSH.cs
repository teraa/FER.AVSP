using System;
using System.Collections;
using System.Collections.Generic;
using AVSP.Lab1a;

namespace AVSP.Lab1b
{
    public class LSH
    {
        private const int HASH_SIZE = 128;
        private const int BAND_COUNT = 8;

        private readonly BitArray[] _hashes;
        private readonly HashSet<int>[] _kandidati;

        public LSH(BitArray[] hashes)
        {
            _hashes = hashes;

            var hashBytes = new byte[hashes.Length][];
            for (int i = 0; i < hashes.Length; i++)
            {
                hashBytes[i] = new byte[HASH_SIZE / 8];
                hashes[i].CopyTo(hashBytes[i], 0);
            }

            _kandidati = new HashSet<int>[hashes.Length];

            for (int b = 0; b < BAND_COUNT; b++)
            {
                var pretinci = new Dictionary<int, HashSet<int>>();

                for (int i = 0; i < hashes.Length; i++)
                {
                    int val = BitConverter.ToUInt16(hashBytes[i], b * 2);

                    var tekstoviUPretincu = new HashSet<int>();
                    if (pretinci.TryGetValue(val, out var pretinac) && pretinac.Count > 0)
                    {
                        tekstoviUPretincu = pretinac;
                        foreach (var id in tekstoviUPretincu)
                        {
                            if (_kandidati[i] == null)
                                _kandidati[i] = new HashSet<int>();
                            _kandidati[i].Add(id);

                            if (_kandidati[id] == null)
                                _kandidati[id] = new HashSet<int>();
                            _kandidati[id].Add(i);
                        }
                    }

                    tekstoviUPretincu.Add(i);
                    pretinci[val] = tekstoviUPretincu;
                }
            }
        }

        public int GetSimilarCount(int targetLine, int maxDistance)
        {
            BitArray targetHash = _hashes[targetLine];

            int result = 0;
            foreach (int i in _kandidati[targetLine])
                if (i != targetLine && HashUtils.GetDistance(targetHash, _hashes[i]) <= maxDistance)
                    result++;

            return result;
        }
    }
}
