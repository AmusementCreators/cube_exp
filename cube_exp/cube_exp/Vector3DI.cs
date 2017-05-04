using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    public struct Vector3DI
    {
        public int X, Y, Z;

        public Vector3DI(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3DI operator +(Vector3DI v1, Vector3DI v2)
        {
            return new Vector3DI(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3DI operator -(Vector3DI v1, Vector3DI v2)
        {
            return new Vector3DI(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3DI operator *(Vector3DI v, int d)
        {
            return new Vector3DI(d * v.X, d * v.Y, d * v.Z);
        }

        public static Vector3DI operator *(int d, Vector3DI v)
        {
            return new Vector3DI(d * v.X, d * v.Y, d * v.Z);
        }

        public static Vector3DI operator /(Vector3DI v, int d)
        {
            return new Vector3DI(v.X / d, v.Y / d, v.Z / d);
        }

        public static bool operator ==(Vector3DI v1, Vector3DI v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        public static bool operator !=(Vector3DI v1, Vector3DI v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
        }

        public asd.Vector3DF ToAsd3DF()
        {
            return new asd.Vector3DF(X, Y, Z);
        }

        public asd.Vector2DF XZToAsd2DF()
        {
            return new asd.Vector2DF(X, Z);
        }

        public static Vector3DI FromAsd3DF(asd.Vector3DF v)
        {
            return new Vector3DI((int)v.X, (int)v.Y, (int)v.Z);
        }
        public static Vector3DI FromAsd2DF(asd.Vector2DF v)
        {
            return new Vector3DI((int)v.X, 0, (int)v.Y);
        }
    }
}
