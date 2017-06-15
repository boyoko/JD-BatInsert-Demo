using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class CheckNewOrderResult
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public List<OrderData> orders { get; set; }
        /// <summary>
        /// 订单总数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 总页码数
        /// </summary>
        public int totalPage { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int curPage { get; set; }
    }
}
