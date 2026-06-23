using MauryDev.KoGaMa.ModelAPI.Interfaces;
using MauryDev.KoGaMa.ModelAPI.Model;
using MauryDev.KoGaMa.ModelAPI.Official.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MauryDev.KoGaMa.ModelAPI.Official
{

    public class OfficialSerializer : ISerializer
    {
        private const byte FlagIdentityCorners = 1;
        private const byte FlagUniformMaterials = 2;
        private const int MaxCubesInRow = 63;

        public void Serialize(Stream stream, ModelInfo model)
        {
            var writer = new ReverseWriter(stream);
            var groups = CubeGrouper.GroupIntoRows(model.Cubes);

            writer.Write(groups.Count);

            foreach (var group in groups)
            {
                WriteCubeGroup(writer, group);
            }
        }

        public ModelInfo Deserialize(Stream stream)
        {
            var model = new ModelInfo();
            var reader = new ReverseReader(stream);
            var groupCount = reader.ReadInt32();

            for (int i = 0; i < groupCount; i++)
            {
                var position = new IntVector(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
                var flags = reader.ReadByte();

                var (corners, materials) = ReadCompressedData(flags, reader);

                int cubesInRow = flags >> 2;
                for (int j = 0; j < cubesInRow; j++)
                {
                    var currentPos = new IntVector(
                        (short)(position.X + j),
                        position.Y,
                        position.Z
                    );
                    model.AddCube(new Cube(currentPos, materials, corners));
                }
            }

            return model;
        }

        private void WriteCubeGroup(ReverseWriter writer, List<Cube> group)
        {
            var firstCube = group[0];
            int rowCount = group.Count;
            byte flags = 0;

            if (firstCube.Corners.SequenceEqual(Cube.IdentityByteCorners))
                flags |= FlagIdentityCorners;

            if (AreMaterialsUniform(firstCube.Materials))
                flags |= FlagUniformMaterials;

            flags |= (byte)(rowCount << 2);

            writer.Write(firstCube.Position.X);
            writer.Write(firstCube.Position.Y);
            writer.Write(firstCube.Position.Z);
            writer.Write(flags);

            if ((flags & FlagIdentityCorners) == 0)
                writer.Write(firstCube.Corners);

            if ((flags & FlagUniformMaterials) == 0)
                writer.Write(firstCube.Materials);
            else
                writer.Write(firstCube.Materials[0]);
        }

        private (byte[] corners, byte[] materials) ReadCompressedData(byte flags, ReverseReader reader)
        {
            byte[] corners = (flags & FlagIdentityCorners) != 0
                ? (byte[])Cube.IdentityByteCorners.Clone()
                : reader.ReadBytes(8);

            byte[] materials;
            if ((flags & FlagUniformMaterials) == 0)
            {
                materials = reader.ReadBytes(6);
            }
            else
            {
                byte materialValue = reader.ReadByte();
                materials = Enumerable.Repeat(materialValue, 6).ToArray();
            }

            return (corners, materials);
        }

        private bool AreMaterialsUniform(byte[] materials)
        {
            if (materials == null || materials.Length == 0) return true;
            return materials.All(m => m == materials[0]);
        }
    }

}
