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

        protected override void OnRegistered()
        {
            Layer = new asd.Layer3D();
            AddLayer(Layer);

            var testMap = new MapRawData("MapTest.txt");
            foreach (var o in testMap)
            {
                Layer.AddObject(o);
            }

            Camera = new asd.CameraObject3D()
            {
                Position = new asd.Vector3DF(testMap.SizeX, 5, testMap.SizeZ),
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
