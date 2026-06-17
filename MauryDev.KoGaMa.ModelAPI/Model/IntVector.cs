using System;

namespace MauryDev.KoGaMa.ModelAPI.Models
{
    public struct IntVector
    {
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }

        public IntVector(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public short this[int key]
        {
            get
            {
                switch (key)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (key)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }
    }
}