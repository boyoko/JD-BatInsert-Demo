using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class CheckResult
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        ///  是否可售，1：是，0：否
        /// </summary>
        public int saleState { get; set; }
        /// <summary>
        /// 是否可开增票，1：支持，0：不支持
        /// </summary>
        public int isCanVAT { get; set; }
        /// <summary>
        /// 是否支持7天无理由退货，1：是，0：否
        /// </summary>
        public int is7ToReturn { get; set; }
    }
}
