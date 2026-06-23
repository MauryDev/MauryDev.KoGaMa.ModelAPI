using MauryDev.KoGaMa.ModelAPI.KogamaStudio.Core;
using MauryDev.KoGaMa.ModelAPI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace MauryDev.KoGaMa.ModelAPI.KogamaStudio.Handler
{
    public class BinaryModelHandler : IModelFormatHandler
    {
        private static readonly byte[] Magic = { (byte)'K', (byte)'S', (byte)'C', (byte)'B' };

        public bool CanHandle(byte firstByte) => firstByte == 'K';

        public ModelInfo Deserialize(Stream stream)
        {
            var model = new ModelInfo();
            using (var br = new BinaryReader(stream, System.Text.Encoding.Default))
            {
                var header = br.ReadBytes(4);
                if (!header.SequenceEqual(Magic)) return model;

                br.ReadByte();
                int count = br.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    var pos = new IntVector((short)br.ReadInt32(), (short)br.ReadInt32(), (short)br.ReadInt32());
                    var materials = br.ReadBytes(6);
                    var corners = br.ReadBytes(8);
                    model.AddCube(new Cube(pos, materials, corners));
                }
                return model;
            }


        }

        public void Serialize(Stream stream, ModelInfo model)
        {
            using (var bw = new BinaryWriter(stream, System.Text.Encoding.Default, leaveOpen: true))
            {
                bw.Write(Magic);
                bw.Write((byte)1);
                bw.Write(model.CubeCount);
                foreach (var c in model.Cubes)
                {
                    bw.Write((int)c.Position.X);
                    bw.Write((int)c.Position.Y);
                    bw.Write((int)c.Position.Z);
                    bw.Write(c.Materials);
                    bw.Write(c.Corners);
                }
            }

        }
    }


}
