using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class SkuReturnObject
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public int category { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        public decimal tax { get; set; }

        public decimal taxPrice { get; set; }

        public decimal nakedPrice { get; set; }
        /// <summary>
        /// 0普通、1附件、2赠品
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// oid为主商品skuid，如果本身是主商品，则oid为0
        /// </summary>
        public long oid { get; set; }

    }
}
