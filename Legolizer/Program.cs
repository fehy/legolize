using Legolize;
using LegoModeler;

namespace Legolizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bricks = Doer.Do(PointCloudGen.Generator.Cube(10, 8, 1));
            /*var bricks = new LegoModeler.Brick[2];
            bricks[0] = new LegoModeler.Brick(BrickType.B4x2, 0, 0, 0, BrickRotation.R0);
            bricks[1] = new LegoModeler.Brick(BrickType.B4x2, 2, 0, 0, BrickRotation.R0);
*/

            //new GenerateStaticBricks().OutputToLdr(bricks);
            new OpenGLSceneWriter("..\\..\\..\\Scenes\\Scene.scene");
        }
    }
}