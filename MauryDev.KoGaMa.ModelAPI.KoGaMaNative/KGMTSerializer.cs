using MauryDev.KoGaMa.ModelAPI.Model;
using MauryDev.KoGaMa.ModelAPI.Models;
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
        public byte[] FaceMaterials { get; set; }

        public byte[] Corners { get; set; }

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

            return new Cube
            {
                Materials = dto.FaceMaterials,
                Corners = dto.Corners,
                Position = new IntVector(dto.Position.X, dto.Position.Y, dto.Position.Z)
            };
        }

        public static CubeInfoDTO ToDto(Cube cube)
        {
            if (cube == null) return null;

            return new CubeInfoDTO
            {
                FaceMaterials = cube.Materials,
                Corners = cube.Corners,
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
