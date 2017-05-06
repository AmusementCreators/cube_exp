using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace cube_exp.Scene
{
    class Slime : BoxObject
    {
        private const uint MoveAnimationLength = 20;
        private const uint SlaveMoveDelay = 10;
        private const float JumpPower = 0.00005f * MoveAnimationLength * MoveAnimationLength;
        private uint MoveCount = MoveAnimationLength + 1;

        public Vector3DI GridPos { get; private set; }
        protected Vector3DI GridPos2 { get; private set; }
        protected Vector3DI GridPosOld { get; private set; }

        public bool IsMaster = false;
        public Slime Slave = null;

        public bool IsCombined { get; protected set; }
        public bool IsChanging { get; protected set; }

        public int CurrentType = 1;

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
                if (IsMaster) Control();
            }

            if (Slave != null && MoveCount == MoveAnimationLength + SlaveMoveDelay && Slave.MoveCount >= MoveAnimationLength)
            {
                if (!IsChanging)
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

        private void Control()
        {
            var angle = (int)((Layer.Scene as Game).CameraAngleR / (float)Math.PI * 180.0 + 180 + 22.5);
            while (angle > 360) angle -= 360;
            while (angle < 0) angle += 360;
            angle /= 45;

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

            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.W) == asd.KeyState.Hold) Move(dir[angle][0], dir[angle][1]);
            else if (asd.Engine.Keyboard.GetKeyState(asd.Keys.S) == asd.KeyState.Hold) Move(dir[(angle + 4) % 8][0], dir[(angle + 4) % 8][1]);
            else if (asd.Engine.Keyboard.GetKeyState(asd.Keys.A) == asd.KeyState.Hold) Move(dir[(angle + 6) % 8][0], dir[(angle + 6) % 8][1]);
            else if (asd.Engine.Keyboard.GetKeyState(asd.Keys.D) == asd.KeyState.Hold) Move(dir[(angle + 2) % 8][0], dir[(angle + 2) % 8][1]);

            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Space) == asd.KeyState.Push)
            {
                if (!IsCombined)
                    StartCombine();
                else
                    StartDisperse();
            }

            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.LeftShift) == asd.KeyState.Push)
            {
                for (var s = this; s != null; s = s.Slave)
                {
                    CurrentType = (CurrentType + 1) % 5 + 1;
                    BoxObjectFactory.UpdateModel(s, CurrentType);
                }
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
            if (!CheckFloorType(cpos)) return;

            MoveCount = 0;
            GridPosOld = GridPos;
            GridPos2 = cpos;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private int _BlockCode(Vector3DI pos, int offsetX = 0, int offsetY = 0, int offsetZ = 0)
        {
            return (Layer.Scene as Game).IsFilled(pos.X + offsetX, pos.Y + offsetY, pos.Z + offsetZ);
        }

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
