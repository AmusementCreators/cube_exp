using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    class GameScene : asd.Scene
    {
        private MapRawData MapData;
        private Slime[] Slimes;

        public uint Counter { get; private set; } = 0;

        public asd.CameraObject3D Camera { get; private set; }
        public float CameraAngleH { get; private set; } = asd.MathHelper.DegreeToRadian(45);
        public float CameraAngleR { get; private set; } = asd.MathHelper.DegreeToRadian(45);
        private float CameraAngleHTarget = asd.MathHelper.DegreeToRadian(45);
        private float CameraAngleRTarget = asd.MathHelper.DegreeToRadian(45);

        public Vector3DI GetSlimePos()
        {
            return Slimes[0].GridPos;
        }

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
            Slimes = new Slime[4];

            Slimes[3] = ObjectFactory.Create<Slime>(new Vector3DI(), 1);
            layer.AddObject(Slimes[3]);
            Slimes[2] = ObjectFactory.Create<Slime>(new Vector3DI(), 1);
            layer.AddObject(Slimes[2]);
            Slimes[1] = ObjectFactory.Create<Slime>(new Vector3DI(), 1);
            layer.AddObject(Slimes[1]);
            Slimes[0] = ObjectFactory.Create<Slime>(new Vector3DI(), 1);
            layer.AddObject(Slimes[0]);


            Slimes[3].SetInitialPosition(new Vector3DI(5, 1, 5));
            Slimes[3].IsMain = false;
            Slimes[3].Follow = Slimes[2];
            Slimes[3].Follower = null;

            Slimes[2].SetInitialPosition(new Vector3DI(5, 1, 5));
            Slimes[2].IsMain = false;
            Slimes[2].Follow = Slimes[1];
            Slimes[2].Follower = Slimes[3];

            Slimes[1].SetInitialPosition(new Vector3DI(5, 1, 5));
            Slimes[1].IsMain = false;
            Slimes[1].Follow = Slimes[0];
            Slimes[1].Follower = Slimes[2];

            Slimes[0].SetInitialPosition(new Vector3DI(5, 1, 5));
            Slimes[0].IsMain = true;
            Slimes[0].Follow = null;
            Slimes[0].Follower = Slimes[1];
        }

        protected override void OnUpdating()
        {
            Camera.Focus = new asd.Vector3DF(Slimes[0].Position.X, 0, Slimes[0].Position.Z);

            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Up) == asd.KeyState.Push &&
                CameraAngleHTarget < asd.MathHelper.DegreeToRadian(60)) CameraAngleHTarget += asd.MathHelper.DegreeToRadian(15f);
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Down) == asd.KeyState.Push &&
                CameraAngleHTarget > 0) CameraAngleHTarget -= asd.MathHelper.DegreeToRadian(15f);
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Push) CameraAngleRTarget -= asd.MathHelper.DegreeToRadian(45f);
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Push) CameraAngleRTarget += asd.MathHelper.DegreeToRadian(45f);

            CameraAngleR += (CameraAngleRTarget - CameraAngleR) / 10.0f;
            CameraAngleH += (CameraAngleHTarget - CameraAngleH) / 10.0f;

            Camera.Position = Camera.Focus + 15.0f * new asd.Vector3DF(
                (float)Math.Cos(CameraAngleH) * (float)Math.Cos(CameraAngleR),
                (float)Math.Sin(CameraAngleH),
                (float)Math.Cos(CameraAngleH) * (float)Math.Sin(CameraAngleR)
                );

            Counter++;
        }

    }
}
