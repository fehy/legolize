using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointCloudGen.Teselation
{
    public struct Vertex
    {
        public readonly float X, Y, Z;

        public Vertex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

       

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }

    public struct Face
    {
        public readonly int X, Y, Z;

        public Vertex Normal;

        public Face(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;

            Normal = new Vertex(0, 0, 0);
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }

    public static class VertexExtensions
    {
        public static Vertex Add(this Vertex a, Vertex b)
        {
            return new Vertex(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vertex Sub(this Vertex a, Vertex b)
        {
            return new Vertex(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vertex Mult(this Vertex a, float b)
        {
            return new Vertex(a.X * b, a.Y * b, a.Z * b);
        }

        public static float ScalarMult(this Vertex a, Vertex b)
        {
            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }
         
        public static Vertex VectorMult(this Vertex a, Vertex b)
        {
            return new Vertex(a.Y * b.Z - a.Z*b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public static Vertex Normalize(this Vertex a)
        {
            var norm = (float)(1.0f / Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z));

            return new Vertex(a.X * norm, a.Y * norm, a.Z * norm);
        }
    }

    class Mesh
    {
        private readonly Vertex[] _vertexes;
        private Face[] _faces;

        public Mesh(Vertex[] vertexes, Face[] faces)
        {
            _vertexes = vertexes;
            _faces = faces;
        }

        public void Validate()
        {
            
            foreach (var face in _faces)
            {
                if (face.X < 0 || face.X >= _vertexes.Length ||
                    face.Y < 0 || face.Y >= _vertexes.Length ||
                    face.Z < 0 || face.Z >= _vertexes.Length)
                    throw new NotSupportedException(face.ToString());
            }

            /*    var v0 = _vertexes[face.X];
                var v1 = _vertexes[face.Y];
                var v2 = _vertexes[face.Z];

                if(v0.Equals(v1) || v1.Equals(v2) || 
            }*/
        }

        public void MakeNormals()
        {
            for (var i = 0; i < _faces.Length; ++i)
            {
                var v0 = _vertexes[_faces[i].X];
                var v1 = _vertexes[_faces[i].Y];
                var v2 = _vertexes[_faces[i].Z];

                var p1 = v1.Sub(v0);
                var p2 = v2.Sub(v0);

                var normal = p1.VectorMult(p2).Normalize();

                _faces[i].Normal = normal;
            }
        }

        public Vertex Min()
        {
            var xMin = float.MaxValue;
            var yMin = float.MaxValue;
            var zMin = float.MaxValue;

            foreach (var vert in _vertexes)
            {
                xMin = Math.Min(xMin, vert.X);
                yMin = Math.Min(yMin, vert.Y);
                zMin = Math.Min(zMin, vert.Z);
            }

            return new Vertex(xMin, yMin, zMin);
        }

        public Vertex Max()
        {
            var xMax = float.MinValue;
            var yMax = float.MinValue;
            var zMax = float.MinValue;

            foreach (var vert in _vertexes)
            {
                xMax = Math.Max(xMax, vert.X);
                yMax = Math.Max(yMax, vert.Y);
                zMax = Math.Max(zMax, vert.Z);
            }

            return new Vertex(xMax, yMax, zMax);
        }

        public float[] Collide(Vertex P0, Vertex dir)
        {

            var res = new List<float>();
            foreach (var face in _faces)
            {
                var v0 = _vertexes[face.X];
                var v1 = _vertexes[face.Y];
                var v2 = _vertexes[face.Z];

                var r = face.Normal.ScalarMult(v0.Sub(P0)) / face.Normal.ScalarMult(dir);
                if (Single.IsNaN(r) || Single.IsInfinity(r) )
                    continue;

                var Pt = P0.Add(dir.Mult(r));

                var l0 = Pt.Sub(v0);
                var l1 = Pt.Sub(v1);
                var l2 = Pt.Sub(v2);

                if (l0.VectorMult(l1).ScalarMult(face.Normal) < 0)
                    continue;

                if (l1.VectorMult(l2).ScalarMult(face.Normal) < 0)
                    continue;

                if (l2.VectorMult(l0).ScalarMult(face.Normal) < 0)
                    continue;

                res.Add(r);
            }

            res.Sort();
            var ret = res.Distinct().ToArray();
            return ret;
        }
    }

}
