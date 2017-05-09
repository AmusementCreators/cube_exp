using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    /// <summary>
    /// オブジェクトの抽象クラス
    /// </summary>
    abstract class BaseObject : asd.ModelObject3D
    {
        /// <summary>
        /// グリッド座標
        /// </summary>
        public Vector3DI GridPos { get; protected set; }
    }
}
