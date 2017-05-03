using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp.Scene
{
    class Player : BoxObject
    {
        private const int MoveAnimationLength = 20;
        private const float JumpPower = 0.00005f * MoveAnimationLength * MoveAnimationLength;
        private int MoveCount = MoveAnimationLength;

        public Vector3DI GridPos { get; private set; }
        private Vector3DI GridPos2;


        protected override void OnUpdate()
        {

            if (MoveCount <= MoveAnimationLength)
            {
                var axis = (GridPos2 - GridPos).ToAsd3DF() / MoveAnimationLength;

                Position = GridPos.ToAsd3DF() + axis * MoveCount + new asd.Vector3DF(0, Parabola(), 0);
                MoveCount++;
            }
            else
            {
                GridPos = GridPos2;
                Console.WriteLine($"{Position.X:F2} {Position.Y:F2} {Position.Z:F2}");

                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Up) == asd.KeyState.Hold) TryMove(0, 1);
                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Down) == asd.KeyState.Hold) TryMove(0, -1);
                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Hold) TryMove(1, 0);
                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Hold) TryMove(-1, 0);
            }
        }

        private float Parabola()
        {
            const float ys = ((float)MoveAnimationLength * MoveAnimationLength) / 4;
            float x = (MoveCount - (float)MoveAnimationLength / 2);
            return JumpPower * (ys - x * x);
        }

        private void TryMove(int x, int z)
        {
            var cpos = GridPos + new Vector3DI(x, 0, z);
            while ((Layer.Scene as Game).IsFilled(cpos.X, cpos.Y, cpos.Z) != 0) cpos.Y++;
            while ((Layer.Scene as Game).IsFilled(cpos.X, cpos.Y - 1, cpos.Z) == 0) cpos.Y--;
            if (cpos.Y - GridPos.Y > 1) return; // 2段以上は上がれない
            GridPos2 = cpos;
            MoveCount = 0;
        }
    }
}
