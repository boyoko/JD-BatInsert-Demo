using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class MessageResult
    {
        /// <summary>
        /// 推送id
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public object result { get; set; }
        /// <summary>
        /// 消息类型
        /// 1代表订单拆分变更
        /// 2代表商品价格变更
        /// 4商品上下架变更消息
        /// 5代表该订单已妥投（买断模式代表外单已妥投或外单已拒收）
        /// 6代表添加、删除商品池内商品
        /// 10代表订单取消（不区分取消原因）
        /// 12 代表配送单生成（打包完成后推送，仅提供给买卖宝类型客户）
        /// 13 换新订单生成（换新单下单后推送，仅提供给买卖宝类型客户）
        /// 14 支付失败消息
        /// 15 7天未支付取消消息/未确认取消（cancelType, 1: 7天未支付取消消息; 2: 未确认取消）
        /// 16 商品介绍及规格参数变更消息
        /// 17 赠品促销变更消息
        /// 25新订单消息
        /// 50 京东地址变更消息推送
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime time { get; set; }
    }
}
