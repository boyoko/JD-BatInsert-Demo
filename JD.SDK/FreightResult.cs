using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class FreightResult
    {
        /// <summary>
        /// 总运费
        /// </summary>
        public decimal freight { get; set; }
        /// <summary>
        /// 基础运费
        /// </summary>
        public decimal baseFreight { get; set; }
        /// <summary>
        /// 偏远运费
        /// </summary>
        public decimal remoteRegionFreight { get; set; }

        /// <summary>
        /// 需收取偏远运费的sku,多个用“,”分割 
        /// </summary>
        public string remoteSku { get; set; }
    }
}
