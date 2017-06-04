using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class GetCategorysRequestDto
    {
        /// <summary>
        /// 必须 授权时获取的access token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 必须 页号，从1开始
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 必须 页大小，最大值5000
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 非必须 父ID
        /// </summary>
        public int parentId { get; set; }
        /// <summary>
        /// 非必须 分类等级（0:一级； 1:二级；2：三级）
        /// </summary>
        public int catClass { get; set; }
    }
}
