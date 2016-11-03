using System;
using Legolize.Algo;

namespace Legolize
{
    public static class Doer
    {
        private static LegoModeler.Brick[] Convert(Brick[] bricks)
        {
            var ret = new LegoModeler.Brick[bricks.Length];
            for(var i = 0; i < ret.Length; i++)
            {
                var vol = bricks[i].Volume;

                if (vol == 1)
                    ret[i] = new LegoModeler.Brick(LegoModeler.BrickType.B1x1, bricks[i].LeftLowNear.X, bricks[i].LeftLowNear.Y, bricks[i].LeftLowNear.Z, LegoModeler.BrickRotation.R0);
                else if (vol == 4)
                    ret[i] = new LegoModeler.Brick(LegoModeler.BrickType.B2x2, bricks[i].LeftLowNear.X, bricks[i].LeftLowNear.Y, bricks[i].LeftLowNear.Z, LegoModeler.BrickRotation.R0);
                else if (vol == 8)
                    ret[i] = new LegoModeler.Brick(LegoModeler.BrickType.B4x2, bricks[i].LeftLowNear.X, bricks[i].LeftLowNear.Y, bricks[i].LeftLowNear.Z,
                        (bricks[i].RightUpFar.X - bricks[i].LeftLowNear.X) == 4 ? LegoModeler.BrickRotation.R0 : LegoModeler.BrickRotation.R90);
                else
                    throw new NotSupportedException(vol.ToString());

            }

            return ret;
        }

        public static LegoModeler.Brick[] Do(PointCloud cloud)
        {
            Console.Write($"PointCloud with {cloud.Cloud.Length} points");

            // 1.) create model 
            var min = new MutablePoint(int.MaxValue, int.MaxValue, int.MaxValue);
            var max = new MutablePoint(int.MinValue, int.MinValue, int.MinValue);

            foreach(var point in cloud.Cloud)
            {
                min.X = Math.Min(min.X, point.X);
                min.Y = Math.Min(min.Y, point.Y);
                min.Z = Math.Min(min.Z, point.Z);

                max.X = Math.Max(max.X, point.X);
                max.Y = Math.Max(max.Y, point.Y);
                max.Z = Math.Max(max.Z, point.Z);
            }

            var model = new Model(max.X - min.X + 1, max.Y - min.Y + 1, max.Z - min.Z+1);
            foreach (var point in cloud.Cloud)
                model[point.X - min.X, point.Y - min.Y, point.Z - min.Z] = true;

            Console.Write(model);

            var master = new ModelMaster(model);
            master.Bricks.Push(new Brick(new Point(0,0,-1), new Point(max.X - min.X + 1, max.Y - min.Y + 1,0)));
            master.CreateNewSlots();

            var algo = new BruteForceAlgo(master);
            return Convert(algo.Go(1000000));
        }
    }
}
