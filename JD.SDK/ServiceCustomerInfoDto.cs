using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class ServiceCustomerInfoDto
    {
        /// <summary>
        /// 客户京东账号
        /// </summary>
        public string customerPin { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string customerName { get; set; }
        /// <summary>
        /// 服务单联系人
        /// </summary>
        public string customerContactName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string customerTel { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string customerMobilePhone { get; set; }
        /// <summary>
        /// 电子邮件地址
        /// </summary>
        public string customerEmail { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string customerPostcode { get; set; }
    }
}
