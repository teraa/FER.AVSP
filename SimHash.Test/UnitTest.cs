using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AVSP.Lab1a
{
    public class UnitTest : IDisposable
    {
        private readonly SimHash _simHash;

        public UnitTest()
        {
            _simHash = new SimHash();
        }

        public void Dispose()
        {
            ((IDisposable)_simHash).Dispose();
        }

        [Theory]
        [InlineData("fakultet elektrotehnike i racunarstva", "f27c6b49c8fcec47ebeef2de783eaf57")]
        [InlineData("wwrajjaozr mthvbqaljd yorrttizjq qsjlyddvnz vpkqrfcbie ryyuoujgup fvqsdiswox zycwgqzgoj ntmnipkzii ljrjqcnspo rrlsflrxus klshxpkjqt mymybkddfq nlqjktglfz udiviecqsy rtamvjwoww gbhplpiwlz mmfrjosglr xarighwilg vtxudgxzuc fkumtoynun mibhnyaxlj eqfywcfjox zdrznbprss rkdpufvbjz dsvvaqznyj ozxkknquwi kpopfzigbq iaaaonoiql peltvtztla dsejlwfkns yvmromxggk vrcrialkmi amubdnxitp qtpxmbvstd nvomaouwfj lifltqcbye cwqmyjyxff uknctbflma cetwmuovai xnvjdrijrg ftukvtceul ucfhfkgdhd alfwadzpuq pztomgmxts bgiasvattz nlmwvkyrog hchppzkyhk gbjturdhrv dcxzvbmion adsqktlgao vsfrbwppep uvcaclhjkx ybmoilicri hevxcztxkr avyquayzor yeeoyhltcj pxakzhpvhv iysrmnicvf cdtxwquqke jffnqwaoql avbicvwpqh jtfjzroybp txkoidzkhj jqnxxxeftd akplyaoupa bnifrveikw lbfnghqdck wmqvgxbxvf zsvatibqbi hcbmyfowey fxtrwcnrzd smqjrekdtu ifzdxygfki gqoqxzwxuc bwbsoelnlc prvqmmgduh wtkdzrzpfz jdoymgkhsa rcoozjyjbv dnnqsgmgtu phqsmmofks ldnurswtkn bogwvaruwj keirccsrgx vslqbiykse kijnhihbwb bqfwsllnwo byolxrbdkl rmqqyfdhyh beeekmfppt evvbuhsiil eogccqgwpg rjvsohyvwm tpeubpifne ycoahirwyu xxszzntuge dzzjzakfgm ghozvkzztl hfgpibsmom ",
            "eaefd0662fdae21f7c8daaf2d12bc1b5")]
        [InlineData("vbhozztvgk domtzqohxn dsfvbsyyjy zultmogmsb wwshvwfyvl pooutbjwfy oeepyhizht awyltkokty uedtvvtcqz pqjeagvnep eoneoicovg bewdkfjlsq swwvpnsznq cnnkgfyain dgrpixcnhw rxptgfgnhe useunxtieh ztmznbbozd xehtobhhaz shlgsychsm ztrhwhyntc qftpfzjtbx zduunjvhuk pgqvujloms lwtffbzdyy illvmawaxy oevkjbdbyw cuccuqeecu ftwmvbtddk anwryareal zwpsqwfzag vnfzymclpg amqtkvnecs jhgrqsutea vpustxsgyj nqgdxsenwd tsuogcyjcb tnwjucckae jrwhuocket patgjvbole nrmebsaaut fyjppljkua doymdrjzwr icasowyhpo cbstkrzhml xzkakxrmxd jniykfrsnq qubbfglzwo zrwnpwnrwc htasnkcnre bckblrvlce lnwzcymuqg plapjlecqh oeylkbxcug fgxuintdlx cmorlmifxk pgifcywtoq paloyjtnec mmyfwbqaci zpclsigtdu oaqhimfrvd oxmfoycksd lleprxynwa qgksygnvid mazryvveow ggqqmzsrqe vvusxzcaen wlnkvfspob nrkxfdoxim mxpscofsmk nzpnlveniv mpsgkmexsq vrvnuzmnxu iaykvnjycd stoljsekio dptzzlmato kqxkjqtdbm meordopeaa jxdccaxhdd abbniuntrd qqktxzuvha tdhmggzxyz qgyogkhwnt yedlwbnqpk aptqmlppre vwolqmbohd riejqgmriv llqdoydiqu zevnjawyqy htmuooywjk rqdvcmgiyz gzpnmarsbq itjbxrlfyk tqashvndeb vvjigorhih ghpsixfdhh irfpzhwpha iizimvzyuq mewofkodcw bzzpofrgup ",
            "7a0d912ff47801a53ca85d665bbc26ea")]
        public void HashTest(string input, string expectedHash)
        {
            BitArray hashArray = _simHash.ComputeHash(input);
            string hash = HashUtils.HashToString(hashArray);
            Assert.Equal(expectedHash, hash);
        }

        [Theory]
        [InlineData(0x00, 0x00, 0)]
        [InlineData(0x00, 0x01, 1)]
        [InlineData(0x01, 0x01, 0)]
        [InlineData(0xFF, 0xFF, 0)]
        [InlineData(0xFF, 0x0F, 4)]
        [InlineData(0x80, 0x01, 2)]
        [InlineData(0xFF, 0x00, 8)]
        public void DistanceTest(byte a, byte b, int exprectedResult)
        {
            var arrA = new BitArray(new [] { a });
            var arrB = new BitArray(new [] { b });

            var distance = HashUtils.GetDistance(arrA, arrB);

            Assert.Equal(exprectedResult, distance);
        }

        [Theory]
        [InlineData("../../../../test/lab1a_primjer/test2/R.in", "../../../../test/lab1a_primjer/test2/R.OUT")]
        public void AnalyzerTest(string inputFile, string resultsFile)
        {
            var results = File
                .ReadAllLines(resultsFile)
                .Where(x => x is { Length: > 0 })
                .Select(int.Parse)
                .ToArray();

            Analyzer analyzer;
            using var fs = File.OpenRead(inputFile);
            using var sr = new StreamReader(fs);
            var line = sr.ReadLine()!;
            var count = int.Parse(line);
            var lines = ReadLines(sr, count);

            analyzer = new Analyzer(lines, count);

            line = sr.ReadLine()!;
            var queryCount = int.Parse(line);

            Assert.Equal(results.Length, queryCount);

            for (int i = 0; i < queryCount; i++)
            {
                line = sr.ReadLine()!;
                var parts = line.Split(' ');
                Assert.NotNull(parts);
                Assert.Equal(2, parts!.Length);

                int targetLine = int.Parse(parts[0]);
                int maxDistance = int.Parse(parts[1]);

                var similarCount = analyzer.GetSimilarCount(targetLine, maxDistance);

                Assert.Equal(results[i], similarCount);
            }

            static IEnumerable<string> ReadLines(StreamReader sr, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    yield return sr.ReadLine()!;
                }
            }
        }
    }
}
