﻿using System;
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

        private MapRawData MapData;

        public int IsFilled(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0) return 0;
            if (x >= MapData.SizeX || y >= MapData.SizeY || z >= MapData.SizeZ) return 0;
            return MapData.Data[y][x][z];
        }

        protected override void OnRegistered()
        {
            var layer = new asd.Layer3D();
            AddLayer(layer);

            BoxObjectFactory.Initialize(new[] { "", "", "Fire.png" });

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
                WindowSize = new asd.Vector2DI(800, 600),
            };
            layer.AddObject(Camera);

            var light1 = new asd.DirectionalLightObject3D()
            {
                Rotation = new asd.Vector3DF(10, 50, 10),
            };
            layer.AddObject(light1);

            var s1 = BoxObjectFactory.Create<Slime>(new Vector3DI(5, 3, 5), 0);
            s1.SetInitialPosition(new Vector3DI(5, 3, 5));
            s1.IsMaster = false;
            s1.Slave = null;
            layer.AddObject(s1);

            var s2 = BoxObjectFactory.Create<Slime>(new Vector3DI(5, 3, 5), 0);
            s2.SetInitialPosition(new Vector3DI(5, 3, 5));
            s2.IsMaster = false;
            s2.Slave = s1;
            layer.AddObject(s2);

            var s3 = BoxObjectFactory.Create<Slime>(new Vector3DI(5, 3, 5), 0);
            s3.SetInitialPosition(new Vector3DI(5, 3, 5));
            s3.IsMaster = false;
            s3.Slave = s2;
            layer.AddObject(s3);

            Character = BoxObjectFactory.Create<Slime>(new Vector3DI(5, 3, 5), 0);
            Character.SetInitialPosition(new Vector3DI(5, 3, 5));
            Character.IsMaster = true;
            Character.Slave = s3;
            layer.AddObject(Character);
        }

        protected override void OnUpdating()
        {
            Camera.Focus = new asd.Vector3DF(Character.Position.X, 0, Character.Position.Z);
        }
    }
}
