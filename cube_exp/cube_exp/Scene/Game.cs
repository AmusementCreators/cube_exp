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
        asd.CameraObject3D Camera;
        BoxObject Character;

        int[,,] FieldData;

        protected override void OnRegistered()
        {
            Layer = new asd.Layer3D();
            AddLayer(Layer);

            Camera = new asd.CameraObject3D()
            {
                Position = new asd.Vector3DF(-5, 5, 10),
                Focus = new asd.Vector3DF(10.5f, 0, 10.5f),
                FieldOfView = 70.0f,
                ZNear = 1.0f,
                ZFar = 100.0f,
                WindowSize = new asd.Vector2DI(800, 600),
            };
            Layer.AddObject(Camera);

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
                        FieldData[i, j, k] = j == 0 ? (i + k) % 2 + 1 : 0;
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


            Character = new Player()
            {
                Position = new asd.Vector3DF(0, 0, 0)
            };
            Layer.AddObject(Character);
        }

        protected override void OnUpdating()
        {
            Camera.Focus = new asd.Vector3DF(Character.Position.X, 0, Character.Position.Z);
        }
    }
}
