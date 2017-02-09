using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!asd.Engine.Initialize("cube [experimental]", 1280, 720, new asd.EngineOption()))return;

            asd.Engine.ChangeScene(new Scene.Game());

            while(asd.Engine.DoEvents())
            {
                asd.Engine.Update();
            }

            asd.Engine.Terminate();
        }
    }
}
