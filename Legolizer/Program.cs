﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legolize;
using PointCloudGen;
using LegoModeler;

namespace Legolizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bricks = Doer.Do(PointCloudGen.Generator.Cube(10, 8, 2));
            //var bricks = Doer.Do(PointCloudGen.Generator.Cone(4, 15, 1));
            /*var bricks = new LegoModeler.Brick[4];
            bricks[0] = new LegoModeler.Brick(BrickType.B4x2, 2, 1, 0, BrickRotation.R0);
            bricks[1] = new LegoModeler.Brick(BrickType.B4x2, 2, 3, 0, BrickRotation.R0);
            bricks[2] = new LegoModeler.Brick(BrickType.B4x2, 5, 2, 0, BrickRotation.R90);
            bricks[3] = new LegoModeler.Brick(BrickType.B4x2, 8, 2, 0, BrickRotation.R0);*/

            new GenerateStaticBricks().OutputToLdr(bricks);

        }

     
    }
}
