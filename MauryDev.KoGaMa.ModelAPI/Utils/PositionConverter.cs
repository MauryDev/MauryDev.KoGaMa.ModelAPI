using System;
using System.Numerics;

namespace MauryDev.KoGaMa.ModelAPI.Utils
{

    public static class PositionConverter
    {
        private static readonly float[] CoordinateValues = { -0.5f, -0.25f, 0f, 0.25f, 0.5f };

        private const int AxisSize = 5;
        private const int PlaneSize = 25;

        public static Vector3 GetVectorFromByte(byte index)
        {
            int xIndex = index / PlaneSize;
            int yIndex = (index % PlaneSize) / AxisSize;
            int zIndex = index % AxisSize;

            return new Vector3(
                CoordinateValues[xIndex],
                CoordinateValues[yIndex],
                CoordinateValues[zIndex]
            );
        }

        public static byte GetByteFromVector(Vector3 position)
        {
            int xIndex = Array.IndexOf(CoordinateValues, position.X);
            int yIndex = Array.IndexOf(CoordinateValues, position.Y);
            int zIndex = Array.IndexOf(CoordinateValues, position.Z);

            if (xIndex == -1 || yIndex == -1 || zIndex == -1)
            {
                throw new ArgumentException($"Position {position} is outside the supported coordinate range.");
            }

            return (byte)((xIndex * PlaneSize) + (yIndex * AxisSize) + zIndex);
        }
    }
}