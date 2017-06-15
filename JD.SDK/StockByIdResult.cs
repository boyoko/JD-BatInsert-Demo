using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class StockByIdResult
    {
        /// <summary>
        /// 地址  格式：1_0_0 (分别代表1、2、3级地址)
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public long sku { get; set; }
        /// <summary>
        /// 33 有货 现货-下单立即发货
        ///39 有货 在途-正在内部配货，预计2 ~6天到达本仓库
        ///40 有货 可配货-下单后从有货仓库配货
        ///36 预订
        ///34 无货
        /// </summary>
        public int state { get; set; }
    }
}
