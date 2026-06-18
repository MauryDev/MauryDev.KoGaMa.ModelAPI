using MauryDev.KoGaMa.ModelAPI.Model;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;

namespace MauryDev.KoGaMa.ModelAPI.KoGaMaToolsNative
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
    public class KGMNSerializer : Interfaces.ISerializer
    {
        public ModelInfo CreateModelInfo(CubeInfoDTO[] value)
        {
            var ret = new ModelInfo();
            foreach (var item in value)
            {
                var pos = item.Position;
                

                byte[] materials = item.FaceMaterials;

                byte[] corners = item.Corners;


                ret.Cubes.Add(new Cube()
                {
                    Corners = corners,
                    Materials = materials,
                    Position = new Models.IntVector() { X = pos.X, Y = pos.Y, Z = pos.Z }
                });
            }
            return ret;
        }
        public CubeInfoDTO[] ToCubeInfoDTOArray(ModelInfo value)
        {
            var ret = new CubeInfoDTO[value.Cubes.Count];
            int i = 0;
            foreach (var item in value.Cubes)
            {
                var pos = item.Position;


                byte[] materials = item.Materials;

                byte[] corners = item.Corners;

                ret[i++] = new CubeInfoDTO()
                {
                    FaceMaterials = materials,
                    Corners = corners,
                    Position = new CubeInfoDTO.PositionDTO()
                    {
                        X = pos.X,
                        Y = pos.Y,
                        Z = pos.Z
                    }
                };
            }
            return ret;
        }
        public ModelInfo Deserialize(Stream stream)
        {
            var result = MessagePackSerializer.Deserialize<CubeInfoDTO[]>(stream);
            return CreateModelInfo(result);
        }

        public void Serialize(Stream stream, ModelInfo model)
        {
            var rootList = ToCubeInfoDTOArray(model);


            MessagePackSerializer.Serialize(stream, rootList);
        }
    }
}
