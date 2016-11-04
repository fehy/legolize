using Legolize;
using LegoModeler;

namespace Legolizer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var bricks = Doer.Do(PointCloudGen.Generator.Cube(10, 8, 2));            
            //var bricks = Doer.Do(PointCloudGen.Generator.Clock(15, 4));
            //var bricks = Doer.Do(PointCloudGen.Generator.Cone(4, 15, 1));

            //var bricks = Doer.Do(PointCloudGen.Generator.GenerateFromBMP(@"..\..\..\PointCloudGen\absaImage.bmp", 100, 200, 5));
            //var bricks = Doer.Do(PointCloudGen.Generator.GenerateFromBMP(@"..\..\..\PointCloudGen\barclays.bmp", 50, 250, 1));
            var bricks = Doer.Do(PointCloudGen.Generator.GenerateFromBMP(@"..\..\..\PointCloudGen\barclaysSmall.bmp", 41, 250, 5));

            //var bricks = Doer.Do(PointCloudGen.Generator.RotatedClock(15, 4));

            /*var bricks = new LegoModeler.Brick[4];
            bricks[0] = new LegoModeler.Brick(BrickType.B4x2, 2, 1, 0, BrickRotation.R0);
            bricks[1] = new LegoModeler.Brick(BrickType.B4x2, 2, 3, 0, BrickRotation.R0);
            bricks[2] = new LegoModeler.Brick(BrickType.B4x2, 5, 2, 0, BrickRotation.R90);
            bricks[3] = new LegoModeler.Brick(BrickType.B4x2, 8, 2, 0, BrickRotation.R0);*/

            //new GenerateStaticBricks().OutputToLdr(bricks);

            //new OpenGLSceneWriter("..\\..\\..\\Scenes\\Scene.scene").Write(bricks);
        }
    }
}