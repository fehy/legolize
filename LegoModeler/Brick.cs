
namespace LegoModeler
{
    public enum BrickType
    {
        B1x1,
        B2x2,
        B4x2
    }

    public enum BrickRotation
    {
        R0,
        R90,
        R180,
        R270
    }

    public struct Brick
    {
        public readonly BrickType BrickType;
        public readonly float X, Y, Z;        
        public readonly BrickRotation BrickRotation;

        public Brick(BrickType brickType, float x, float y, float z, BrickRotation brickRotation)
        {
            BrickType = brickType;
            X = x;
            Y = y;
            Z = z;
            BrickRotation = brickRotation;
        }
    }    
}
