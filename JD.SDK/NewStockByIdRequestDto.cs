using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class NewStockByIdRequestDto
    {
        /// <summary>
        /// 必须 授权时获取的access token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 必须	商品和数量
        /// </summary>
        public List<SkuAndNum> skuNums { get; set; }
        /// <summary>
        /// 必须	格式：1_0_0 (分别代表1、2、3级地址)
        /// </summary>
        public string area { get; set; }
    }

    public class SkuAndNum
    {
        public int skuId { get; set; }
        public int num { get; set; }
    }
}
