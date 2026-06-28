using NUnit.Framework;
using MauryDev.KoGaMa.ModelAPI.Model;
using MauryDev.KoGaMa.ModelAPI.Official;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    [TestFixture]
    public class OfficialSerializerTests
    {
        private OfficialSerializer _serializer;

        [SetUp]
        public void SetUp()
        {
            _serializer = new OfficialSerializer();
        }

        [Test]
        public void SerializeAndDeserialize_BasicModel_ReturnsEquivalentModel()
        {
            // Arrange
            var model = new ModelInfo();
            var pos = new IntVector(0, 0, 0);
            var materials = new byte[] { 1, 1, 1, 1, 1, 1 };
            var corners = (byte[])Cube.IdentityByteCorners.Clone();

            model.AddCube(new Cube(pos, materials, corners));
            model.AddCube(new Cube(new IntVector(1, 0, 0), materials, corners));

            // Act
            byte[] data;
            using (var ms = new MemoryStream())
            {
                _serializer.Serialize(ms, model);
                data = ms.ToArray();
            }

            ModelInfo deserializedModel;
            using (var ms = new MemoryStream(data))
            {
                deserializedModel = _serializer.Deserialize(ms);
            }

            // Assert
            Assert.That(deserializedModel.Cubes.Count, Is.EqualTo(model.Cubes.Count));

            var sortedOriginal = model.Cubes
                 .OrderBy(c => c.Position.X)
                 .ThenBy(c => c.Position.Y)
                 .ThenBy(c => c.Position.Z)
                 .ToList();

            var sortedDeserialized = deserializedModel.Cubes
                .OrderBy(c => c.Position.X)
                .ThenBy(c => c.Position.Y)
                .ThenBy(c => c.Position.Z)
                .ToList();
            for (int i = 0; i < sortedOriginal.Count; i++)
            {
                Assert.That(sortedDeserialized[i].Position, Is.EqualTo(sortedOriginal[i].Position));
                Assert.That(sortedDeserialized[i].Materials, Is.EqualTo(sortedOriginal[i].Materials));
                Assert.That(sortedDeserialized[i].Corners, Is.EqualTo(sortedOriginal[i].Corners));
            }
        }

        [Test]
        public void SerializeAndDeserialize_UniformMaterialsAndIdentityCorners_WorksCorrectly()
        {
            // Arrange
            var model = new ModelInfo();
            var materials = new byte[] { 2, 2, 2, 2, 2, 2 }; // Uniform
            var corners = (byte[])Cube.IdentityByteCorners.Clone(); // Identity

            model.AddCube(new Cube(new IntVector(0, 0, 0), materials, corners));
            model.AddCube(new Cube(new IntVector(1, 0, 0), materials, corners));

            // Act
            byte[] data;
            using (var ms = new MemoryStream())
            {
                _serializer.Serialize(ms, model);
                data = ms.ToArray();
            }

            ModelInfo deserializedModel;
            using (var ms = new MemoryStream(data))
            {
                deserializedModel = _serializer.Deserialize(ms);
            }

            // Assert
            Assert.That(deserializedModel.Cubes.Count, Is.EqualTo(2));
            Assert.That(deserializedModel.Cubes[0].Materials, Is.EqualTo(materials));
            Assert.That(deserializedModel.Cubes[0].Corners, Is.EqualTo(corners));
        }

        [Test]
        public void SerializeAndDeserialize_NonUniformMaterialsAndCustomCorners_WorksCorrectly()
        {
            // Arrange
            var model = new ModelInfo();
            var materials = new byte[] { 1, 2, 3, 4, 5, 6 }; // Non-uniform
            var corners = new byte[8];
            for (int i = 0; i < 8; i++) corners[i] = (byte)(i + 1); // Custom

            model.AddCube(new Cube(new IntVector(0, 0, 0), materials, corners));

            // Act
            byte[] data;
            using (var ms = new MemoryStream())
            {
                _serializer.Serialize(ms, model);
                data = ms.ToArray();
            }

            ModelInfo deserializedModel;
            using (var ms = new MemoryStream(data))
            {
                deserializedModel = _serializer.Deserialize(ms);
            }

            // Assert
            Assert.That(deserializedModel.Cubes.Count, Is.EqualTo(1));
            Assert.That(deserializedModel.Cubes[0].Materials, Is.EqualTo(materials));
            Assert.That(deserializedModel.Cubes[0].Corners, Is.EqualTo(corners));
        }
    }
}
