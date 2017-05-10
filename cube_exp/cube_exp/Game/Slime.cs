using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace cube_exp
{
    /// <summary>
    /// スライム
    /// </summary>
    class Slime : BaseObject
    {
        /// <summary>
        /// 1マス移動するモーションのフレーム数
        /// </summary>
        private const uint MoveAnimationLength = 20;

        /// <summary>
        /// ジャンプ高さ計算時に用いる係数
        /// </summary>
        private const float JumpPower = 1f / 40000f * MoveAnimationLength * MoveAnimationLength;

        /// <summary>
        /// 経過フレーム数
        /// </summary>
        private uint MoveCount { get { return (Layer.Scene as GameScene).Counter; } }

        /// <summary>
        /// 移動開始からの経過時間
        /// </summary>
        public uint LastMoveCount { get; private set; } = 0;
        
        /// <summary>
        /// 次に移動する先の座標
        /// </summary>
        protected Vector3DI GridPos2 { get; private set; }

        /// <summary>
        /// 1つ前の座標
        /// </summary>
        protected Vector3DI GridPosOld { get; private set; }

        /// <summary>
        /// メインのスライム（操作するやつ）かどうか
        /// </summary>
        public bool IsMain { get; set; } = false;

        /// <summary>
        /// 一つ前のスライム
        /// </summary>
        public Slime Follow { get; set; } = null;

        /// <summary>
        /// 後続のスライム
        /// </summary>
        public Slime Follower { get; set; } = null;


        /// <summary>
        /// 合体状態であるかどうか
        /// </summary>
        public bool IsCombined { get; protected set; }

        /// <summary>
        /// 合体・分裂中であるかどうか
        /// </summary>
        public bool IsChanging { get; protected set; }


        /// <summary>
        /// 現在のテクスチャ番号
        /// </summary>
        public int CurrentType { get; private set; } = 1;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Slime()
        {

        }

        /// <summary>
        /// 初期座標をセットする
        /// </summary>
        /// <remarks>各種座標系プロパティを一気に上書きする</remarks>
        public void SetInitialPosition(Vector3DI pos)
        {
            Position = pos.ToAsd3DF();
            GridPos = pos;
            GridPos2 = pos;
            GridPosOld = pos;
        }



        protected override void OnUpdate()
        {
            if (MoveCount <= LastMoveCount + MoveAnimationLength)//移動中
            {
                var prg = (float)(MoveCount - LastMoveCount) / MoveAnimationLength;
                var axis = (GridPos2 - GridPos).ToAsd3DF();
                Position = GridPos.ToAsd3DF() + axis * prg + new asd.Vector3DF(0, Parabola(), 0);
                if (IsCombined) Position += new asd.Vector3DF(0.5f, 0.5f, 0.5f) * (IsChanging ? prg : 1.0f);
            }
            else //移動完了
            {
                GridPos = GridPos2;
                if (IsMain) Control();
                else if (MoveCount > Follow.LastMoveCount + MoveAnimationLength &&
                    GridPos != Follow.GridPosOld && !IsChanging) MoveTo(Follow.GridPosOld, true);
                IsChanging = false;

            }

            //合体すると大きくなる
            if (IsMain && IsCombined && !IsChanging)
            {
                var c = 1.0f;
                for (var s = Follower; s != null; s = s.Follower)
                {
                    s.IsDrawn = false;
                    c *= 1.2f;
                }
                Scale = new asd.Vector3DF(c, c, c) / 2;
            }

            if(!IsMain && !IsChanging)
            {
                var m = this;
                while (m.Follow != null) m = m.Follow;
                if (m.IsCombined)
                {
                    Position = new asd.Vector3DF();
                }
            }
        }

        /// <summary>
        /// スライムの操作を受け付ける
        /// </summary>
        private void Control()
        {
            //角度計算
            var angle = (int)((Layer.Scene as GameScene).CameraAngleR / (float)Math.PI * 180.0 + 180 + 22.5);
            while (angle > 360) angle -= 360;
            while (angle < 0) angle += 360;
            angle /= 45;

            //角度→移動方向のテーブル
            int[][] dir = new[] {
                new[] { 1,0 },
                new[] { 1,1 },
                new[] { 0,1 },
                new[] { -1,1 },
                new[] { -1,0 },
                new[] { -1,-1 },
                new[] { 0,-1 },
                new[] { 1,-1 },
            };

            //移動
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.W) == asd.KeyState.Hold) Move(dir[angle][0], dir[angle][1]);
            else if (asd.Engine.Keyboard.GetKeyState(asd.Keys.S) == asd.KeyState.Hold) Move(dir[(angle + 4) % 8][0], dir[(angle + 4) % 8][1]);
            else if (asd.Engine.Keyboard.GetKeyState(asd.Keys.A) == asd.KeyState.Hold) Move(dir[(angle + 6) % 8][0], dir[(angle + 6) % 8][1]);
            else if (asd.Engine.Keyboard.GetKeyState(asd.Keys.D) == asd.KeyState.Hold) Move(dir[(angle + 2) % 8][0], dir[(angle + 2) % 8][1]);

            //合体・分裂
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Space) == asd.KeyState.Push)
            {
                if (!IsCombined)
                    StartCombine();
                else
                    StartDisperse();
            }

            //色の変更
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.LeftShift) == asd.KeyState.Push)
            {
                CurrentType = (CurrentType + 1) % 5 + 1;
                for (var s = this; s != null; s = s.Follower)
                {
                    s.CurrentType = CurrentType;
                    ObjectFactory.UpdateModel(s, CurrentType);
                }
            }
        }

        /// <summary>
        /// ジャンプ中の高さを生成する
        /// </summary>
        private float Parabola()
        {
            const float ys = ((float)MoveAnimationLength * MoveAnimationLength) / 4;
            float x = ((MoveCount - LastMoveCount) - (float)MoveAnimationLength / 2);
            return (ys - x * x) * (IsMain ? JumpPower : JumpPower / 2);
        }

        /// <summary>
        /// (<see cref="x"/>, 0, <see cref="z"/>) 移動する
        /// </summary>
        /// <remarks>上り下りは内部で処理する</remarks>
        private void Move(int x, int z)
        {
            var cpos = GridPos + new Vector3DI(x, 0, z);
            MoveTo(cpos);
        }

        /// <summary>
        /// <see cref="cpos"/>「に」 移動する
        /// </summary>
        /// <param name="cpos">移動先</param>
        /// <param name="force">一部条件を無視して移動するかどうか</param>
        private void MoveTo(Vector3DI cpos, bool force = false)
        {
            while (!CheckFilled(cpos)) cpos.Y++;
            while (CheckFilled(cpos - new Vector3DI(0, 1, 0))) cpos.Y--;
            if (cpos.Y - GridPos.Y > 1 && !force) return; // 2段以上は上がれない
            if (Layer.Objects.OfType<Slime>().Any(x => x.GridPos == cpos || x.GridPos2 == cpos) && !force && !IsCombined) return; // すでに小スライムがある場所には行かない
            if (!CheckFloorType(cpos)) return;

            GridPosOld = GridPos;
            GridPos2 = cpos;
            LastMoveCount = MoveCount;
        }

        /// <summary>
        /// マップデータの取得
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private int _BlockCode(Vector3DI pos, int offsetX = 0, int offsetY = 0, int offsetZ = 0)
        {
            return (Layer.Scene as GameScene).GetMapData(pos.X + offsetX, pos.Y + offsetY, pos.Z + offsetZ);
        }

        /// <summary>
        /// <see cref="pos"/>周辺に空きがあるかどうかを調べる
        /// </summary>
        private bool CheckFilled(Vector3DI pos)
        {
            if (!IsCombined)
            {
                return _BlockCode(pos) == 0;
            }
            else
            {
                for (int y = 0; y <= 1; y++)
                {
                    if (_BlockCode(pos, 0, y, 0) != 0) return false;
                    if (_BlockCode(pos, 1, y, 0) != 0) return false;
                    if (_BlockCode(pos, 1, y, 1) != 0) return false;
                    if (_BlockCode(pos, 0, y, 1) != 0) return false;
                }
                return true;
            }
        }

        /// <summary>
        /// <see cref="pos"/>周辺の床の種類がスライムと一致するかどうかを調べる
        /// </summary>
        private bool CheckFloorType(Vector3DI pos)
        {
            if (!IsCombined)
            {
                return _BlockCode(pos, 0, -1, 0) == CurrentType;
            }
            else
            {
                if (_BlockCode(pos, 0, -1, 0) != CurrentType) return false;
                if (_BlockCode(pos, 1, -1, 0) != CurrentType) return false;
                if (_BlockCode(pos, 1, -1, 1) != CurrentType) return false;
                if (_BlockCode(pos, 0, -1, 1) != CurrentType) return false;
                return true;
            }
        }

        /// <summary>
        /// 合体を開始する
        /// </summary>
        private void StartCombine()
        {
            for (var s = this; s != null; s = s.Follower)
            {
                s.MoveTo(GridPos, true);
                s.IsChanging = true;
            }
            IsCombined = true;
        }

        /// <summary>
        /// 分裂を開始する
        /// </summary>
        private void StartDisperse()
        {
            IsCombined = false;
            Scale = new asd.Vector3DF(1, 1, 1) / 2;
            MoveTo(GridPos, true);
            IsChanging = true;

            var s = Follower;
            s.SetInitialPosition(GridPos + new Vector3DI(1, 0, 0));
            s.MoveTo(GridPos + new Vector3DI(1, 0, 0));
            s.IsDrawn = true;
            s.IsChanging = true;
            s = s.Follower;

            s.SetInitialPosition(GridPos + new Vector3DI(1, 0, 1));
            s.MoveTo(GridPos + new Vector3DI(1, 0, 1));
            s.IsDrawn = true;
            s.IsChanging = true;
            s = s.Follower;

            s.SetInitialPosition(GridPos + new Vector3DI(1, 0, 1));
            s.MoveTo(GridPos + new Vector3DI(0, 0, 1));
            s.IsChanging = true;
            s.IsDrawn = true;
        }
    }
}
