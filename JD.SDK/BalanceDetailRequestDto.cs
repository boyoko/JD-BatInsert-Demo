using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class BalanceDetailRequestDto
    {
        /// <summary>
        /// 必须	授权时获取的access token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 非必填	分页查询当前页数，默认为1
        /// </summary>
        public int pageNum { get; set; }
        /// <summary>
        /// 非必填	每页记录数，默认为20
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 非必填	订单号, 例如：42747145688
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 非必填	开始日期，格式必须：yyyyMMdd
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 非必填	截止日期，格式必须：yyyyMMdd
        /// </summary>
        public string endDate { get; set; }

    }
}
