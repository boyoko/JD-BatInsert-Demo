using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class SaleAttr
    {
        /// <summary>
        /// 标签图片地址
        /// </summary>
        public string imagePath { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string saleValue { get; set; }
        /// <summary>
        /// 当前标签下的同类商品skuId
        /// </summary>
        public List<long> skuIds { get; set; }
    }
}
