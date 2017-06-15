using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class CheckAreaRequestDto
    {
        /// <summary>
        /// 必须 授权时获取的access token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 必须 一级地址
        /// </summary>
        public int provinceId { get; set; }

        /// <summary>
        /// 必须 二级地址
        /// </summary>
        public int cityId { get; set; }

        /// <summary>
        /// 必须 三级地址，如果是空请传入0
        /// </summary>
        public int countyId { get; set; }

        /// <summary>
        /// 必须 四级地址，如果是空请传入0
        /// </summary>
        public int townId { get; set; }
    }
}
