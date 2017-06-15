using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class ServiceExpressInfoDto
    {
        /// <summary>
        /// 服务单号
        /// </summary>
        public int afsServiceId { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public string freightMoney { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string expressCompany { get; set; }
        /// <summary>
        /// 客户发货日期  格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string deliverDate { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string expressCode { get; set; }
    }
}
