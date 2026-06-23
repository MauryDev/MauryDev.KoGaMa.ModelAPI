# KoGaMa Model API

The **KoGaMa Model API** is a lightweight library, developed in `.NET Standard 2.0`, designed for the manipulation, processing, and serialization of KoGaMa 3D model data. It abstracts the complexity of representing blocks (voxels) that feature vertex deformations, per-face materials, and compressed coordinates.

## 🚀 Features

- **Cube Representation**: Complete block modeling, including global position, individual materials for each face, and vertex offsets (corners).
- **Model Management**: `ModelInfo` acts as a manager, allowing you to add, remove, and search for cubes by position/material, as well as calculate the model's Bounding Box.
- **Bidirectional Coordinate Conversion**: The `PositionConverter` translates compressed bytes into `Vector3` and vice versa, utilizing a $5 \times 5 \times 5$ grid system.
- **Optimized Data Structure**: Use of `IntVector` (based on `short`) to reduce the memory footprint in models containing thousands of cubes.
- **Serialization Interface**: A generic `ISerializer` for implementing persistence in Binary, JSON, or custom formats.
- **Maximum Compatibility**: Target `.NET Standard 2.0` (compatible with Unity, .NET Framework, and .NET Core/5+).

## 🛠 Technical Architecture

### 1. `ModelInfo` (The Model Manager)
More than just a container, this is the central class for model manipulation:
- **Search and Filtering**: Methods such as `FindCubeAt(position)` and `GetCubesByMaterial(id)`.
- **Bounding Box**: Automatically calculates the minimum and maximum limits of the model via the `GetBoundingBox()` method.
- **Manipulation**: Support for `AddCube`, `RemoveCube`, and `Clear`.

### 2. `Cube` (The Basic Unit)
Represents an individual block with the following properties:
- **`Position`**: Grid location via `IntVector`.
- **`Materials`**: An array of 6 bytes (one for each face defined in the `Face` enum).
- **`Corners`**: An array of 8 bytes that define vertex deformations. 
    - *Tip:* Use `Cube.IdentityByteCorners` to reset a cube to its default state.
- **`GetCornersVectors()`**: Converts corner bytes into actual `Vector3` values for rendering.

### 3. `PositionConverter` (The Mathematical Core)
Maps a single `byte` (0-124) to a `Vector3` based on a 5-level grid:
- **Reference values**: `{-0.5f, -0.25f, 0f, 0.25f, 0.5f}`.
- **Calculation**:
  - $X = index / 25$
  - $Y = (index \pmod{25}) / 5$
  - $Z = index \pmod 5$

### 4. `IntVector`
An optimized `struct` for integer coordinates, avoiding the overhead of `float` when unnecessary and providing access via an indexer (`vector[0]` for X, etc.).

---

## 💻 Usage Examples

### Creating and Managing a Model
```csharp
using MauryDev.KoGaMa.ModelAPI.Model;
using MauryDev.KoGaMa.ModelAPI.Models;

var model = new ModelInfo();

// Creating a cube at position (0, 1, 0)
var cube = new Cube(new IntVector(0, 1, 0)) 
{
    Materials = new byte[] { 1, 1, 1, 1, 1, 1 } 
};

model.AddCube(cube);

// Checking if a cube exists at a certain position
if (model.HasCubeAt(new IntVector(0, 1, 0))) 
{
    var foundCube = model.FindCubeAt(new IntVector(0, 1, 0));
}

// Getting the model's Bounding Box
var (min, max) = model.GetBoundingBox();
Console.WriteLine($"Model expands from {min} to {max}");
```

### Working with Vertices (Corners)
```csharp
var cube = new Cube(new IntVector(0, 0, 0));
cube.Corners = Cube.IdentityByteCorners; // Sets default corners

// Gets the actual positions of the 8 vertices for rendering
System.Numerics.Vector3[] vertices = cube.GetCornersVectors();

foreach (var v in vertices)
{
    Console.WriteLine($"Vertex: {v}");
}
```

### Manual Coordinate Conversion
```csharp
using MauryDev.KoGaMa.ModelAPI.Utils;

// From Byte to Vector3
byte byteVal = 124; 
System.Numerics.Vector3 vec = PositionConverter.GetVectorFromByte(byteVal);

// From Vector3 to Byte
byte backToByte = PositionConverter.GetByteFromVector(vec);
```

---

## 📦 Installation

Add the reference to the compiled DLL to your project or include the source files.

**Dependencies:**
- `System.Numerics.Vectors` (v4.6.1+)

## 📐 Coordinate Table (5x5x5 Grid)

| Index | Float Value |
| :--- | :--- |
| 0 | -0.5f |
| 1 | -0.25f |
| 2 | 0f |
| 3 | 0.25f |
| 4 | 0.5f |

## 👤 Author

- **MauryDev**

## 📄 License

This project is under the MIT license. See the [LICENSE](LICENSE) file for more details.