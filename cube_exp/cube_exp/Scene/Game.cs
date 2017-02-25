using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp.Scene
{
    class Game : asd.Scene
    {
        asd.Layer3D Layer;
        BoxObject Box;

        protected override void OnRegistered()
        {
            Layer = new asd.Layer3D();
            AddLayer(Layer);

            var cam = new asd.CameraObject3D()
            {
                Position = new asd.Vector3DF(0, 0, 10),
                Focus = new asd.Vector3DF(0, 0, 0),
                FieldOfView = 20.0f,
                ZNear = 1.0f,
                ZFar = 20.0f,
                WindowSize = new asd.Vector2DI(800, 600),
            };
            Layer.AddObject(cam);

            var light = new asd.DirectionalLightObject3D()
            {
                Rotation = new asd.Vector3DF(30, 160, 0),
            };
            Layer.AddObject(light);

            Box = new BoxObject()
            {
                Rotation = new asd.Vector3DF(20.0f, 20.0f, 0.0f)
            };
            Layer.AddObject(Box);
        }

        protected override void OnUpdating()
        {
            var r = Box.Rotation;
            r.X += 0.5f;
            Box.Rotation = r;
        }
    }
}
