using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointCloudGen.Teselation
{
    static class ObjReader
    {
        public static Mesh Read(string filename, float scale)
        {
            var lines = File.ReadLines(filename);

            var vertexes = new List<Vertex>();
            var faces = new List<Face>();
            foreach (var line in lines)
            {
                if (line.StartsWith("v "))
                {
                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 4)
                        //throw new NotSupportedException();
                        continue;

                    vertexes.Add(new Vertex(float.Parse(parts[1]) * scale, float.Parse(parts[2]) * scale, float.Parse(parts[3])* scale));
                }
                else if(line.StartsWith("f "))
                {
                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 4)
                        //throw new NotSupportedException();
                        continue;

                    faces.Add(new Face(int.Parse(parts[1].Split('/')[0]), int.Parse(parts[2].Split('/')[0]), int.Parse(parts[3].Split('/')[0])));
                }
            }

            // normalize faces 
            var nVertexes = vertexes.Count;
            Func<int,int> f = x => x > 0 ? x-1 : nVertexes + x;

            var facesNom = faces.Select(x => new Face(f(x.X), f(x.Y), f(x.Z)));
            return new Mesh(vertexes.ToArray(), facesNom.ToArray());            
        }
    }
}
