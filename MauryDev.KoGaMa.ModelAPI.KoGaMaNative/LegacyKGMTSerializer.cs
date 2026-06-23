using MauryDev.KoGaMa.ModelAPI.Interfaces;
using MauryDev.KoGaMa.ModelAPI.Model;
using System;
using System.IO;

namespace MauryDev.KoGaMa.ModelAPI.KoGaMaTools
{

    public class LegacyKGMTSerializer : ISerializer
    {
        private const string Signature = "KTMODEL";
        public const float DefaultScale = 1.0f;

        public ModelInfo Deserialize(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            
            var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen: true);

            string header = reader.ReadString();
            if (header != Signature)
            {
                throw new InvalidDataException($"Invalid model data: Expected {Signature}, found {header}");
            }

            float scale = reader.ReadSingle();

            var modelInfo = new ModelInfo();

            try
            {
                while (stream.Position < stream.Length)
                {
                    short x = reader.ReadInt16();
                    short y = reader.ReadInt16();
                    short z = reader.ReadInt16();

                    byte[] faceMaterials = reader.ReadBytes(Cube.MaterialsLen);
                    byte[] byteCorners = reader.ReadBytes(Cube.CornersLen);

                    modelInfo.AddCube(new Cube
                    {
                        Position = new IntVector(x, y, z),
                        Materials = faceMaterials,
                        Corners = byteCorners
                    });
                }
            }
            catch (EndOfStreamException) { }
            reader.Dispose();
            return modelInfo;
        }

        public void Serialize(Stream stream, ModelInfo model)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);

            writer.Write(Signature);
            writer.Write(DefaultScale);

            foreach (var cube in model.Cubes)
            {
                // Posição
                writer.Write(cube.Position.X);
                writer.Write(cube.Position.Y);
                writer.Write(cube.Position.Z);

                // Materiais (Garantindo que sempre escreva exatamente 6 bytes)
                writer.Write(PrepareArray(cube.Materials, Cube.MaterialsLen));

                // Cantos (Garantindo que sempre escreva exatamente 8 bytes)
                writer.Write(PrepareArray(cube.Corners, Cube.CornersLen));
            }

            writer.Flush();

            writer.Dispose();
        }

        #region Helper Methods

       
        private static byte[] PrepareArray(byte[] data, int expectedLength)
        {
            if (data == null) return new byte[expectedLength];
            if (data.Length == expectedLength) return data;

            byte[] resized = new byte[expectedLength];
            Array.Copy(data, resized, Math.Min(data.Length, expectedLength));
            return resized;
        }

        public ModelInfo LoadFromFile(string path)
        {
            var stream = File.OpenRead(path);
            var ret = Deserialize(stream);
            stream.Dispose();
            return ret;
        }

        public void SaveToFile(string path, ModelInfo model)
        {
            var stream = File.Create(path);
            Serialize(stream, model);
            stream.Dispose();
        }

        #endregion
    }
}
