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

        public asd.Vector3DF Velocity { get; private set; } = new asd.Vector3DF();
        public asd.Vector2DI GridPos { get; private set; }
        private asd.Vector2DI GridPos2;


        protected override void OnUpdate()
        {

            if (MoveCount <= MoveAnimationLength)
            {
                var axis = (Helper.Vector2DITo3DFXZ(GridPos2) - Helper.Vector2DITo3DFXZ(GridPos)) / MoveAnimationLength;

                Position = Helper.Vector2DITo3DFXZ(GridPos) + axis * MoveCount + new asd.Vector3DF(0, Parabola(), 0);
                MoveCount++;
            }
            else
            {
                GridPos = GridPos2;
                Console.WriteLine($"{Position.X:F2} {Position.Y:F2} {Position.Z:F2}");

                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Up) == asd.KeyState.Hold)
                {
                    MoveCount = 0;
                    GridPos2 = GridPos + new asd.Vector2DI(0, 1);
                }
                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Down) == asd.KeyState.Hold)
                {
                    MoveCount = 0;
                    GridPos2 = GridPos + new asd.Vector2DI(0, -1);
                }
                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Hold)
                {
                    MoveCount = 0;
                    GridPos2 = GridPos + new asd.Vector2DI(1, 0);
                }
                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Hold)
                {
                    MoveCount = 0;
                    GridPos2 = GridPos + new asd.Vector2DI(-1, 0);
                }
            }
        }

        private float Parabola()
        {
            const float ys = ((float)MoveAnimationLength * MoveAnimationLength) / 4;
            float x = (MoveCount - (float)MoveAnimationLength / 2);
            return JumpPower * (ys - x * x);
        }
    }
}
