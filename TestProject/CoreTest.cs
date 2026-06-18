using MauryDev.KoGaMa.ModelAPI.Utils;
using NUnit.Framework;
using System;
using System.Numerics;

namespace TestProject
{
    [TestFixture]
    public class PositionConverterTests
    {
        [Test]
        public void Test_RoundTrip_AllPossibleValues()
        {

            for (byte i = 0; i < 125; i++)
            {
                Vector3 vector = PositionConverter.GetVectorFromByte(i);
                byte resultByte = PositionConverter.GetByteFromVector(vector);

                Assert.That(resultByte, Is.EqualTo(i), $"Falha no round-trip para o índice {i}");
            }
        }

        [TestCase(0, -0.5f, -0.5f, -0.5f)]
        [TestCase(124, 0.5f, 0.5f, 0.5f)]
        [TestCase(62, 0f, 0f, 0f)]
        [TestCase(24, -0.5f, 0.5f, 0.5f)]
        public void Test_SpecificValues(byte expectedByte, float x, float y, float z)
        {
            Vector3 expectedVector = new Vector3(x, y, z);

            Vector3 actualVector = PositionConverter.GetVectorFromByte(expectedByte);
            Assert.That(actualVector, Is.EqualTo(expectedVector));

            byte actualByte = PositionConverter.GetByteFromVector(expectedVector);
            Assert.That(actualByte, Is.EqualTo(expectedByte));
        }

