using System;
using Legolize.Algo;
using System.Collections.Generic;

namespace Legolize
{
    public static class Doer
    {
        private static LegoModeler.Brick[] Convert(Brick[] bricks)
        {
            var ret = new List<LegoModeler.Brick>(bricks.Length -1);
            for(var i = 0; i < bricks.Length; i++)
            {
                var vol = bricks[i].Volume;

                if (vol == 1)
                    // TODO: hacking to workaround model problem
                    ret.Add(new LegoModeler.Brick(LegoModeler.BrickType.B1x1, bricks[i].LeftLowNear.X + 0.5f, bricks[i].LeftLowNear.Y+0.5f, bricks[i].LeftLowNear.Z, LegoModeler.BrickRotation.R0));
                else if (vol == 4)
                    ret.Add(new LegoModeler.Brick(LegoModeler.BrickType.B2x2, bricks[i].LeftLowNear.X + 1, bricks[i].LeftLowNear.Y + 1, bricks[i].LeftLowNear.Z, LegoModeler.BrickRotation.R0));
                else if (vol == 8)
                {
                    var rot = (bricks[i].RightUpFar.X - bricks[i].LeftLowNear.X) == 4 ? LegoModeler.BrickRotation.R0 : LegoModeler.BrickRotation.R90;
                    ret.Add(new LegoModeler.Brick(LegoModeler.BrickType.B4x2, 
                        bricks[i].LeftLowNear.X + (rot == LegoModeler.BrickRotation.R0 ? 2 : 1), 
                        bricks[i].LeftLowNear.Y + (rot == LegoModeler.BrickRotation.R0 ? 1 : 2), bricks[i].LeftLowNear.Z,
                        rot));
                }
            }

            return ret.ToArray();
        }

        public static LegoModeler.Brick[] Do(PointCloud cloud)
        {
            Console.Write($"PointCloud with {cloud.Cloud.Length} points");

            // 1.) create bottom model 
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
            master.SlotsToSearch = Tuple.Create(0, master.Slots.Count);

            var algo = new BruteForceAlgo(master);
            return Convert(algo.Go(10000000));
        }
    }
}
