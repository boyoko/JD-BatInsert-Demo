using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class CheckAreaLimitResult
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        /// 代表区域受限 false 区域不受限
        /// </summary>
        public bool isAreaRestrict { get; set; }
    }
}
