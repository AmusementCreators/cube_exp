using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp.Scene
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
                .Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x)).ToArray();

            SizeX = raw[0];
            SizeY = raw[1];
            SizeZ = raw[2];

            Data = new int[SizeY][][];
            int counter = 3;
            for (int y = SizeY - 1; y >= 0; y--)
            {
                Data[y] = new int[SizeX][];
                for (int x = 0; x < SizeX; x++)
                {
                    Data[y][x] = new int[SizeZ];
                    for (int z = 0; z < SizeZ; z++)
                    {
                        Data[y][x][z] = raw[counter++];
                    }
                }
            }
        }

        /// <summary>
        /// マップ上のオブジェクトを生成して列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerator<asd.ModelObject3D> GetEnumerator()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    for (int z = 0; z < SizeZ; z++)
                    {
                        if (Data[y][x][z] != 0)
                        {
                            yield return ObjectFactory.Create<asd.ModelObject3D>(new Vector3DI(x, y, z), Data[y][x][z]);
                        }
                    }
                }
            }
        }


        public int GetData(int x, int y, int z)
        {
            return Data[y][x][z];
        }
    }
}
