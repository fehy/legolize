﻿using Legolize;
using LegoModeler;
using System.Linq;
using System.Threading;

namespace Legolizer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var bricks = Doer.Do(PointCloudGen.Generator.GenerateFromBMP(@"..\..\..\PointCloudGen\absaImage.bmp", 100, 200, 6));
            //var bricks = Doer.Do(PointCloudGen.Generator.FromObj(@"C:\work\users\fehy\playground\hackatlon\data\Wolf.obj", 0.2f, 0));
            //var bricks = Doer.Do(PointCloudGen.Generator.GenerateFromBMP(@"..\..\..\PointCloudGen\barclays.bmp", 30, 150, 6)););
            //var bricks = Doer.Do(PointCloudGen.Generator.RotatedClock(15, 4));

            var bricks = Doer.Do(PointCloudGen.Generator.Cube(10, 8, 2));
            //var bricks = Doer.Do(PointCloudGen.Generator.Clock(15, 4));
            //var bricks = Doer.Do(PointCloudGen.Generator.Cone(4, 15, 1));

            //var bricks = Doer.Do(PointCloudGen.Generator.GenerateFromBMP(@"..\..\..\PointCloudGen\absaImage.bmp", 100, 200, 5));
            //var bricks = Doer.Do(PointCloudGen.Generator.FromObj(@"C:\work\users\fehy\playground\hackatlon\data\Wolf.obj", 0.2f, 0));
            //var bricks = Doer.Do(PointCloudGen.Generator.GenerateFromBMP(@"..\..\..\PointCloudGen\barclays.bmp", 30, 150, 6));
            //var bricks = Doer.Do(PointCloudGen.Generator.GenerateFromBMP(@"..\..\..\PointCloudGen\barclaysSmall.bmp", 41, 250, 5));

            //var bricks = Doer.Do(PointCloudGen.Generator.FromObj(@"C:\work\users\fehy\playground\hackatlon\data\skull.obj", 4f, 0));
            //var bricks = Doer.Do(PointCloudGen.Generator.FromObj(@"C:\work\users\fehy\playground\hackatlon\data\Wolf.obj", 0.2f, 0));
            //var bricks = Doer.Do(PointCloudGen.Generator.FromObj(@"C:\work\users\fehy\playground\hackatlon\data\Wolf.obj", 0.3f,4));
            //var bricks = Doer.Do(PointCloudGen.Generator.RotatedClock(15, 4));
            //var bricks = Doer.Do(PointCloudGen.Generator.FromObj(@"C:\work\users\fehy\playground\hackatlon\data\Touareg.obj", 0.03f));
            //var bricks = Doer.Do(PointCloudGen.Generator.FromObj(@"C:\work\users\fehy\playground\hackatlon\data\Ball OBJ.obj",10));
            //var bricks = Doer.Do(PointCloudGen.Generator.GenerateABSA(0,0));
            /*var bricks = new LegoModeler.Brick[4];
            bricks[0] = new LegoModeler.Brick(BrickType.B4x2, 2, 1, 0, BrickRotation.R0);
            bricks[1] = new LegoModeler.Brick(BrickType.B4x2, 2, 3, 0, BrickRotation.R0);
            bricks[2] = new LegoModeler.Brick(BrickType.B4x2, 5, 2, 0, BrickRotation.R90);
            bricks[3] = new LegoModeler.Brick(BrickType.B4x2, 8, 2, 0, BrickRotation.R0);*/

            //new GenerateStaticBricks().OutputToLdr(bricks);
            bricks= bricks.Reverse().ToArray();
            for (var i = 1; i <= bricks.Length; i++)
            {
                new OpenGLSceneWriter("..\\..\\..\\Scenes\\Scene.scene").Write(bricks.Take(i).ToArray());
                Thread.Sleep(20);
            }
        }
    }
}