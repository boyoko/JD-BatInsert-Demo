using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class YanbaoSkuResult
    {
        /// <summary>
        /// 主商品的sku
        /// </summary>
        public long mainSkuId { get; set; }
        /// <summary>
        /// 保障服务类别显示图标url
        /// </summary>
        public string imgUrl { get; set; }
        /// <summary>
        /// 保障服务类别静态页详情url
        /// </summary>
        public string detailUrl { get; set; }
        /// <summary>
        /// 保障服务类别显示排序
        /// </summary>
        public int displayNo { get; set; }
        /// <summary>
        /// 保障服务分类编码
        /// </summary>
        public string categoryCode { get; set; }
        /// <summary>
        /// 保障服务类别名称
        /// </summary>
        public string displayName { get; set; }
        /// <summary>
        /// 保障服务商品详情列表
        /// </summary>
        public List<YanbaoSkuDetail> fuwuSkuDetailList { get; set; }
    }
}
