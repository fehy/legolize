﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Legolize.Algo
{
    static class BrickExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LineCollision(int aMin, int aMax, int bMin, int bMax)
        {
            return (aMin >= bMin && aMin < bMax) || (bMin >= aMin && bMin < aMax);            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LineInTouch(int aMin, int aMax, int bMin, int bMax)
        {
            return (aMin == bMax) || (bMin == aMax);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LineCollisionSize(int aMin, int aMax, int bMin, int bMax)
        {
            var from = Math.Max(aMin, bMin);
            var to = Math.Min(aMax, bMax);

            return (to > from) ? to - from : 0;
        }

        public static bool InCollision(this Brick a, Brick b)
        {
            return LineCollision(a.LeftLowNear.X, a.RightUpFar.X, b.LeftLowNear.X, b.RightUpFar.X) &&
                LineCollision(a.LeftLowNear.Y, a.RightUpFar.Y, b.LeftLowNear.Y, b.RightUpFar.Y) &&
                LineCollision(a.LeftLowNear.Z, a.RightUpFar.Z, b.LeftLowNear.Z, b.RightUpFar.Z);
        }

        public static bool InTouch(this Brick a, Brick b)
        {
            if(LineInTouch(a.LeftLowNear.X, a.RightUpFar.X, b.LeftLowNear.X, b.RightUpFar.X))
                return LineCollision(a.LeftLowNear.Y, a.RightUpFar.Y, b.LeftLowNear.Y, b.RightUpFar.Y) &&
                LineCollision(a.LeftLowNear.Z, a.RightUpFar.Z, b.LeftLowNear.Z, b.RightUpFar.Z);

            if (!LineCollision(a.LeftLowNear.X, a.RightUpFar.X, b.LeftLowNear.X, b.RightUpFar.X))
                return false;

            if (LineInTouch(a.LeftLowNear.Y, a.RightUpFar.Y, b.LeftLowNear.Y, b.RightUpFar.Y))
                return LineCollision(a.LeftLowNear.Z, a.RightUpFar.Z, b.LeftLowNear.Z, b.RightUpFar.Z);

            // don't calculate Z
            //return LineCollision(a.LeftLowNear.Y, a.RightUpFar.Y, b.LeftLowNear.Y, b.RightUpFar.Y) &&
            //    LineInTouch(a.LeftLowNear.Z, a.RightUpFar.Z, b.LeftLowNear.Z, b.RightUpFar.Z);            
            return false;
        }

        public static int InTouchSize(this Brick a, Brick b)
        {
            if (LineInTouch(a.LeftLowNear.X, a.RightUpFar.X, b.LeftLowNear.X, b.RightUpFar.X))
                return LineCollisionSize(a.LeftLowNear.Y, a.RightUpFar.Y, b.LeftLowNear.Y, b.RightUpFar.Y) * 
                LineCollisionSize(a.LeftLowNear.Z, a.RightUpFar.Z, b.LeftLowNear.Z, b.RightUpFar.Z);

            var xLineCollisionSize = LineCollisionSize(a.LeftLowNear.X, a.RightUpFar.X, b.LeftLowNear.X, b.RightUpFar.X);
            if (xLineCollisionSize == 0)
                return 0;

            if (LineInTouch(a.LeftLowNear.Y, a.RightUpFar.Y, b.LeftLowNear.Y, b.RightUpFar.Y))
                return xLineCollisionSize * LineCollisionSize(a.LeftLowNear.Z, a.RightUpFar.Z, b.LeftLowNear.Z, b.RightUpFar.Z);

            // well for  Z
            return 0;            
        }
    }
}
