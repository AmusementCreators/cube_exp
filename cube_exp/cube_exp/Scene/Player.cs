using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp.Scene
{
    class Player : BoxObject
    {

        protected override void OnUpdate()
        {
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Up) == asd.KeyState.Push)
            {
                Position += new asd.Vector3DF(0, 0, 1);
            }
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Down) == asd.KeyState.Push)
            {
                Position += new asd.Vector3DF(0, 0, -1);
            }

            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Push)
            {
                Position += new asd.Vector3DF(-1, 0, 0);
            }
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Push)
            {
                Position += new asd.Vector3DF(1, 0, 0);
            }

            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Space) == asd.KeyState.Push)
            {
                Position += new asd.Vector3DF(0, 1, 0);
            }
            else if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Space) == asd.KeyState.Release)
            {
                Position -= new asd.Vector3DF(0, 1, 0);
            }

        }
    }
}
