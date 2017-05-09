using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube_exp
{
    /// <summary>
    /// フィールド上のブロック
    /// </summary>
    class Block : BaseObject
    {
        /// <summary>
        /// 種類
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// コンポーネント
        /// </summary>
        public BlockComponent Component { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Block()
        {
            
        }

        protected override void OnUpdate()
        {
            if (Component != null)
            {
                Component.Owner = this;
                Component.OnUpdate();
            }
        }
    }
}
