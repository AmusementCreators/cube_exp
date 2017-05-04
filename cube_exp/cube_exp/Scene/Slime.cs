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

        public Vector3DI GridPos { get; set; }
        private Vector3DI GridPos2, GridPosOld;

        public bool IsMaster = false;
        public Slime Slave = null;

        public Slime(Vector3DI pos, bool isMaster = false, Slime slave = null)
        {
            Position = pos.ToAsd3DF();
            GridPos = pos;
            GridPos2 = pos;
            IsMaster = isMaster;
            Slave = slave;
        }

        protected override void OnUpdate()
        {
            if (MoveCount <= MoveAnimationLength)
            {
                var axis = (GridPos2 - GridPos).ToAsd3DF() / MoveAnimationLength;
                Position = GridPos.ToAsd3DF() + axis * MoveCount + new asd.Vector3DF(0, Parabola(), 0);
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
                }
            }

            if (MoveCount == MoveAnimationLength + SlaveMoveDelay)
            {
                if (Slave != null) Slave.MoveTo(GridPosOld);
            }
            MoveCount++;
            Console.WriteLine($"{IsMaster} {GridPos.X} {GridPos.Y} {GridPos.Z}");
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
            while ((Layer.Scene as Game).IsFilled(cpos.X, cpos.Y, cpos.Z) != 0) cpos.Y++;
            while ((Layer.Scene as Game).IsFilled(cpos.X, cpos.Y - 1, cpos.Z) == 0) cpos.Y--;
            if (cpos.Y - GridPos.Y > 1 && !force) return; // 2段以上は上がれない
            MoveCount = 0;
            GridPosOld = GridPos;
            GridPos2 = cpos;
        }


    }
}
