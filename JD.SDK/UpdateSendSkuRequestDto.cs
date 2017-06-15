using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class UpdateSendSkuRequestDto
    {
        /// <summary>
        /// 必需 服务单号
        /// </summary>
        public int afsServiceId { get; set; }
        /// <summary>
        /// 必需 运费
        /// </summary>
        public decimal freightMoney { get; set; }
        /// <summary>
        /// 必需，发运公司: 圆通快递、申通快递、韵达快递、中通快递、宅急送、EMS、顺丰快递
        /// </summary>
        public string expressCompany { get; set; }
        /// <summary>
        /// 必需，发货日期 格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string deliverDate { get; set; }
        /// <summary>
        /// 必需，货运单号 不超过50
        /// </summary>
        public string expressCode { get; set; }
    }
}
