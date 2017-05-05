using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp.Scene
{
    class Slime : BoxObject
    {
        private const int MoveAnimationLength = 20;
        private const int SlaveMoveDelay = 3;
        private const float JumpPower = 0.00005f * MoveAnimationLength * MoveAnimationLength;
        private int MoveCount = MoveAnimationLength + SlaveMoveDelay + 1;

        public Vector3DI GridPos { get; private set; }
        protected Vector3DI GridPos2 { get; private set; }
        protected Vector3DI GridPosOld { get; private set; }

        public bool IsMaster = false;
        public Slime Slave = null;

        public bool IsCombined { get; protected set; }
        public bool IsChanging { get; protected set; }

        public Slime()
        {

        }

        public void SetInitialPosition(Vector3DI pos)
        {
            Position = pos.ToAsd3DF();
            GridPos = pos;
            GridPos2 = pos;
            GridPosOld = pos;
        }

        protected override void OnUpdate()
        {
            if (MoveCount <= MoveAnimationLength)
            {
                var axis = (GridPos2 - GridPos).ToAsd3DF() / MoveAnimationLength;
                Position = GridPos.ToAsd3DF() + axis * MoveCount + new asd.Vector3DF(0, Parabola(), 0);
                if (IsCombined) Position += new asd.Vector3DF(0.5f, 0.5f, 0.5f);
            }
            else
            {
                GridPos = GridPos2;
                if (IsMaster)
                {
                    if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Up) == asd.KeyState.Hold) Move(0, 1);
                    if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Down) == asd.KeyState.Hold) Move(0, -1);
                    if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Hold) Move(1, 0);
                    if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Hold) Move(-1, 0);
                    if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Space) == asd.KeyState.Push)
                    {
                        if (!IsCombined)
                            StartCombine();
                        else
                            StartDisperse();
                    }
                }
            }

            if (MoveCount == MoveAnimationLength + SlaveMoveDelay)
            {
                if (Slave != null && !IsChanging)
                {
                    Slave.MoveTo(GridPosOld, true);
                }
                IsChanging = false;
            }
            MoveCount++;
            if (IsMaster && IsCombined)
            {
                var c = 1.0f;
                for (var s = Slave; s != null; s = s.Slave) c *= 1.2f;
                Scale = new asd.Vector3DF(c, c, c) / 2;
            }
        }

        private float Parabola()
        {
            const float ys = ((float)MoveAnimationLength * MoveAnimationLength) / 4;
            float x = (MoveCount - (float)MoveAnimationLength / 2);
            return (ys - x * x) * (IsMaster ? JumpPower : JumpPower / 2);
        }

        private void Move(int x, int z)
        {
            var cpos = GridPos + new Vector3DI(x, 0, z);
            MoveTo(cpos);
        }

        private void MoveTo(Vector3DI cpos, bool force = false)
        {
            while (!CheckFilled(cpos)) cpos.Y++;
            while (CheckFilled(cpos - new Vector3DI(0, 1, 0))) cpos.Y--;
            if (cpos.Y - GridPos.Y > 1 && !force) return; // 2段以上は上がれない
            if (Layer.Objects.OfType<Slime>().Any(x => x.GridPos == cpos || x.GridPos2 == cpos) && !force && !IsCombined) return; // すでに小スライムがある場所には行かない
            MoveCount = 0;
            GridPosOld = GridPos;
            GridPos2 = cpos;
        }

        private bool CheckFilled(Vector3DI pos)
        {
            if (!IsCombined)
            {
                return (Layer.Scene as Game).IsFilled(pos.X, pos.Y, pos.Z) == 0;
            }
            else
            {
                for (int y = pos.Y; y <= pos.Y + 1; y++)
                {
                    if ((Layer.Scene as Game).IsFilled(pos.X, y, pos.Z) != 0 ||
                    (Layer.Scene as Game).IsFilled(pos.X + 1, y, pos.Z) != 0 ||
                    (Layer.Scene as Game).IsFilled(pos.X + 1, y, pos.Z + 1) != 0 ||
                    (Layer.Scene as Game).IsFilled(pos.X, y, pos.Z + 1) != 0) return false;
                }
                return true;
            }
        }

        private void StartCombine()
        {
            for (var s = Slave; s != null; s = s.Slave)
            {
                s.MoveTo(GridPos, true);
                s.IsChanging = true;
            }
            IsCombined = true;
        }

        private void StartDisperse()
        {
            IsCombined = false;
            Scale = new asd.Vector3DF(1, 1, 1) / 2;
            MoveTo(GridPos, true);
            IsChanging = true;

            var s = Slave;
            s.MoveTo(GridPos + new Vector3DI(1, 0, 0));
            s.IsDrawn = true;
            s.IsChanging = true;
            s = s.Slave;

            s.MoveTo(GridPos + new Vector3DI(1, 0, 1));
            s.IsDrawn = true;
            s.IsChanging = true;
            s = s.Slave;

            s.MoveTo(GridPos + new Vector3DI(0, 0, 1));
            s.IsChanging = true;
            s.IsDrawn = true;
        }
    }
}
