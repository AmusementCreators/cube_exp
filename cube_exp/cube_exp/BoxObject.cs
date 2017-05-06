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

        public static void Initialize(int length)
        {
            Models = new asd.Model[length + 1];
            for (int i = 1; i <= length; i++)
            {
                Models[i] = asd.Engine.Graphics.CreateModel(@"Resources\Box" + i + ".mdl");
                var texture = asd.Engine.Graphics.CreateTexture2D($@"Resources\{i}.png");
                Models[i].GetMesh(0).SetColorTexture(0, texture);
            }
        }

        public static T Create<T>(Vector3DI pos, int textureID) where T : BoxObject, new()
        {
            if (textureID == 0) return null;
            var obj = new T();
            obj.Position = pos.ToAsd3DF();
            UpdateModel(obj, textureID);
            return obj;
        }

        public static void UpdateModel<T>(T obj, int textureID) where T : BoxObject, new()
        {
            if (textureID == 0) return;
            obj.SetModel(Models[textureID]);
            obj.Scale = new asd.Vector3DF(0.5f, 0.5f, 0.5f);
        }
    }
}
