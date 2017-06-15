using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class IsCodRequestDto
    {
        /// <summary>
        /// 必须 授权时获取的access token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 商品编号，支持批量，(最高支持100个商品)
        /// </summary>
        public List<long> skuIds { get; set; }
        /// <summary>
        /// 必须	 京东一级地址编号
        /// </summary>
        public int province { get; set; }

        /// <summary>
        /// 必须	 京东二级地址编号
        /// </summary>
        public int city { get; set; }

        /// <summary>
        /// 必须	 京东三级地址编号
        /// </summary>
        public int county { get; set; }

        /// <summary>
        /// 必须	 京东四级地址编号,存在四级地址必填，若不存在四级地址，则填0
        /// </summary>
        public int town { get; set; }
    }
}
