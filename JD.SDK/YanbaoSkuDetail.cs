using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class YanbaoSkuDetail
    {
        /// <summary>
        /// 保障服务skuId
        /// </summary>
        public long bindSkuId { get; set; }
        /// <summary>
        /// 保障服务sku名称（6字内）
        /// </summary>
        public string bindSkuName { get; set; }
        /// <summary>
        /// 显示排序
        /// </summary>
        public int sortIndex { get; set; }
        /// <summary>
        /// 保障服务sku价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 保障服务说明提示语（20字内）
        /// </summary>
        public string tip { get; set; }
        /// <summary>
        /// 是否是优惠保障服务（PC单品页、PC购物车会根据此标识是否展示优惠图标，优惠图标单品页提供）
        /// </summary>
        public bool favor { get; set; }

    }
}
