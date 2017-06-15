using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class SellPriceResult
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        /// 客户购买价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 京东价格
        /// </summary>
        public decimal jdPrice { get; set; }
    }
}
