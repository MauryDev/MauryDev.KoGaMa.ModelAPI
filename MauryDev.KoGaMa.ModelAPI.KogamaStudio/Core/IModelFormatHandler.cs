using MauryDev.KoGaMa.ModelAPI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MauryDev.KoGaMa.ModelAPI.KogamaStudio.Core
{
    public interface IModelFormatHandler
    {
        bool CanHandle(byte firstByte);
        ModelInfo Deserialize(Stream stream);
        void Serialize(Stream stream, ModelInfo model);
    }
}
