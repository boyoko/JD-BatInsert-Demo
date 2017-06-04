using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class SkuImageResult
    {
        public long id { get; set; }
        public long skuId { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 是否是主图，1为主图，0为附图
        /// </summary>
        public bool isPrimary { get; set; }

        public int? orderSort { get; set; }
    }
}
