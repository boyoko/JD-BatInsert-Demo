using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class ServiceDetailInfoDto
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int wareId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string wareName { get; set; }
        /// <summary>
        /// 商品品牌
        /// </summary>
        public string wareBrand { get; set; }
        /// <summary>
        /// 明细类型
        /// 主商品(10), 赠品(20), 附件(30)，拍拍取主商品就可以
        /// </summary>
        public int afsDetailType { get; set; }
        /// <summary>
        /// 附件描述
        /// </summary>
        public string wareDescribe { get; set; }
    }
}
