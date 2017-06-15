using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class OrderTrackResult
    {
        /// <summary>
        /// 京东订单号
        /// </summary>
        public string jdOrderId { get; set; }
        /// <summary>
        /// 配送信息
        /// </summary>
        public List<OrderTrackObject> orderTrack { get; set; }
    }
}
