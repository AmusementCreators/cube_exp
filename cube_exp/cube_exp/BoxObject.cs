using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    class BoxObject : asd.ModelObject3D
    {
        static asd.Model Model = null;

        protected override void OnAdded()
        {
            if (Model == null) Model = asd.Engine.Graphics.CreateModel("Box.mdl");
            SetModel(Model);
        }
    }
}
