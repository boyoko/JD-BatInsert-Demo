using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class OrderData
    {
        /// <summary>
        /// 京东订单编号
        /// </summary>
        public string jdOrderId { get; set; }
        /// <summary>
        /// 订单状态 0 是新建  1是妥投   2是拒收
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 是否挂起   0为为挂起    1为挂起
        /// </summary>
        public bool hangUpState { get; set; }
        /// <summary>
        /// 开票方式(1为随货开票，0为订单预借，2为集中开票 )
        /// </summary>
        public int invoiceState { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal orderPrice { get; set; }
    }
}
