using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    public static class Helper
    {
        public static asd.Vector3DF Vector2DITo3DFXZ(asd.Vector2DI v)
        {
            return new asd.Vector3DF(v.X, 0, v.Y);
        }


        public static asd.Vector3DF Vector2DFTo3DFXZ(asd.Vector2DF v)
        {
            return new asd.Vector3DF(v.X, 0, v.Y);
        }

        public static asd.Vector2DI Vector3DFXZTo2DI(asd.Vector3DF v)
        {
            return new asd.Vector2DI((int)v.X, (int)v.Y);
        }
    }
}
