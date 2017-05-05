using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    class BoxObject : asd.ModelObject3D
    {
        public BoxObject()
        {

        }
    }

    static class BoxObjectFactory
    {
        private static asd.Model[] Models;

        public static void Initialize(string[] Textures)
        {
            Models = new asd.Model[Textures.Length];
            for (int i = 0; i < Textures.Length; i++)
            {
                Models[i] = asd.Engine.Graphics.CreateModel(@"Resources\Box" + i + ".mdl");
                var texture = asd.Engine.Graphics.CreateTexture2D(@"Resources\" + Textures[i]);
                Models[i].GetMesh(0).SetColorTexture(0, texture);
            }
        }

        public static T Create<T>(Vector3DI pos, int textureID) where T : BoxObject, new()
        {
            var obj = new T();
            obj.Position = pos.ToAsd3DF();
            obj.SetModel(Models[textureID]);
            obj.Scale = new asd.Vector3DF(0.5f, 0.5f, 0.5f);
            return obj;
        }
    }
}
