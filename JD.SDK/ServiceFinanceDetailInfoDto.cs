using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class ServiceFinanceDetailInfoDto
    {
        /// <summary>
        /// 退款方式
        /// </summary>
        public int refundWay { get; set; }
        /// <summary>
        /// 退款方式名称
        /// </summary>
        public string refundWayName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string statusName { get; set;}
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal refundPrice { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string wareName { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int wareId { get; set; }

    }
}
