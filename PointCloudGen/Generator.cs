using System.Collections.Generic;
using Legolize;
using System.Linq;
using PointCloudGen.Teselation;
using System.IO;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace PointCloudGen
{
    public static class Generator
    {
        public static PointCloud Cube(int x, int y, int z)
        {
            var cloud = new List<Point>(x * y * z);

            for(var ix = 0;  ix < x; ix++)
                for (var iy = 0; iy < y; iy++)
                    for (var iz = 0; iz < z; iz++)
                    {
                        cloud.Add(new Point(ix, iy, iz));
                    }

            return new PointCloud(cloud.ToArray());
        }

        public static PointCloud Cone(int startRadius, int endRadius, int step)
        {
            var cloud = new List<Point>();
            var z = 0;
            for(var radius = startRadius; radius != endRadius; radius += step, z++)
            {
                var radius2 = radius * radius;
                for (var x = -radius; x < radius; x++)
                {
                    var x2 = x * x;
                    for (var y = -radius; y < radius; y++)
                        if ((x2 + y * y) < radius2)
                            cloud.Add(new Point(x, y, z));
                }
            }
            
            return new PointCloud(cloud.ToArray());
        }

        public static PointCloud Clock(int startRadius, int endRadius)
        {
            var bottom = Cone(endRadius, startRadius, 1);
            var top = Cone(startRadius, endRadius, -1);

            var shift = endRadius - startRadius;
            return new PointCloud(bottom.Cloud.Concat(top.Cloud.Select(x => new Point(x.X, x.Y, x.Z + shift))).ToArray());
        }

        public static PointCloud RotatedClock(int startRadius, int endRadius)
        {
            var clock = Clock(startRadius, endRadius);
            return new PointCloud(clock.Cloud.Select(x => new Point(x.Z, x.Y, x.X)).ToArray());
        }


        public static PointCloud GenerateABSA(int height, int width)
        {
            var cloud = new List<Point>();
            int w, h;

            System.Drawing.Bitmap image = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(@"..\..\..\PointCloudGen\absaImage.bmp", false);
            w = image.Width;
            h = image.Height;

            for (int x = 0; x < w; x++)
            {
                for (int z = 0; z < h; z++)
                {
                    var p = image.GetPixel(x, z);
                    if (p != image.GetPixel(0, 0))
                    {
                        for(int y=0; y<10; y++)
                        cloud.Add(new Point(x, y, z));
                    }
                }
            }

            int i = 0;
            return new PointCloud(cloud.ToArray());
        }


        public static PointCloud FromObj(string fileName, float scale = 1)
        {
            var mesh = ObjReader.Read(fileName, scale);
            mesh.Validate();
            mesh.MakeNormals();

            var min = mesh.Min();
            var max = mesh.Max();

            var dir = new Vertex(1, 0, 0);

            var cloud = new ConcurrentBag<Point>();
            Parallel.For((int)min.Z, (int)max.Z, iz =>
            {
                Console.WriteLine(iz);
                for (var iy = (int)min.Y; iy < max.Y; iy++)
                {
                    Console.WriteLine(iy);
                    var r = mesh.Collide(new Vertex(0, iy, iz), dir);

                    for (var i = 1; i < r.Length; i += 2)
                    {
                        for (var ix = (int)r[i - 1]; ix < r[i]; ix++)
                            cloud.Add(new Point(ix, iy, iz));
                    }

                }
            });

            return new PointCloud(cloud.ToArray());
        }
    }
}
