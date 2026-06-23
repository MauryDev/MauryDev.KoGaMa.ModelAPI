using MauryDev.KoGaMa.ModelAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MauryDev.KoGaMa.ModelAPI.Official
{
    public static class CubeGrouper
    {
        const int MaxLenRow = 63;

        public static List<List<Cube>> GroupIntoRows(IReadOnlyCollection<Cube> allCubes)
        {
            var groups = new List<List<Cube>>();
            var processed = new HashSet<Cube>();

            var sortedCubes = allCubes
                .OrderBy(c => c.Position.Z)
                .ThenBy(c => c.Position.Y)
                .ThenBy(c => c.Position.X)
                .ToList();

            foreach (var cube in sortedCubes)
            {
                if (processed.Contains(cube)) continue;

                var group = new List<Cube> { cube };
                processed.Add(cube);

                int count = 1;
                while (count < MaxLenRow)
                {
                    var nextCube = FindNextCompatibleCube(sortedCubes, processed, cube, count);
                    if (nextCube == null) break;

                    group.Add(nextCube);
                    processed.Add(nextCube);
                    count++;
                }
                groups.Add(group);
            }
            return groups;
        }

        private static Cube FindNextCompatibleCube(List<Cube> sortedCubes, HashSet<Cube> processed, Cube baseCube, int offset)
        {
            return sortedCubes.FirstOrDefault(c =>
                !processed.Contains(c) &&
                c.Position.X == baseCube.Position.X + offset &&
                c.Position.Y == baseCube.Position.Y &&
                c.Position.Z == baseCube.Position.Z &&
                c.Materials.SequenceEqual(baseCube.Materials) &&
                c.Corners.SequenceEqual(baseCube.Corners));
        }
    }
}
