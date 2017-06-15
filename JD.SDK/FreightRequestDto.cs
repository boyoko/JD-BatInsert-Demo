using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class FreightRequestDto
    {
        /// <summary>
        /// 必须 授权时获取的access token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// （最多支持50种商品）
        /// </summary>
        public List<GetFreightSkuAttr> sku { get; set; }

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
        /// 非必须	四级地址  (如果该地区有四级地址，则必须传递四级地址，没有四级地址则传0)
        /// </summary>
        public int town { get; set; }
        /// <summary>
        /// 京东支付方式
        /// </summary>
        public int paymentType { get; set; }
    }

    public class GetFreightSkuAttr
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int num { get; set; }
    }
}
