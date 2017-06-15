using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class NewStockByIdResult
    {
        /// <summary>
        /// 配送地址id 一级地址_二级地址_三级地址
        /// </summary>
        public string areaId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        /// 库存状态编号 33,39,40,36,34
        /// </summary>
        public int stockStateId { get; set; }
        /// <summary>
        /// 库存状态描述
        ///33 有货 现货-下单立即发货
        ///39 有货 在途-正在内部配货，预计2 ~6天到达本仓库
        ///40 有货 可配货-下单后从有货仓库配货
        ///36 预订
        ///34 无货
        /// </summary>
        public string StockStateDesc { get; set; }
        /// <summary>
        /// 剩余数量 -1未知；当库存小于5时展示真实库存数量
        /// </summary>
        public int remainNum { get; set; }
    }
}
