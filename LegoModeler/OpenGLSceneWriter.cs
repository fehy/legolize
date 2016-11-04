using System;
using System.IO;
using System.Threading;

namespace LegoModeler
{
    public class OpenGLSceneWriter
    {
        private readonly string _fileName;
        private readonly Random _rnd = new Random();

        public OpenGLSceneWriter(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            _fileName = fileName;
        }

        public void Write(Brick[] scene)
        {
            if (File.Exists(_fileName))
                return; // yield, not consumed yet

            // No multicheck, as file does not exist, so we are creating it
            using (var stream = new FileStream(_fileName, FileMode.Create, FileAccess.Write))
            {
                foreach (var brick in scene)
                {
                    Write(stream, Model(brick));
                    Write(stream, brick.X);
                    Write(stream, brick.Y);
                    Write(stream, brick.Z);

                    switch (brick.BrickRotation)
                    {
                        case BrickRotation.R0:
                            Write(stream, 0);
                            break;

                        case BrickRotation.R90:
                            Write(stream, 90);
                            break;

                        case BrickRotation.R180:
                            Write(stream, 180);
                            break;

                        case BrickRotation.R270:
                            Write(stream, 270);
                            break;

                        default:
                            throw new ArgumentException(brick.BrickRotation.ToString());
                    }
                }
            }
        }
        
        private static int Model(Brick brick)
        {
            const int TotalColors = 4; // RGBY
            var color = ((int)(brick.X + brick.Y + brick.Z)) % TotalColors;

            switch (brick.BrickType)
            {
                case BrickType.B1x1:
                    return TotalColors * 0 + color;

                case BrickType.B2x2:
                    return TotalColors * 1 + color;

                case BrickType.B4x2:
                    return TotalColors * 2 + color;

                default:
                    throw new ArgumentException(brick.BrickType.ToString());
            }
        }

        private static void Write(Stream stream, int val)
        {
            var bytes = BitConverter.GetBytes(val);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static void Write(Stream stream, float val)
        {
            var bytes = BitConverter.GetBytes(val);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}