using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class SimilarProduct
    {
        /// <summary>
        /// 维度
        /// </summary>
        public int dim { get; set; }
        /// <summary>
        /// 销售名称
        /// </summary>
        public string saleName { get; set; }
        /// <summary>
        /// 商品销售标签
        /// 销售属性下可能只有一个标签，
        /// 此种场景可以选择显示销售名称和标签，也可以不显示
        /// </summary>
        public List<SaleAttr> saleAttrList { get; set; }
    }
}
