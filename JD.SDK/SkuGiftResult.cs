using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class SkuGiftResult
    {
        /// <summary>
        /// 赠品/附件列表
        /// </summary>
        public List<GiftAttr> gifts { get; set; }
        /// <summary>
        /// 赠品要求最多购买数量（为0表示没配置）
        /// </summary>
        public int maxNum { get; set; }
        /// <summary>
        /// 赠品要求最多购买数量（为0表示没配置）
        /// </summary>
        public int minNum { get; set; }
        /// <summary>
        /// 促销开始时间
        /// </summary>
        public long promoStartTime { get; set; }
        /// <summary>
        /// 促销结束时间
        /// </summary>
        public long promoEndTime { get; set; }

    }
}
