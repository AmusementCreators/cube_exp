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
        public asd.CameraObject3D Camera { get; private set; }
        public Slime Character { get; private set; }
        public uint Counter { get; private set; } = 0;


        public float CameraAngleH { get; private set; } = asd.MathHelper.DegreeToRadian(45);
        public float CameraAngleR { get; private set; } = asd.MathHelper.DegreeToRadian(45);
        private float CameraAngleHTarget = asd.MathHelper.DegreeToRadian(45);
        private float CameraAngleRTarget = asd.MathHelper.DegreeToRadian(45);


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
            var s1 = ObjectFactory.Create<Slime>(new Vector3DI(), 1);
            layer.AddObject(s1);
            var s2 = ObjectFactory.Create<Slime>(new Vector3DI(), 1);
            layer.AddObject(s2);
            var s3 = ObjectFactory.Create<Slime>(new Vector3DI(), 1);
            layer.AddObject(s3);
            Character = ObjectFactory.Create<Slime>(new Vector3DI(), 1);
            layer.AddObject(Character);


            s1.SetInitialPosition(new Vector3DI(5, 1, 5));
            s1.IsMain = false;
            s1.Follow = s2;
            s1.Follower = null;

            s2.SetInitialPosition(new Vector3DI(5, 1, 5));
            s2.IsMain = false;
            s2.Follow = s3;
            s2.Follower = s1;

            s3.SetInitialPosition(new Vector3DI(5, 1, 5));
            s3.IsMain = false;
            s3.Follow = Character;
            s3.Follower = s2;

            Character.SetInitialPosition(new Vector3DI(5, 1, 5));
            Character.IsMain = true;
            Character.Follow = null;
            Character.Follower = s3;
        }

        protected override void OnUpdating()
        {
            Camera.Focus = new asd.Vector3DF(Character.Position.X, 0, Character.Position.Z);

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
