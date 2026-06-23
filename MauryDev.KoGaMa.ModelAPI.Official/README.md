# KoGaMa Model API - Official Implementation

`MauryDev.KoGaMa.ModelAPI.Official` is a specialized implementation of the KoGaMa model serialization system. It provides the logic required to convert in-memory models into the official binary format used by the game, focusing on high-efficiency data compression and spatial optimization.

This library is built on top of the base `MauryDev.KoGaMa.ModelAPI` and targets **.NET Standard 2.0**, ensuring compatibility across .NET Framework, .NET Core, and .NET 5+.

## 🚀 Core Features

The official implementation focuses on minimizing the binary footprint of models through two main components:

### 🧊 CubeGrouper (Spatial Optimization)

The `CubeGrouper` is the optimization engine that reduces redundancy in geometry. Instead of storing every cube as a separate entity, it identifies and groups adjacent cubes that are identical in properties.

**Logic Flow:**
1. **Spatial Sorting:** Cubes are sorted by Z, then Y, and finally X coordinates.
2. **Linear Scanning:** The algorithm scans for sequences of cubes aligned specifically along the **X-axis**.
3. **Property Matching:** To be grouped into a "row," cubes must:
   - Be in consecutive positions on the X-axis.
   - Share the exact same material array (6 faces).
   - Share the exact same corner configuration.
4. **Limit:** A single group can contain up to **63 cubes**.

This significantly reduces the amount of coordinate data written to the stream.

### 💾 OfficialSerializer (Binary Implementation)

The `OfficialSerializer` implements the `ISerializer` interface. It transforms the grouped data into a compact binary stream using a bit-flag system to omit redundant information.

#### Compression Flags:
The serializer uses a flag byte to notify the reader about the data structure:

- **`FlagIdentityCorners`**: If the group's corners match the standard identity corners, the flag is set and the 8-byte corner data is completely omitted.
- **`FlagUniformMaterials`**: If all 6 faces of the cubes in the group use the same material, the flag is set and only **one byte** is written instead of six.
- **Row Compression**: The number of cubes in a row (calculated by `CubeGrouper`) is shifted into the flag byte (`rowCount << 2`), allowing the deserializer to expand the row back into individual cubes.

#### Low-Level I/O:
To handle the specific binary requirements of the official format, the library uses `ReverseReader` and `ReverseWriter`, which manage custom byte-ordering and cached reads.

---

## 🛠️ Usage Example

```csharp
using MauryDev.KoGaMa.ModelAPI.Model;
using MauryDev.KoGaMa.ModelAPI.Official;
using System.IO;

// 1. Setup your model (from the base ModelAPI library)
ModelInfo myModel = new ModelInfo();
myModel.AddCube(new Cube(new IntVector(0, 0, 0), materials, corners));

// 2. Use the Official Serializer
var serializer = new OfficialSerializer();

// 3. Serialize to the official binary format
using (var stream = File.OpenWrite("model.bin"))
{
    serializer.Serialize(stream, myModel);
}

// 4. Deserialize back into a ModelInfo object
using (var stream = File.OpenRead("model.bin"))
{
    ModelInfo loadedModel = serializer.Deserialize(stream);
}
```

## 📊 Technical Specifications

| Specification | Detail |
| :--- | :--- |
| **Target Framework** | `.NET Standard 2.0` |
| **Max Row Length** | 63 cubes |
| **Material Compression** | 6 bytes -> 1 byte (if uniform) |
| **Corner Compression** | 8 bytes -> 0 bytes (if identity) |
| **Sorting Order** | Z -> Y -> X |
| **Dependencies** | `MauryDev.KoGaMa.ModelAPI` |

## 📦 Project Architecture

- **`MauryDev.KoGaMa.ModelAPI`**: (Dependency) Core interfaces, `ModelInfo`, and `Cube` definitions.
- **`MauryDev.KoGaMa.ModelAPI.Official`**: 
    - `CubeGrouper`: Spatial logic for row optimization.
    - `OfficialSerializer`: Binary logic and flag management.
    - `Official.IO`: Low-level `ReverseReader` and `ReverseWriter`.