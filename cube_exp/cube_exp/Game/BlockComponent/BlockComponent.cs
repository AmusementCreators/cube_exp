using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    /// <summary>
    /// ブロックの挙動を定義する抽象クラス
    /// </summary>
    abstract class BlockComponent
    {
        public Block Owner { get; set; }

        /// <summary>
        /// Update時に呼び出すメソッド
        /// </summary>
        public abstract void OnUpdate();
    }
}
