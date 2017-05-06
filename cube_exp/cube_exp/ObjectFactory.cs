using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    /// <summary>
    /// モデルオブジェクトを生成する静的クラス
    /// </summary>
    static class ObjectFactory
    {
        /// <summary>
        /// テクスチャごとのモデルデータ
        /// </summary>
        private static asd.Model[] Models;

        /// <summary>
        /// モデルデータとテクスチャ画像を読み込む
        /// </summary>
        /// <param name="length">テクスチャの枚数</param>
        public static void Initialize(int length)
        {
            Models = new asd.Model[length + 1];
            for (int i = 1; i <= length; i++)
            {
                Models[i] = asd.Engine.Graphics.CreateModel(@"Resources\Box" + i + ".mdl");
                var texture = asd.Engine.Graphics.CreateTexture2D($@"Resources\{i}.png");
                Models[i].GetMesh(0).SetColorTexture(0, texture);
            }
        }

        /// <summary>
        /// モデルオブジェクトを生成する
        /// </summary>
        /// <param name="pos">座標</param>
        /// <param name="textureID">テクスチャ番号</param>
        /// <returns>モデルオブジェクト</returns>
        /// <remarks><see cref="textureID"/> = 0 は空白なので使用しない</remarks>
        public static T Create<T>(Vector3DI pos, int textureID) where T : asd.ModelObject3D, new()
        {
            if (textureID == 0) return null;
            var obj = new T();
            obj.Position = pos.ToAsd3DF();
            UpdateModel(obj, textureID);
            return obj;
        }

        /// <summary>
        /// モデルオブジェクトのテクスチャを変更する
        /// </summary>
        /// <param name="obj">モデルオブジェクト</param>
        /// <param name="textureID">テクスチャ番号</param>
        /// <remarks><see cref="textureID"/> = 0 は空白なので使用しない</remarks>
        public static void UpdateModel<T>(T obj, int textureID) where T : asd.ModelObject3D, new()
        {
            if (textureID == 0) return;
            obj.SetModel(Models[textureID]);
            obj.Scale = new asd.Vector3DF(0.5f, 0.5f, 0.5f);
        }
    }
}
