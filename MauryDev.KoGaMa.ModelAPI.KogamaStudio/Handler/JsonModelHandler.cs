using MauryDev.KoGaMa.ModelAPI.KogamaStudio.Core;
using MauryDev.KoGaMa.ModelAPI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Nodes;

namespace MauryDev.KoGaMa.ModelAPI.KogamaStudio.Handler
{
    public class JsonModelHandler : IModelFormatHandler
    {
        public bool CanHandle(byte firstByte) => firstByte == '{';

        public ModelInfo Deserialize(Stream stream)
        {
            var model = new ModelInfo();
            using (var reader = new StreamReader(stream))
            {
                var jObj = JsonNode.Parse(reader.ReadToEnd());

                var cubes = jObj["cubes"]?.AsArray();
                if (cubes == null) return model;

                foreach (var cube in cubes)
                {
                    var pos = new IntVector(
                        (short)cube["x"].GetValue<int>(),
                        (short)cube["y"].GetValue<int>(),
                        (short)cube["z"].GetValue<int>()
                    );
                    model.AddCube(new Cube(pos, cube["materials"].GetValue<byte[]>(), cube["corners"].GetValue<byte[]>()));
                }
                return model;
            }

        }

        public void Serialize(Stream stream, ModelInfo model)
            => throw new System.NotImplementedException("JSON serialization not implemented yet.");
    }
}
