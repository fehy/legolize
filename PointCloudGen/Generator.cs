using System.Collections.Generic;
using Legolize;

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
    }
}
