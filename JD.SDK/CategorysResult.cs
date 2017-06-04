using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class CategorysResult
    {
        /// <summary>
        /// 分类列表信息
        /// </summary>
        public List<CategoryResult> categorys { get; set; }
        /// <summary>
        /// 条目总数
        /// </summary>
        public int totalRows { get; set; }
        /// <summary>
        /// 当前页号
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int pageSize { get; set; }

    }
}
