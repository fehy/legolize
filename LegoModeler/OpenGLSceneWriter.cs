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
            Directory.CreateDirectory(fileName);
            _fileName = fileName;
        }

        public void Write(Brick[] scene)
        {
            if (File.Exists(_fileName))
                return; // yield, not consumed yet

            var streamCandidate = OpenStream();
            if (streamCandidate == null)
                return; // yield, takes too long to unlock

            using (var stream = streamCandidate)
            {
                foreach (var brick in scene)
                {
                    Write(stream, (int)brick.BrickType);
                    Write(stream, (float)brick.X);
                    Write(stream, (float)brick.Y);
                    Write(stream, (float)brick.Z);

                    switch (brick.BrickRotation)
                    {
                        default:
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
                    }
                }
            }
        }

        private Stream OpenStream(int timeout = 1000)
        {
            var timeAvailable = timeout;
            var minSleep = timeout / 100 + 1;
            var maxSleep = timeout / 50 + 2;

            while(timeAvailable > 0)
            {
                try
                {
                    return new FileStream(_fileName, FileMode.Create, FileAccess.Write);
                }
                catch (IOException ex)
                {
                    if (ex.GetType() != typeof(IOException))
                        throw;

                    var sleepTime = Math.Min(_rnd.Next(minSleep, maxSleep), timeAvailable);
                    Thread.Sleep(sleepTime);
                    timeAvailable -= sleepTime;

                    continue;
                }
            }

            return null;
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