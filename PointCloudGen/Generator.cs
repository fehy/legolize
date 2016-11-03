using System.Collections.Generic;
using Legolize;
using System.Linq;
using PointCloudGen.Teselation;

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

        public static PointCloud FromObj(string fileName, float scale = 1)
        {
            var mesh = ObjReader.Read(fileName, 10);
            mesh.Validate();

            mesh.MakeNormals();

            var r = mesh.Collide(new Vertex(-1, 0.2f, 0.4f), new Vertex(1, 0, 0).Normalize());
            return Clock(10, 5);
        }
    }
}
