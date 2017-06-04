using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class GetCommentSummarysResult
    {
        /// <summary>
        /// 商品评分 (5颗星，4颗星)
        /// </summary>
        public int averageScore { get; set; }
        /// <summary>
        /// 好评度
        /// </summary>
        public decimal goodRate { get; set; }
        /// <summary>
        /// 中评度
        /// </summary>
        public decimal generalRate { get; set; }
        /// <summary>
        /// 差评度
        /// </summary>
        public decimal poorRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long skuId { get; set; }
    }
}
