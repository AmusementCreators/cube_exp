using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    class Melt : BlockComponent
    {
        private int Count = 0;

        public override void OnUpdate()
        {
            var dist = ((Owner.Layer.Scene as GameScene).GetSlimePos() - Vector3DI.FromAsd3DF(Owner.Position)).GetManhattanDistance();
            if (dist > 1)
            {
                Count = 0;
                return;
            }
            Count++;
            if (Count > 60)
            {
                Owner.Layer.RemoveObject(Owner);
            }

        }
    }
}
