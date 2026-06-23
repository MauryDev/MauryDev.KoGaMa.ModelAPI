using MauryDev.KoGaMa.ModelAPI.Enums;
using MauryDev.KoGaMa.ModelAPI.Utils;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace MauryDev.KoGaMa.ModelAPI.Model
{
    public class Cube
    {
        public const int MaterialsLen = 6;
        public const int CornersLen = 8;

        public static readonly byte[] IdentityByteCorners = { 20, 120, 124, 24, 4, 104, 100, 0 };

        public IntVector Position { get; set; }
        public byte[] Materials { get; set; } = new byte[MaterialsLen];
        public byte[] Corners { get; set; } = new byte[CornersLen];

        public Cube() { }

        public Cube(IntVector position, byte[] materials = null, byte[] corners = null)
        {
            Position = position;
            if (materials != null) Materials = materials;
            if (corners != null) Corners = corners;
        }

        public Vector3[] GetCornersVectors()
        {
            var vectors = new Vector3[CornersLen];
            for (int i = 0; i < CornersLen; i++)
            {
                vectors[i] = PositionConverter.GetVectorFromByte(Corners[i]);
            }
            return vectors;
        }

        public byte GetMaterial(Face face) => Materials[(int)face];

        public byte GetCorner(BoundCorners boundCorners) => Corners[(int)boundCorners];
    }
}
