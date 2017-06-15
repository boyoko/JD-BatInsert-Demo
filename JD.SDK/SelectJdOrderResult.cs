using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class SelectJdOrderResult
    {
        /// <summary>
        /// 京东订单编号
        /// </summary>
        public string jdOrderId { get; set; }
        /// <summary>
        /// 物流状态 0 是新建  1是妥投   2是拒收
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 订单类型   1是父订单   2是子订单
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 订单价格
        /// </summary>
        public decimal orderPrice { get; set; }
        /// <summary>
        /// 运费（合同配置了才返回）
        /// </summary>
        public decimal freight { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<SkuReturnObject> sku { get; set; }
        /// <summary>
        /// 父订单号
        /// </summary>
        public string pOrder { get; set; }
        /// <summary>
        /// 订单状态  0为取消订单  1为有效
        /// </summary>
        public int orderState { get; set; }
        /// <summary>
        /// 0为未确认下单订单   1为确认下单订单
        /// </summary>
        public int submitState { get; set; }

    }
}
