using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class GiftAttr
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        /// 赠品数量
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 1附件 2赠品
        /// </summary>
        public int giftType { get; set; }
    }
}
