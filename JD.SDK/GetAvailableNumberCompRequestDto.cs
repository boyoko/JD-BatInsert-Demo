using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class GetAvailableNumberCompRequestDto
    {
        /// <summary>
        /// 授权时获取的access token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 请求的json参数
        /// </summary>
        public ParamObject param { get; set; }
    }

    public class ParamObject
    {
        /// <summary>
        /// 必需 京东订单号
        /// </summary>
        public long jdOrderId { get; set; }
        /// <summary>
        /// 必需 京东商品编号
        /// </summary>
        public long skuId { get; set; }
    }
}
