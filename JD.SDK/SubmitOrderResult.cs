using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class SubmitOrderResult
    {
        /// <summary>
        /// 京东订单号(可以理解为父单号，拆单后，会出现子单号; 不拆单的，为个即是父单号也是子单号)
        /// </summary>
        public string jdOrderId { get; set; }
        /// <summary>
        /// 总运费; 这个是订单总运费 = 基础运费 + 总的超重偏远附加运费
        /// </summary>
        public decimal freight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SkuReturnObject> sku { get; set; }
        /// <summary>
        /// 商品总价格
        /// </summary>
        public decimal orderPrice { get; set; }
        /// <summary>
        /// 订单裸价
        /// </summary>
        public decimal orderNakedPrice { get; set; }
        /// <summary>
        /// 订单税额
        /// </summary>
        public decimal orderTaxPrice { get; set; }
    }



    


}
