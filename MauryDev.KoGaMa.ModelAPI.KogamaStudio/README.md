# KoGaMa ModelAPI Studio

**KoGaMa ModelAPI Studio** is a .NET Standard 2.0 library designed to handle the serialization and deserialization of model data used in KoGaMa Studio. It provides a unified interface to work with different model formats, primarily focusing on Binary and JSON representations of model cubes.

## 🚀 Features

- **Automatic Format Detection**: The library automatically detects whether a stream is in Binary or JSON format by analyzing the first byte.
- **Binary Support**: Full support for reading and writing the `KSCB` binary format.
- **JSON Support**: Ability to deserialize model data from JSON files.
- **Lightweight**: Built on .NET Standard 2.0 for maximum compatibility across different .NET platforms.

## 📦 Installation

This library depends on `MauryDev.KoGaMa.ModelAPI`. Ensure you have that project referenced in your solution.

If you are adding it via project reference:
```xml
<ProjectReference Include="..\MauryDev.KoGaMa.ModelAPI.KogamaStudio\MauryDev.KoGaMa.ModelAPI.KogamaStudio.csproj" />
```

## 🛠 Usage

The primary entry point for the library is the `KGMStudioSerializer` class.

### Deserializing a Model
The serializer will automatically determine if the file is a Binary (`KSCB`) or JSON file.

```csharp
using MauryDev.KoGaMa.ModelAPI.KogamaStudio;
using MauryDev.KoGaMa.ModelAPI.Model;
using System.IO;

var serializer = new KGMStudioSerializer();

using (FileStream fs = File.OpenRead("myModel.kgm"))
{
    ModelInfo model = serializer.Deserialize(fs);
    Console.WriteLine($"Loaded model with {model.CubeCount} cubes.");
}
```

### Serializing a Model
Currently, the library exports models using the **Binary format**.

```csharp
using MauryDev.KoGaMa.ModelAPI.KogamaStudio;
using MauryDev.KoGaMa.ModelAPI.Model;
using System.IO;

var serializer = new KGMStudioSerializer();
ModelInfo myModel = new ModelInfo(); 
// ... add cubes to your model ...

using (FileStream fs = File.Create("exportedModel.kgm"))
{
    serializer.Serialize(fs, myModel);
}
```

## 📋 Supported Formats

| Format | Detection Byte | Read | Write | Notes |
| :--- | :--- | :---: | :---: | :--- |
| **Binary** | `K` (`KSCB`) | ✅ | ✅ | Optimized for size and speed. |
| **JSON** | `{` | ✅ | ❌ | Useful for human-readable editing. |

## 🏗 Architecture

The library uses a provider-based pattern:
- `KGMStudioSerializer`: The main coordinator.
- `IModelFormatHandler`: Interface defining how a specific format is handled.
- `BinaryModelHandler`: Logic for the binary `KSCB` format.
- `JsonModelHandler`: Logic for the JSON format.

## 📄 License

Refer to the `LICENSE.md` file for licensing details.

---
**Author:** MauryDev