using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class ServiceTrackInfoDto
    {
        /// <summary>
        /// 服务单号
        /// </summary>
        public int afsServiceId { get; set; }
        /// <summary>
        /// 追踪标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 追踪内容
        /// </summary>
        public string context { get; set; }
        /// <summary>
        /// 提交时间	格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 操作人昵称
        /// </summary>
        public string createName { get; set; }
        /// <summary>
        /// 操作人账号
        /// </summary>
        public string createPin { get; set; }
    }
}
