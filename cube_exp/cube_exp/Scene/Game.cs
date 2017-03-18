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
                Position = new asd.Vector3DF(100, 100, 100),
                Focus = new asd.Vector3DF(10, 0, 10),
                FieldOfView = 10.0f,
                ZNear = 1.0f,
                ZFar = 200.0f,
                WindowSize = new asd.Vector2DI(800, 600),
            };
            Layer.AddObject(cam);

            var light1 = new asd.DirectionalLightObject3D()
            {
                Rotation = new asd.Vector3DF(10, 50, 50),
            };
            Layer.AddObject(light1);

            FieldData = new int[20, 20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        FieldData[i, j, k] = j == 0 ? (i + k) % 2 : 0;
                    }
                }
            }

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        if (FieldData[i, j, k] == 1)
                        {
                            var b = new BoxObject()
                            {
                                Position = new asd.Vector3DF(i, j, k),
                                IsDrawn = FieldData[i, j, k] == 1,
                            };
                            Layer.AddObject(b);
                        }
                    }
                }
            }
        }

        protected override void OnUpdating()
        {

        }
    }
}
