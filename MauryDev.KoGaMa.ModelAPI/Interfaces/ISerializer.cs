using MauryDev.KoGaMa.ModelAPI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MauryDev.KoGaMa.ModelAPI.Interfaces
{
    public interface ISerializer
    {
        void Serialize(Stream stream, ModelInfo model);
        ModelInfo Deserialize(Stream stream);
    }
}
