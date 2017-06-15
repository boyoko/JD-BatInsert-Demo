using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class BalanceDetailData
    {
        /// <summary>
        /// 余额明细ID
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 账户类型  1：可用余额 2：锁定余额
        /// </summary>
        public int accountType { get; set; }
        /// <summary>
        /// 金额（元），有正负，可以是零，表示订单流程变化，如退款时会先有一条退款申请的记录，金额为0
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 京东Pin
        /// </summary>
        public string pin { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int tradeType { get; set; }
        /// <summary>
        /// 业务类型名称
        /// </summary>
        public string tradeTypeName { get; set; }
        /// <summary>
        /// 余额变动日期
        /// </summary>
        public DateTime createdDate { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string notePub { get; set; }
        /// <summary>
        /// 业务号，一般由余额系统，在每一次操作成功后自动生成，也可以由前端业务系统传入
        /// </summary>
        public long tradeNo { get; set; }

    }
}
