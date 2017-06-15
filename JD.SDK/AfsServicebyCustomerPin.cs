using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class AfsServicebyCustomerPin
    {
        /// <summary>
        /// 服务单号
        /// </summary>
        public int afsServiceId { get; set; }
        /// <summary>
        /// 服务类型码 退货(10)、换货(20)、维修(30)
        /// </summary>
        public int customerExpect { get; set; }
        /// <summary>
        /// 服务类型名称
        /// </summary>
        public string customerExpectName { get; set; }
        /// <summary>
        /// 服务单申请时间 格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string afsApplyTime { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public long orderId { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public long  wareId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string wareName { get; set; }
        /// <summary>
        /// 服务单环节
        /// 申请阶段(10),审核不通过(20),客服审核(21),商家审核(22),京东收货(31),商家收货(32),
        /// 京东处理(33),商家处理(34), 用户确认(40),完成(50), 取消 (60);
        /// </summary>
        public int afsServiceStep { get; set; }
        /// <summary>
        /// 服务单环节名称
        /// 申请阶段,客服审核,商家审核,京东收货,商家收货, 京东处理,商家处理, 用户确认,完成, 取消;
        /// </summary>
        public string afsServiceStepName { get; set; }
        /// <summary>
        /// 是否可取消 0代表否，1代表是
        /// </summary>
        public int cancel { get; set; }
    }
}
