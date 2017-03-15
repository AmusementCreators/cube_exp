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

        int[,,] FieldData;

        protected override void OnRegistered()
        {
            Layer = new asd.Layer3D();
                        AddLayer(Layer);

            var cam = new asd.CameraObject3D()
            {
                Position = new asd.Vector3DF(0, 20, 10),
                Focus = new asd.Vector3DF(10, 10, 0),
                FieldOfView = 50.0f,
                ZNear = 1.0f,
                ZFar = 100.0f,
                WindowSize = new asd.Vector2DI(800, 600),
            };
            Layer.AddObject(cam);

            var light1 = new asd.DirectionalLightObject3D()
            {
                Rotation = new asd.Vector3DF(10, 50, 50),
            };
            Layer.AddObject(light1);

            /*
            Box = new BoxObject()
            {
                Rotation = new asd.Vector3DF(20.0f, 20.0f, 0.0f)
            };
            Layer.AddObject(Box);
            */

            FieldData = new int[20, 20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    FieldData[i, j, 0] = 0;
                }
            }

            for (int i = 0; i < 20; i++)
            {
                //int j = 0;
                for (int j = 0; j < i; j++)
                {
                    int k = 0;
                    //for (int k = 0; k < 20; k++)
                    {
                        var b = new BoxObject()
                        {
                            Position = new asd.Vector3DF(i, j, k),
                        };
                        Layer.AddObject(b);
                    }
                }
            }
        }

        protected override void OnUpdating()
        {

        }
    }
}
