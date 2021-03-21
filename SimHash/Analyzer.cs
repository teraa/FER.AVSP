using System;
using System.Collections;
using System.Collections.Generic;

namespace AVSP.Lab1a
{
    public class Analyzer
    {
        private readonly BitArray[] _hashes;

        public Analyzer(BitArray[] hashes)
        {
            _hashes = hashes;
        }

        public Analyzer(IEnumerable<string> lines, int count)
        {
            _hashes = new BitArray[count];

            using var simHash = new SimHash();

            int i = 0;
            foreach (var line in lines)
                _hashes[i++] = simHash.ComputeHash(line);

            if (i != count)
                throw new ArgumentException($"Missing data, expected {_hashes.Length} rows, got {i}");
        }

        public int GetSimilarCount(int targetLine, int maxDistance)
        {
            BitArray targetHash = _hashes[targetLine];
            int result = 0;

            for (int i = 0; i < _hashes.Length; i++)
                if (i != targetLine && HashUtils.GetDistance(targetHash, _hashes[i]) <= maxDistance)
                    result++;

            return result;
        }
    }
}