        [Test]
        public void Test_InvalidIndex_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                PositionConverter.GetVectorFromByte(125)
            );
        }

        [Test]
        public void Test_InvalidVector_ShouldThrowArgumentException()
        {
            Vector3 invalidVector = new Vector3(1.0f, 0f, 0f);

            var ex = Assert.Throws<ArgumentException>(() =>
                PositionConverter.GetByteFromVector(invalidVector)
            );

            Assert.That(ex.Message, Does.Contain("inválida"));
        }

        [Test]
        public void TestAllPositions()
        {
            int i = 0;
            foreach (var b in bytePositionLookUpTable)
            {
                var idx = PositionConverter.GetByteFromVector(b);
                Is.Equals(idx, i++);
            }

        }

        [Test]
        public void TestAllIndex()
        {
            foreach (var item in positionByteLookUpTable)
            {
                var pos = PositionConverter.GetVectorFromByte(item.Value);
                Is.Equals(item.Key, pos);
            }

        }

        private static Vector3[] bytePositionLookUpTable = new Vector3[]
            {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.25f),
            new Vector3(-0.5f, -0.5f, 0f),
            new Vector3(-0.5f, -0.5f, 0.25f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.25f, -0.5f),
            new Vector3(-0.5f, -0.25f, -0.25f),
            new Vector3(-0.5f, -0.25f, 0f),
            new Vector3(-0.5f, -0.25f, 0.25f),
            new Vector3(-0.5f, -0.25f, 0.5f),
            new Vector3(-0.5f, 0f, -0.5f),
            new Vector3(-0.5f, 0f, -0.25f),
            new Vector3(-0.5f, 0f, 0f),
            new Vector3(-0.5f, 0f, 0.25f),
            new Vector3(-0.5f, 0f, 0.5f),
            new Vector3(-0.5f, 0.25f, -0.5f),
            new Vector3(-0.5f, 0.25f, -0.25f),
            new Vector3(-0.5f, 0.25f, 0f),
            new Vector3(-0.5f, 0.25f, 0.25f),
            new Vector3(-0.5f, 0.25f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.25f),
            new Vector3(-0.5f, 0.5f, 0f),
            new Vector3(-0.5f, 0.5f, 0.25f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.25f, -0.5f, -0.5f),
            new Vector3(-0.25f, -0.5f, -0.25f),
            new Vector3(-0.25f, -0.5f, 0f),
            new Vector3(-0.25f, -0.5f, 0.25f),
            new Vector3(-0.25f, -0.5f, 0.5f),
            new Vector3(-0.25f, -0.25f, -0.5f),
            new Vector3(-0.25f, -0.25f, -0.25f),
            new Vector3(-0.25f, -0.25f, 0f),
            new Vector3(-0.25f, -0.25f, 0.25f),
            new Vector3(-0.25f, -0.25f, 0.5f),
            new Vector3(-0.25f, 0f, -0.5f),
            new Vector3(-0.25f, 0f, -0.25f),
            new Vector3(-0.25f, 0f, 0f),
            new Vector3(-0.25f, 0f, 0.25f),
            new Vector3(-0.25f, 0f, 0.5f),
            new Vector3(-0.25f, 0.25f, -0.5f),
            new Vector3(-0.25f, 0.25f, -0.25f),
            new Vector3(-0.25f, 0.25f, 0f),
            new Vector3(-0.25f, 0.25f, 0.25f),
            new Vector3(-0.25f, 0.25f, 0.5f),
            new Vector3(-0.25f, 0.5f, -0.5f),
            new Vector3(-0.25f, 0.5f, -0.25f),
            new Vector3(-0.25f, 0.5f, 0f),
            new Vector3(-0.25f, 0.5f, 0.25f),
            new Vector3(-0.25f, 0.5f, 0.5f),
            new Vector3(0f, -0.5f, -0.5f),
            new Vector3(0f, -0.5f, -0.25f),
            new Vector3(0f, -0.5f, 0f),
            new Vector3(0f, -0.5f, 0.25f),
            new Vector3(0f, -0.5f, 0.5f),
            new Vector3(0f, -0.25f, -0.5f),
            new Vector3(0f, -0.25f, -0.25f),
            new Vector3(0f, -0.25f, 0f),
            new Vector3(0f, -0.25f, 0.25f),
            new Vector3(0f, -0.25f, 0.5f),
            new Vector3(0f, 0f, -0.5f),
            new Vector3(0f, 0f, -0.25f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 0.25f),
            new Vector3(0f, 0f, 0.5f),
            new Vector3(0f, 0.25f, -0.5f),
            new Vector3(0f, 0.25f, -0.25f),
            new Vector3(0f, 0.25f, 0f),
            new Vector3(0f, 0.25f, 0.25f),
            new Vector3(0f, 0.25f, 0.5f),
            new Vector3(0f, 0.5f, -0.5f),
            new Vector3(0f, 0.5f, -0.25f),
            new Vector3(0f, 0.5f, 0f),
            new Vector3(0f, 0.5f, 0.25f),
            new Vector3(0f, 0.5f, 0.5f),
            new Vector3(0.25f, -0.5f, -0.5f),
            new Vector3(0.25f, -0.5f, -0.25f),
            new Vector3(0.25f, -0.5f, 0f),
            new Vector3(0.25f, -0.5f, 0.25f),
            new Vector3(0.25f, -0.5f, 0.5f),
            new Vector3(0.25f, -0.25f, -0.5f),
            new Vector3(0.25f, -0.25f, -0.25f),
            new Vector3(0.25f, -0.25f, 0f),
            new Vector3(0.25f, -0.25f, 0.25f),
            new Vector3(0.25f, -0.25f, 0.5f),
            new Vector3(0.25f, 0f, -0.5f),
            new Vector3(0.25f, 0f, -0.25f),
            new Vector3(0.25f, 0f, 0f),
            new Vector3(0.25f, 0f, 0.25f),
            new Vector3(0.25f, 0f, 0.5f),
            new Vector3(0.25f, 0.25f, -0.5f),
            new Vector3(0.25f, 0.25f, -0.25f),
            new Vector3(0.25f, 0.25f, 0f),
            new Vector3(0.25f, 0.25f, 0.25f),
            new Vector3(0.25f, 0.25f, 0.5f),
            new Vector3(0.25f, 0.5f, -0.5f),
            new Vector3(0.25f, 0.5f, -0.25f),
            new Vector3(0.25f, 0.5f, 0f),
            new Vector3(0.25f, 0.5f, 0.25f),
            new Vector3(0.25f, 0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.25f),
            new Vector3(0.5f, -0.5f, 0f),
            new Vector3(0.5f, -0.5f, 0.25f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.25f, -0.5f),
            new Vector3(0.5f, -0.25f, -0.25f),
            new Vector3(0.5f, -0.25f, 0f),
            new Vector3(0.5f, -0.25f, 0.25f),
            new Vector3(0.5f, -0.25f, 0.5f),
            new Vector3(0.5f, 0f, -0.5f),
            new Vector3(0.5f, 0f, -0.25f),
            new Vector3(0.5f, 0f, 0f),
            new Vector3(0.5f, 0f, 0.25f),
            new Vector3(0.5f, 0f, 0.5f),
            new Vector3(0.5f, 0.25f, -0.5f),
            new Vector3(0.5f, 0.25f, -0.25f),
            new Vector3(0.5f, 0.25f, 0f),
            new Vector3(0.5f, 0.25f, 0.25f),
            new Vector3(0.5f, 0.25f, 0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.25f),
            new Vector3(0.5f, 0.5f, 0f),
            new Vector3(0.5f, 0.5f, 0.25f),
            new Vector3(0.5f, 0.5f, 0.5f)
            };

        private static Dictionary<Vector3, byte> positionByteLookUpTable = new Dictionary<Vector3, byte>
        {
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                0
            },
            {
                new Vector3(-0.5f, -0.5f, -0.25f),
                1
            },
            {
                new Vector3(-0.5f, -0.5f, 0f),
                2
            },
            {
                new Vector3(-0.5f, -0.5f, 0.25f),
                3
            },
            {
                new Vector3(-0.5f, -0.5f, 0.5f),
                4
            },
            {
                new Vector3(-0.5f, -0.25f, -0.5f),
                5
            },
            {
                new Vector3(-0.5f, -0.25f, -0.25f),
                6
            },
            {
                new Vector3(-0.5f, -0.25f, 0f),
                7
            },
            {
                new Vector3(-0.5f, -0.25f, 0.25f),
                8
            },
            {
                new Vector3(-0.5f, -0.25f, 0.5f),
                9
            },
            {
                new Vector3(-0.5f, 0f, -0.5f),
                10
            },
            {
                new Vector3(-0.5f, 0f, -0.25f),
                11
            },
            {
                new Vector3(-0.5f, 0f, 0f),
                12
            },
            {
                new Vector3(-0.5f, 0f, 0.25f),
                13
            },
            {
                new Vector3(-0.5f, 0f, 0.5f),
                14
            },
            {
                new Vector3(-0.5f, 0.25f, -0.5f),
                15
            },
            {
                new Vector3(-0.5f, 0.25f, -0.25f),
                16
            },
            {
                new Vector3(-0.5f, 0.25f, 0f),
                17
            },
            {
                new Vector3(-0.5f, 0.25f, 0.25f),
                18
            },
            {
                new Vector3(-0.5f, 0.25f, 0.5f),
                19
            },
            {
                new Vector3(-0.5f, 0.5f, -0.5f),
                20
            },
            {
                new Vector3(-0.5f, 0.5f, -0.25f),
                21
            },
            {
                new Vector3(-0.5f, 0.5f, 0f),
                22
            },
            {
                new Vector3(-0.5f, 0.5f, 0.25f),
                23
            },
            {
                new Vector3(-0.5f, 0.5f, 0.5f),
                24
            },
            {
                new Vector3(-0.25f, -0.5f, -0.5f),
                25
            },
            {
                new Vector3(-0.25f, -0.5f, -0.25f),
                26
            },
            {
                new Vector3(-0.25f, -0.5f, 0f),
                27
            },
            {
                new Vector3(-0.25f, -0.5f, 0.25f),
                28
            },
            {
                new Vector3(-0.25f, -0.5f, 0.5f),
                29
            },
            {
                new Vector3(-0.25f, -0.25f, -0.5f),
                30
            },
            {
                new Vector3(-0.25f, -0.25f, -0.25f),
                31
            },
            {
                new Vector3(-0.25f, -0.25f, 0f),
                32
            },
            {
                new Vector3(-0.25f, -0.25f, 0.25f),
                33
            },
            {
                new Vector3(-0.25f, -0.25f, 0.5f),
                34
            },
            {
                new Vector3(-0.25f, 0f, -0.5f),
                35
            },
            {
                new Vector3(-0.25f, 0f, -0.25f),
                36
            },
            {
                new Vector3(-0.25f, 0f, 0f),
                37
            },
            {
                new Vector3(-0.25f, 0f, 0.25f),
                38
            },
            {
                new Vector3(-0.25f, 0f, 0.5f),
                39
            },
            {
                new Vector3(-0.25f, 0.25f, -0.5f),
                40
            },
            {
                new Vector3(-0.25f, 0.25f, -0.25f),
                41
            },
            {
                new Vector3(-0.25f, 0.25f, 0f),
                42
            },
            {
                new Vector3(-0.25f, 0.25f, 0.25f),
                43
            },
            {
                new Vector3(-0.25f, 0.25f, 0.5f),
                44
            },
            {
                new Vector3(-0.25f, 0.5f, -0.5f),
                45
            },
            {
                new Vector3(-0.25f, 0.5f, -0.25f),
                46
            },
            {
                new Vector3(-0.25f, 0.5f, 0f),
                47
            },
            {
                new Vector3(-0.25f, 0.5f, 0.25f),
                48
            },
            {
                new Vector3(-0.25f, 0.5f, 0.5f),
                49
            },
            {
                new Vector3(0f, -0.5f, -0.5f),
                50
            },
            {
                new Vector3(0f, -0.5f, -0.25f),
                51
            },
            {
                new Vector3(0f, -0.5f, 0f),
                52
            },
            {
                new Vector3(0f, -0.5f, 0.25f),
                53
            },
            {
                new Vector3(0f, -0.5f, 0.5f),
                54
            },
            {
                new Vector3(0f, -0.25f, -0.5f),
                55
            },
            {
                new Vector3(0f, -0.25f, -0.25f),
                56
            },
            {
                new Vector3(0f, -0.25f, 0f),
                57
            },
            {
                new Vector3(0f, -0.25f, 0.25f),
                58
            },
            {
                new Vector3(0f, -0.25f, 0.5f),
                59
            },
            {
                new Vector3(0f, 0f, -0.5f),
                60
            },
            {
                new Vector3(0f, 0f, -0.25f),
                61
            },
            {
                new Vector3(0f, 0f, 0f),
                62
            },
            {
                new Vector3(0f, 0f, 0.25f),
                63
            },
            {
                new Vector3(0f, 0f, 0.5f),
                64
            },
            {
                new Vector3(0f, 0.25f, -0.5f),
                65
            },
            {
                new Vector3(0f, 0.25f, -0.25f),
                66
            },
            {
                new Vector3(0f, 0.25f, 0f),
                67
            },
            {
                new Vector3(0f, 0.25f, 0.25f),
                68
            },
            {
                new Vector3(0f, 0.25f, 0.5f),
                69
            },
            {
                new Vector3(0f, 0.5f, -0.5f),
                70
            },
            {
                new Vector3(0f, 0.5f, -0.25f),
                71
            },
            {
                new Vector3(0f, 0.5f, 0f),
                72
            },
            {
                new Vector3(0f, 0.5f, 0.25f),
                73
            },
            {
                new Vector3(0f, 0.5f, 0.5f),
                74
            },
            {
                new Vector3(0.25f, -0.5f, -0.5f),
                75
            },
            {
                new Vector3(0.25f, -0.5f, -0.25f),
                76
            },
            {
                new Vector3(0.25f, -0.5f, 0f),
                77
            },
            {
                new Vector3(0.25f, -0.5f, 0.25f),
                78
            },
            {
                new Vector3(0.25f, -0.5f, 0.5f),
                79
            },
            {
                new Vector3(0.25f, -0.25f, -0.5f),
                80
            },
            {
                new Vector3(0.25f, -0.25f, -0.25f),
                81
            },
            {
                new Vector3(0.25f, -0.25f, 0f),
                82
            },
            {
                new Vector3(0.25f, -0.25f, 0.25f),
                83
            },
            {
                new Vector3(0.25f, -0.25f, 0.5f),
                84
            },
            {
                new Vector3(0.25f, 0f, -0.5f),
                85
            },
            {
                new Vector3(0.25f, 0f, -0.25f),
                86
            },
            {
                new Vector3(0.25f, 0f, 0f),
                87
            },
            {
                new Vector3(0.25f, 0f, 0.25f),
                88
            },
            {
                new Vector3(0.25f, 0f, 0.5f),
                89
            },
            {
                new Vector3(0.25f, 0.25f, -0.5f),
                90
            },
            {
                new Vector3(0.25f, 0.25f, -0.25f),
                91
            },
            {
                new Vector3(0.25f, 0.25f, 0f),
                92
            },
            {
                new Vector3(0.25f, 0.25f, 0.25f),
                93
            },
            {
                new Vector3(0.25f, 0.25f, 0.5f),
                94
            },
            {
                new Vector3(0.25f, 0.5f, -0.5f),
                95
            },
            {
                new Vector3(0.25f, 0.5f, -0.25f),
                96
            },
            {
                new Vector3(0.25f, 0.5f, 0f),
                97
            },
            {
                new Vector3(0.25f, 0.5f, 0.25f),
                98
            },
            {
                new Vector3(0.25f, 0.5f, 0.5f),
                99
            },
            {
                new Vector3(0.5f, -0.5f, -0.5f),
                100
            },
            {
                new Vector3(0.5f, -0.5f, -0.25f),
                101
            },
            {
                new Vector3(0.5f, -0.5f, 0f),
                102
            },
            {
                new Vector3(0.5f, -0.5f, 0.25f),
                103
            },
            {
                new Vector3(0.5f, -0.5f, 0.5f),
                104
            },
            {
                new Vector3(0.5f, -0.25f, -0.5f),
                105
            },
            {
                new Vector3(0.5f, -0.25f, -0.25f),
                106
            },
            {
                new Vector3(0.5f, -0.25f, 0f),
                107
            },
            {
                new Vector3(0.5f, -0.25f, 0.25f),
                108
            },
            {
                new Vector3(0.5f, -0.25f, 0.5f),
                109
            },
            {
                new Vector3(0.5f, 0f, -0.5f),
                110
            },
            {
                new Vector3(0.5f, 0f, -0.25f),
                111
            },
            {
                new Vector3(0.5f, 0f, 0f),
                112
            },
            {
                new Vector3(0.5f, 0f, 0.25f),
                113
            },
            {
                new Vector3(0.5f, 0f, 0.5f),
                114
            },
            {
                new Vector3(0.5f, 0.25f, -0.5f),
                115
            },
            {
                new Vector3(0.5f, 0.25f, -0.25f),
                116
            },
            {
                new Vector3(0.5f, 0.25f, 0f),
                117
            },
            {
                new Vector3(0.5f, 0.25f, 0.25f),
                118
            },
            {
                new Vector3(0.5f, 0.25f, 0.5f),
                119
            },
            {
                new Vector3(0.5f, 0.5f, -0.5f),
                120
            },
            {
                new Vector3(0.5f, 0.5f, -0.25f),
                121
            },
            {
                new Vector3(0.5f, 0.5f, 0f),
                122
            },
            {
                new Vector3(0.5f, 0.5f, 0.25f),
                123
            },
            {
                new Vector3(0.5f, 0.5f, 0.5f),
                124
            }
        };
    }

}