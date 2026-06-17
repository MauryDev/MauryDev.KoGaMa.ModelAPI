using System;
using System.Collections.Generic;
using System.Text;

namespace MauryDev.KoGaMa.ModelAPI.Enums
{


    public enum Edge { None, Front, Back, Left, Right }

    [Flags]
    public enum FaceFlags : byte
    {
        Top = 1, Bottom = 2, Front = 4, Back = 8, Left = 16, Right = 32
    }

    public enum Face { Top, Bottom, Front, Back, Left, Right }

    public enum BoundCorners
    {
        UpperFrontLeft, UpperFrontRight, UpperBackRight, UpperBackLeft,
        LowerBackLeft, LowerBackRight, LowerFrontRight, LowerFrontleft
    }
    
}
