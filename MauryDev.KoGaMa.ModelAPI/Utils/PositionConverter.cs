using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MauryDev.KoGaMa.ModelAPI.Utils
{
    public static class PositionConverter
    {
        private static readonly float[] Steps = { -0.5f, -0.25f, 0f, 0.25f, 0.5f };

        public static Vector3 GetVectorFromByte(byte index)
        {
            int x = index / 25;
            int y = (index % 25) / 5;
            int z = index % 5;
            return new Vector3(Steps[x], Steps[y], Steps[z]);
        }

        public static byte GetByteFromVector(Vector3 pos)
        {
            int x = Array.IndexOf(Steps, pos.X);
            int y = Array.IndexOf(Steps, pos.Y);
            int z = Array.IndexOf(Steps, pos.Z);

            if (x == -1 || y == -1 || z == -1)
                throw new ArgumentException("Position outside the lookup table");

            return (byte)(x * 25 + y * 5 + z);
        }
    }

}
