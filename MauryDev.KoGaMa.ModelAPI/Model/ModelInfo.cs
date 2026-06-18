using MauryDev.KoGaMa.ModelAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace MauryDev.KoGaMa.ModelAPI.Model
{
    public class ModelInfo
    {
        private readonly List<Cube> _cubes;

       
        public IReadOnlyList<Cube> Cubes => _cubes;

        
        public int CubeCount => _cubes.Count;

        public ModelInfo()
        {
            _cubes = new List<Cube>();
        }

        public ModelInfo(IEnumerable<Cube> cubes)
        {
            if (cubes != null)
            {
                _cubes.AddRange(cubes);
            }
        }

        public void AddCube(Cube cube)
        {
            if (cube == null) return;
            _cubes.Add(cube);
        }

        public void AddRange(IEnumerable<Cube> cubes)
        {
            if (cubes == null) return;
            _cubes.AddRange(cubes);
        }

        public void RemoveCube(Cube cube) => _cubes.Remove(cube);
        public void RemoveAtCube(int index) => _cubes.RemoveAt(index);

        public void Clear() => _cubes.Clear();

        public Cube FindCubeAt(IntVector position)
          => _cubes.Find(c => c.Position == position);

        public bool HasCubeAt(IntVector position)
            => _cubes.Exists(c => c.Position == position);

        public IEnumerable<Cube> GetCubesByMaterial(byte materialId)
            => _cubes.Where(c => c.Materials.Contains(materialId));

        public int RemoveCubesAt(IntVector position)
        {
            int removedCount = _cubes.RemoveAll(c => c.Position == position);
            return removedCount;
        }

        public (IntVector Min, IntVector Max) GetBoundingBox()
        {
            if (_cubes.Count == 0)
                return (new IntVector(0, 0, 0), new IntVector(0, 0, 0));

            short minX = short.MaxValue, minY = short.MaxValue, minZ = short.MaxValue;
            short maxX = short.MinValue, maxY = short.MinValue, maxZ = short.MinValue;

            foreach (var cube in _cubes)
            {
                if (cube.Position.X < minX) minX = cube.Position.X;
                if (cube.Position.X > maxX) maxX = cube.Position.X;
                if (cube.Position.Y < minY) minY = cube.Position.Y;
                if (cube.Position.Y > maxY) maxY = cube.Position.Y;
                if (cube.Position.Z < minZ) minZ = cube.Position.Z;
                if (cube.Position.Z > maxZ) maxZ = cube.Position.Z;
            }

            return (new IntVector(minX, minY, minZ), new IntVector(maxX, maxY, maxZ));
        }
    }
}
