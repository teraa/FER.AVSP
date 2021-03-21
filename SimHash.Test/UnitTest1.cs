using System;
using System.Collections;
using Xunit;

namespace AVSP.Lab1a
{
    public class UnitTest1 : IDisposable
    {
        private readonly SimHash _simHash;

        public UnitTest1()
        {
            _simHash = new SimHash();
        }

        public void Dispose()
        {
            ((IDisposable)_simHash).Dispose();
        }

        [Fact]
        public void ExampleTest1()
        {
            string input = "fakultet elektrotehnike i racunarstva";
            BitArray hash = _simHash.ComputeHash(input);
            string hashString = HashUtils.HashToString(hash);
            Assert.Equal("f27c6b49c8fcec47ebeef2de783eaf57", hashString);
        }

        [Theory]
        [InlineData(0x00, 0x00, 0)]
        [InlineData(0x00, 0x01, 1)]
        [InlineData(0x80, 0x01, 2)]
        [InlineData(0xFF, 0x00, 8)]
        public void DistanceTest(byte a, byte b, int exprectedResult)
        {
            var arrA = new BitArray(new [] { a });
            var arrB = new BitArray(new [] { b });

            var distance = HashUtils.GetDistance(arrA, arrB, arrA.Count);

            Assert.Equal(exprectedResult, distance);
        }
    }
}
