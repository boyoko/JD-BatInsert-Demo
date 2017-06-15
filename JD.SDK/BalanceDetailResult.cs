using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class BalanceDetailResult
    {
        /// <summary>
        /// 记录总条数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 分页大小，默认20，最大1000
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public List<BalanceDetailData> data { get; set; }


    }
}
