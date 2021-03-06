﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace cube_exp
{
    /// <summary>
    /// ファイルから読み込んだ生のマップデータ
    /// </summary>
    class MapRawData
    {
        /// <summary>
        /// マップの幅
        /// </summary>
        public int SizeX { get; private set; }

        /// <summary>
        /// マップの高さ
        /// </summary>
        /// <remarks>最高段+1無いと移動先判定時に落ちる</remarks>
        public int SizeY { get; private set; }

        /// <summary>
        /// マップの奥行
        /// </summary>
        public int SizeZ { get; private set; }

        /// <summary>
        /// マップデータ
        /// </summary>
        /// <remarks>順番に注意。Data[Y][X][Z]</remarks>
        private int[][][] Data;

        /// <summary>
        /// マップファイルを読み込む
        /// </summary>
        public void LoadMapFile(string fileName)
        {
            var file = asd.Engine.File.CreateStaticFile(fileName);
            var raw = Encoding.UTF8.GetString(file.Buffer)
                .Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var size = raw.Take(3).Select(x => int.Parse(x)).ToArray();
            SizeX = size[0];
            SizeY = size[1];
            SizeZ = size[2];

            var field = raw.Skip(3).Select(x => int.Parse(x, NumberStyles.HexNumber)).ToArray();
            Data = new int[SizeY][][];
            int counter = 0;
            for (int y = SizeY - 1; y >= 0; y--)
            {
                Data[y] = new int[SizeX][];
                for (int x = 0; x < SizeX; x++)
                {
                    Data[y][x] = new int[SizeZ];
                    for (int z = 0; z < SizeZ; z++)
                    {
                        Data[y][x][z] = field[counter++];
                    }
                }
            }
        }

        /// <summary>
        /// マップ上のオブジェクトを生成して列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Block> GetEnumerator()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    for (int z = 0; z < SizeZ; z++)
                    {
                        if (Data[y][x][z] != 0)
                        {
                            var obj = ObjectFactory.Create<Block>(new Vector3DI(x, y, z), Data[y][x][z] & 0x0f);
                            obj.Type = Data[y][x][z];
                            switch ((Data[y][x][z] & 0xf0) >> 4)
                            {
                                case 0:
                                    break;
                                case 1:
                                    obj.Component = new Melt();
                                    break;
                            }
                            Data[y][x][z] &= 0x0f;
                            yield return obj;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// (<see cref="x"/>, <see cref="y"/>, <see cref="z"/>)のデータを返す
        /// </summary>
        public int GetData(int x, int y, int z)
        {
            return Data[y][x][z];
        }

        /// <summary>
        /// (<see cref="x"/>, <see cref="y"/>, <see cref="z"/>)のデータを返す
        /// </summary>
        public int GetData(Vector3DI gpos)
        {
            return Data[gpos.Y][gpos.X][gpos.Z];
        }

        /// <summary>
        /// <see cref="gpos"/>のデータを更新する
        /// </summary>
        public void UpdateData(int x, int y, int z, int value)
        {
            Data[y][x][z] = value;
        }

        /// <summary>
        /// <see cref="gpos"/>のデータを更新する
        /// </summary>
        public void UpdateData(Vector3DI gpos, int value)
        {
            Data[gpos.Y][gpos.X][gpos.Z] = value;
        }

        /// <summary>
        /// (<see cref="x"/>, <see cref="y"/>, <see cref="z"/>)がマップの範囲内かどうかをチェックする
        /// </summary>
        public bool IsInvalidPosition(int x, int y, int z)
        {
            return (x >= 0 && y >= 0 && z >= 0 &&
                x < SizeX && y < SizeY && z < SizeZ);
        }
    }
}
