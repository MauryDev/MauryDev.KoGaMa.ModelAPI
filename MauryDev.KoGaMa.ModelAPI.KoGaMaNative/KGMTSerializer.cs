using MauryDev.KoGaMa.ModelAPI.Model;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static MauryDev.KoGaMa.ModelAPI.KoGaMaTools.CubeInfoDTO;

namespace MauryDev.KoGaMa.ModelAPI.KoGaMaTools
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class CubeInfoDTO
    {
        public int[] FaceMaterials { get; set; }

        public int[] Corners { get; set; }

        public PositionDTO Position { get; set; }
        [MessagePackObject(keyAsPropertyName: true)]
        public class PositionDTO
        {
            public short X { get; set; }
            public short Y { get; set; }
            public short Z { get; set; }
        }
    }

    public static class ModelMapper
    {
        public static Cube ToDomain(CubeInfoDTO dto)
        {
            if (dto == null) return null;

            byte[] materials = new byte[dto.FaceMaterials.Length];
            for (int i = 0; i < dto.FaceMaterials.Length; i++)
            {
                materials[i] = Convert.ToByte(dto.FaceMaterials[i]);
            }

            byte[] corners = new byte[dto.Corners.Length];
            for (int i = 0; i < dto.Corners.Length; i++)
            {
                corners[i] = Convert.ToByte(dto.Corners[i]);
            }

            return new Cube
            {
                Materials = materials,
                Corners = corners,
                Position = new IntVector(dto.Position.X, dto.Position.Y, dto.Position.Z)
            };
        }

        public static CubeInfoDTO ToDto(Cube cube)
        {
            if (cube == null) return null;

            int[] materials = new int[cube.Materials.Length];
            for (int i = 0; i < cube.Materials.Length; i++)
            {
                materials[i] = Convert.ToInt32(cube.Materials[i]);
            }

            int[] corners = new int[cube.Corners.Length];
            for (int i = 0; i < cube.Corners.Length; i++)
            {
                corners[i] = Convert.ToInt32(cube.Corners[i]);
            }

            return new CubeInfoDTO
            {
                FaceMaterials = materials,
                Corners = corners,
                Position = new PositionDTO
                {
                    X = cube.Position.X,
                    Y = cube.Position.Y,
                    Z = cube.Position.Z
                }
            };
        }

        public static ModelInfo MapToModel(CubeInfoDTO[] dtos)
        {
            var model = new ModelInfo();
            if (dtos == null) return model;

            foreach (var dto in dtos)
            {
                model.AddCube(ToDomain(dto));
            }
            return model;
        }

        public static CubeInfoDTO[] MapToDtoArray(ModelInfo model)
        {
            return model.Cubes.Select(ToDto).ToArray();
        }
    }


    public class KGMTSerializer : Interfaces.ISerializer
    {
        public ModelInfo Deserialize(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var dtos = MessagePackSerializer.Deserialize<CubeInfoDTO[]>(stream);
            return ModelMapper.MapToModel(dtos);
        }

       
        public void Serialize(Stream stream, ModelInfo model)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var dtos = ModelMapper.MapToDtoArray(model);
            MessagePackSerializer.Serialize(stream, dtos);
        }


        
        public ModelInfo LoadFromFile(string filePath)
        {
            var stream = File.OpenRead(filePath);
            var ret = Deserialize(stream);
            stream.Dispose();
            return ret;
        }

        
        public void SaveToFile(string filePath, ModelInfo model)
        {
            var stream = File.Create(filePath);
            Serialize(stream, model);
            stream.Dispose();

        }

        public byte[] SerializeToBytes(ModelInfo model)
        {
            var dtos = ModelMapper.MapToDtoArray(model);
            return MessagePackSerializer.Serialize(dtos);
        }

    }
}
