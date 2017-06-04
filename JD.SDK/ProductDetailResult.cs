using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class ProductDetailResult
    {
        /// <summary>
        /// 销售单位
        /// </summary>
        public string saleUnit { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public string weight { get; set; }
        /// <summary>
        /// 产地
        /// </summary>
        public string productArea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wareQD { get; set; }
        /// <summary>
        /// 主图地址
        /// </summary>
        public string imagePath { get; set; }
        /// <summary>
        /// 规格参数
        /// </summary>
        public string param { get; set; }
        /// <summary>
        /// 上下架状态
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int sku { get; set; }
        /// <summary>
        /// 售后
        /// </summary>
        public string shouhou { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string brandName { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public string upc { get; set; }
        /// <summary>
        /// 手机端详细介绍
        /// </summary>
        public string appintroduce { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 详细介绍
        /// </summary>
        public string introduction { get; set; }
        /// <summary>
        /// 京东自营礼品卡， 只有当sku为京东自营实物礼品卡的时候才有该字段
        /// </summary>
        public int eleGift { get; set; }

    }
}
