using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class OrderTrackObject
    {
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime msgTime { get; set; }
        /// <summary>
        /// 配送内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public string @operator{get;set;}
    }
}
