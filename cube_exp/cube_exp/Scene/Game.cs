using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp.Scene
{
    class Game : asd.Scene
    {
        public asd.CameraObject3D Camera { get; private set; }
        public Slime Character { get; private set; }

        public float CameraAngleH { get; private set; } = (float)Math.PI / 4.0f;
        public float CameraAngleR { get; private set; } = (float)Math.PI / 4.0f;

        private MapRawData MapData;

        public int GetMapData(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0) return 0;
            if (x >= MapData.SizeX || y >= MapData.SizeY || z >= MapData.SizeZ) return 0;
            return MapData.GetData(x, y, z);
        }

        protected override void OnRegistered()
        {
            var layer = new asd.Layer3D();
            AddLayer(layer);

            ObjectFactory.Initialize(5);

            MapData = new MapRawData();
            MapData.LoadMapFile(@"Resources\MapTest.txt");
            foreach (var o in MapData)
            {
                layer.AddObject(o);
            }

            Camera = new asd.CameraObject3D()
            {
                Position = new asd.Vector3DF(MapData.SizeX, 15, MapData.SizeZ),
                Focus = new asd.Vector3DF(10.5f, 0, 10.5f),
                FieldOfView = 70.0f,
                ZNear = 1.0f,
                ZFar = 100.0f,
                WindowSize = new asd.Vector2DI(1280, 720),
            };
            layer.AddObject(Camera);

            var light1 = new asd.DirectionalLightObject3D()
            {
                Rotation = new asd.Vector3DF(70, 0, 0),
            };
            layer.AddObject(light1);

            InitializeSlimes(layer);
        }

        private void InitializeSlimes(asd.Layer3D layer)
        {
            var s1 = ObjectFactory.Create<Slime>(new Vector3DI(5, 3, 5), 1);
            s1.SetInitialPosition(new Vector3DI(5, 3, 5));
            s1.IsMaster = false;
            s1.Slave = null;
            layer.AddObject(s1);

            var s2 = ObjectFactory.Create<Slime>(new Vector3DI(5, 3, 5), 1);
            s2.SetInitialPosition(new Vector3DI(5, 3, 5));
            s2.IsMaster = false;
            s2.Slave = s1;
            layer.AddObject(s2);

            var s3 = ObjectFactory.Create<Slime>(new Vector3DI(5, 3, 5), 1);
            s3.SetInitialPosition(new Vector3DI(5, 3, 5));
            s3.IsMaster = false;
            s3.Slave = s2;
            layer.AddObject(s3);

            Character = ObjectFactory.Create<Slime>(new Vector3DI(5, 3, 5), 1);
            Character.SetInitialPosition(new Vector3DI(5, 3, 5));
            Character.IsMaster = true;
            Character.Slave = s3;
            layer.AddObject(Character);
        }

        protected override void OnUpdating()
        {
            Camera.Focus = new asd.Vector3DF(Character.Position.X, 0, Character.Position.Z);

            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Up) == asd.KeyState.Hold && CameraAngleH < (float)Math.PI / 3.0f) CameraAngleH += 0.01f;
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Down) == asd.KeyState.Hold && CameraAngleH > 0) CameraAngleH -= 0.01f;
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Hold) CameraAngleR -= 0.01f;
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Hold) CameraAngleR += 0.01f;

            Camera.Position = Camera.Focus + 15.0f * new asd.Vector3DF(
                (float)Math.Cos(CameraAngleH) * (float)Math.Cos(CameraAngleR),
                (float)Math.Sin(CameraAngleH),
                (float)Math.Cos(CameraAngleH) * (float)Math.Sin(CameraAngleR)
                );
        }

    }
}
