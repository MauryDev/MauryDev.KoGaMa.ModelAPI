using MauryDev.KoGaMa.ModelAPI.KoGaMaTools;
using MauryDev.KoGaMa.ModelAPI.Model;
using MauryDev.KoGaMa.ModelAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject
{
    [TestFixture]
    public class KGMNSerializerTests
    {
        private KGMTSerializer _serializer;

        [SetUp]
        public void Setup()
        {
            _serializer = new KGMTSerializer();
        }

        [Test]
        public void SerializeAndDeserialize_EmptyModel_ShouldReturnEmptyList()
        {
            // Arrange
            var originalModel = new ModelInfo();

            // Act
            var resultModel = RoundTrip(originalModel);

            // Assert
            Assert.That(resultModel.Cubes, Is.Empty);
        }

        [Test]
        public void SerializeAndDeserialize_SingleCube_ShouldPreserveData()
        {
            // Arrange
            var originalModel = new ModelInfo();
            originalModel.AddCube(new Cube
            {
                Position = new IntVector { X = 10, Y = -5, Z = 100 },
                Materials = new byte[] { 1, 2, 3, 4, 5, 6},
                Corners = new byte[] { 8, 9, 10, 11, 12, 13, 14, 15 }
            });

            // Act
            var resultModel = RoundTrip(originalModel);

            // Assert
            Assert.That(resultModel.Cubes.Count, Is.EqualTo(1));
            var cube = resultModel.Cubes[0];

            Assert.Multiple(() =>
            {
                Assert.That(cube.Position.X, Is.EqualTo(10));
                Assert.That(cube.Position.Y, Is.EqualTo(-5));
                Assert.That(cube.Position.Z, Is.EqualTo(100));
                Assert.That(cube.Materials, Is.EqualTo(originalModel.Cubes[0].Materials));
                Assert.That(cube.Corners, Is.EqualTo(originalModel.Cubes[0].Corners));
            });
        }

        [Test]
        public void SerializeAndDeserialize_MultipleCubes_ShouldPreserveOrderAndData()
        {
            // Arrange
            var originalModel = new ModelInfo();
            originalModel.AddCube(new Cube
            {
                Position = new IntVector { X = 1, Y = 1, Z = 1 },
                Materials = new byte[6],
                Corners = new byte[8]
            });
            originalModel.AddCube(new Cube
            {
                Position = new IntVector { X = 2, Y = 2, Z = 2 },
                Materials = new byte[6],
                Corners = new byte[8]
            });

            // Act
            var resultModel = RoundTrip(originalModel);

            // Assert
            Assert.That(resultModel.Cubes.Count, Is.EqualTo(2));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(resultModel.Cubes[0].Position.X, Is.EqualTo(1));
                Assert.That(resultModel.Cubes[1].Position.X, Is.EqualTo(2));
            }
        }

        [Test]
        public void SerializeAndDeserialize_ExtremeValues_ShouldHandleShortLimits()
        {
            // Arrange
            var originalModel = new ModelInfo();
            originalModel.AddCube(new Cube
            {
                Position = new IntVector { X = short.MinValue, Y = short.MaxValue, Z = 0 },
                Materials = new byte[7],
                Corners = new byte[8]
            });

            // Act
            var resultModel = RoundTrip(originalModel);

            // Assert
            var pos = resultModel.Cubes[0].Position;
            Assert.Multiple(() =>
            {
                Assert.That(pos.X, Is.EqualTo(short.MinValue));
                Assert.That(pos.Y, Is.EqualTo(short.MaxValue));
            });
        }

        /// <summary>
        /// Método auxiliar que simula o fluxo: Objeto -> Stream -> Objeto
        /// </summary>
        private ModelInfo RoundTrip(ModelInfo original)
        {
            using var stream = new MemoryStream();
            // 1. Serializa para o stream
            _serializer.Serialize(stream, original);

            // 2. Volta o cursor do stream para o início para que o Deserialize possa ler
            stream.Position = 0;

            // 3. Desserializa do stream
            return _serializer.Deserialize(stream);
        }
    }
}
