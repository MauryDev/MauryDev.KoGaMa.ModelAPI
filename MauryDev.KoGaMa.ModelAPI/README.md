# KoGaMa Model API

A lightweight .NET Standard 2.0 library designed for handling, manipulating, and serializing 3D model data for KoGaMa. This API provides the data structures necessary to represent voxel-like cubes, their materials, and their vertex positions.

## 🚀 Features

- **Cube Representation**: Detailed modeling of cubes including positions, face materials, and corner offsets.
- **Coordinate Mapping**: A specialized `PositionConverter` that maps byte values to `Vector3` coordinates based on a predefined grid.
- **Serialization Interface**: A generic `ISerializer` interface to implement custom binary or JSON saving/loading logic.
- **Lightweight**: Targets `.NET Standard 2.0` for maximum compatibility across different .NET platforms (Core, Framework, Unity).

## 🛠 Technical Overview

### Core Components

- **`ModelInfo`**: The root container for a model, containing a list of all `Cube` objects.
- **`Cube`**: Represents a single block. It contains:
    - `Position`: An `IntVector` (short-based 3D coordinate).
    - `Materials`: An array of 6 bytes representing the material of each face.
    - `Corners`: An array of 8 bytes representing the offset of each corner vertex.
- **`PositionConverter`**: Handles the conversion between a single `byte` and a `Vector3`. It uses a step-based lookup table (`-0.5f, -0.25f, 0f, 0.25f, 0.5f`) to determine exact vertex placements.
- **`IntVector`**: A memory-efficient 3D vector using `short` values instead of `float` or `int`.

## 📦 Installation

This project targets `.NET Standard 2.0`. You can include it in your project by adding a reference to the compiled DLL or including the source files.

**Dependencies:**
- `System.Numerics.Vectors` (v4.6.1+)

## 💻 Usage Example

### Creating a Model
```csharp
using MauryDev.KoGaMa.ModelAPI.Model;
using MauryDev.KoGaMa.ModelAPI.Models;
using System.Collections.Generic;

var model = new ModelInfo();

var cube = new Cube
{
    Position = new IntVector(0, 0, 0),
    Materials = new byte[] { 1, 1, 1, 1, 1, 1 } // Example material IDs
};

model.Cubes.Add(cube);
```

### Converting Corner Bytes to Vectors
The `Cube` class provides a convenient `CornersVector` property that uses the `PositionConverter` internally:

```csharp
var cube = new Cube();
// These vectors represent the 8 corners of the cube relative to its position
System.Numerics.Vector3[] vertices = cube.CornersVector; 
```

### Implementing a Serializer
You can create your own serialization logic by implementing the `ISerializer` interface:

```csharp
public class MyModelSerializer : ISerializer
{
    public void Serialize(Stream stream, ModelInfo model) 
    {
        // Implement binary writing logic here
    }

    public ModelInfo Deserialize(Stream stream) 
    {
        // Implement binary reading logic here
        return new ModelInfo();
    }
}
```

## 📐 Coordinate System

The `PositionConverter` uses a 5 * 5 * 5 grid system. A byte value is decoded into X, Y, and Z coordinates as follows:
- **X**: `index / 25`
- **Y**: `(index % 25) / 5`
- **Z**: `index % 5`

Each index maps to one of the following floats: `{-0.5, -0.25, 0, 0.25, 0.5}`.

# Author

- MauryDev

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
