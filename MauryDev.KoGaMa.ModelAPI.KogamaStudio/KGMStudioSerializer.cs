using MauryDev.KoGaMa.ModelAPI.Model;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using MauryDev.KoGaMa.ModelAPI.KogamaStudio.Core;
using MauryDev.KoGaMa.ModelAPI.KogamaStudio.Handler;

namespace MauryDev.KoGaMa.ModelAPI.KogamaStudio
{
    public class KGMStudioSerializer : Interfaces.ISerializer
    {
        private static readonly IEnumerable<IModelFormatHandler> _handlers = new List<IModelFormatHandler>
        {
            new BinaryModelHandler(),
            new JsonModelHandler()
        };

        public ModelInfo Deserialize(Stream stream)
        {
            if (stream.CanRead == false || stream.Length == 0) return new ModelInfo();

            int firstByte = stream.ReadByte();
            stream.Seek(0, SeekOrigin.Begin);

            var handler = _handlers.FirstOrDefault(h => h.CanHandle((byte)firstByte));

            return handler == null ? throw new InvalidDataException("Unsupported model format.") : handler.Deserialize(stream);
        }

        public void Serialize(Stream stream, ModelInfo model)
        {
            var handler = _handlers.OfType<BinaryModelHandler>().First();
            handler.Serialize(stream, model);
        }
    }
}
