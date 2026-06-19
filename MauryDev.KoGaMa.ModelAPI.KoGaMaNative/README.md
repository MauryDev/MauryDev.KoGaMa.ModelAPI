# KoGaMa Model Serializer (KGMT)

A high-performance serialization library for KoGaMa model data. This project provides tools to convert complex 3D model structures (composed of cubes, materials, and coordinates) into compact binary formats for storage or network transmission.

## 🚀 Features

- **Modern Serialization**: Uses [MessagePack](https://msgpack.org/) for highly efficient, compact binary serialization.
- **Legacy Support**: Includes a `LegacyKGMTSerializer` to maintain compatibility with older `.ktmodel` binary formats.
- **DTO Mapping**: Implements a clean separation between Domain Models (`ModelInfo`, `Cube`) and Data Transfer Objects (`CubeInfoDTO`) to ensure data integrity and flexibility.
- **Flexible I/O**: Support for `Stream` based operations, direct file system saving/loading, and byte array conversion.

## 🛠 Technical Architecture

### 1. Modern Serializer (`KGMTSerializer`)
The modern implementation leverages **MessagePack**. It transforms the domain model into a flattened DTO structure before serialization. This approach reduces the payload size and increases deserialization speed.

**Data Flow:** 
`ModelInfo` -> `ModelMapper` -> `CubeInfoDTO[]` -> `MessagePack Binary`

### 2. Legacy Serializer (`LegacyKGMTSerializer`)
The legacy implementation uses a custom binary format identified by the `KTMODEL` signature. It reads and writes raw bytes and shorts, making it ideal for reading older version files.

---

## 📦 Installation

This project depends on the `MessagePack` NuGet package.

```bash
dotnet add package MessagePack
```

## 💻 Usage

### Modern Serialization (Recommended)
Use the `KGMTSerializer` for new projects and optimal performance.

```csharp
using MauryDev.KoGaMa.ModelAPI.KoGaMaTools;
using MauryDev.KoGaMa.ModelAPI.Model;

// 1. Create your model
ModelInfo myModel = new ModelInfo();
myModel.AddCube(new Cube { 
    Position = new IntVector(0, 0, 0), 
    Materials = new byte[6], 
    Corners = new byte[8] 
});

// 2. Initialize serializer
var serializer = new KGMTSerializer();

// 3. Save to file
serializer.SaveToFile("model.kgmt", myModel);

// 4. Load from file
ModelInfo loadedModel = serializer.LoadFromFile("model.kgmt");
```

### Legacy Serialization (Backward Compatibility)
Use the `LegacyKGMTSerializer` if you are dealing with files that start with the `KTMODEL` header.

```csharp
using MauryDev.KoGaMa.ModelAPI.KoGaMaTools;

var legacySerializer = new LegacyKGMTSerializer();

// Load an old .ktmodel file
ModelInfo oldModel = legacySerializer.LoadFromFile("old_model.ktmodel");

// Save in legacy format
legacySerializer.SaveToFile("backup_legacy.ktmodel", oldModel);
```

## 📊 Comparison

| Feature | `KGMTSerializer` | `LegacyKGMTSerializer` |
| :--- | :--- | :--- |
| **Format** | MessagePack (Binary) | Custom Binary (`KTMODEL`) |
| **Speed** | Very High | High |
| **File Size** | Extremely Compact | Compact |
| **Use Case** | New versions / API calls | Old file compatibility |
| **Mapping** | Uses DTOs | Direct Stream read/write |

## 📜 License
MIT License