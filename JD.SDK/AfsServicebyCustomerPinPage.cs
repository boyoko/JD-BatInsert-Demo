using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class AfsServicebyCustomerPinPage
    {
        public List<AfsServicebyCustomerPin> serviceInfoList { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int totalNum { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int pageSize { get; set; }
        // <summary>
        /// 总页数
        /// </summary>
        public int pageNum { get; set; }
        // <summary>
        /// 当前页数
        /// </summary>
        public int pageIndex { get; set; }
    }
}
