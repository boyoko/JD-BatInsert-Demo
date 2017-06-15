using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class CompatibleServiceDetailDto
    {
        /// <summary>
        /// 服务单号
        /// </summary>
        public int afsServiceId { get; set; }
        /// <summary>
        /// 服务类型码
        /// </summary>
        public int customerExpect { get; set; }
        /// <summary>
        /// 服务单申请时间 格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string afsApplyTime { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public long orderId { get; set; }
        /// <summary>
        /// 是不是有发票 0没有 1有
        /// </summary>
        public int isHasInvoice { get; set; }
        /// <summary>
        /// 是不是有检测报告 0没有 1有
        /// </summary>
        public int isNeedDetectionReport { get; set; }
        /// <summary>
        /// 是不是有包装	0没有 1有
        /// </summary>
        public int isHasPackage { get; set; }
        /// <summary>
        /// 上传图片访问地址 不同图片逗号分割，可能为空
        /// </summary>
        public string questionPic { get; set; }
        /// <summary>
        /// 服务单环节: 申请阶段(10),审核不通过(20),客服审核(21),商家审核(22),京东收货(31),商家收货(32), 京东处理(33),商家处理(34), 用户确认(40),完成(50), 取消(60)
        /// </summary>
        public int afsServiceStep { get; set; }
        /// <summary>
        /// 服务单环节名称  申请阶段,客服审核,商家审核,京东收货,商家收货, 京东处理,商家处理, 用户确认,完成, 取消;
        /// </summary>
        public string afsServiceStepName { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string approveNotes { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string questionDesc { get; set; }
        /// <summary>
        /// 审核结果 
        ///直赔积分 (11),
        ///直赔余额(12),
        ///直赔优惠卷(13),
        ///直赔京豆(14),
        ///直赔商品(21),上门换新(22),自营取件(31),客户送货(32),客户发货(33),闪电退款(34),
        ///虚拟退款(35),大家电检测(80),大家电安装(81),大家电移机(82),大家电维修(83),大家电其它(84);
        /// </summary>
        public int approvedResult { get; set; }
        /// <summary>
        /// 审核结果名称
        /// 直赔积分 ,直赔余额 ,直赔优惠卷 ,直赔京豆,直赔商品,上门换新,自营取件, 
        ///客户送货,客户发货,闪电退款,虚拟退款,大家电检测,大家电安装,大家电移机,大家电维修 ,大家电其它;
        /// </summary>
        public string approvedResultName { get; set; }
        /// <summary>
        /// 处理结果
        /// 返修换新(23),
        ///退货(40), 换良(50),原返(60),病单(71),出检(72),维修(73),强制关单(80),线下换新(90)
        /// </summary>
        public int processResult { get; set; }
        /// <summary>
        /// 处理结果名称
        /// 返修换新,退货 , 换良,原返,病单,出检,维修,强制关单,线下换新 
        /// </summary>
        public string processResultName { get; set; }
        /// <summary>
        /// 客户信息
        /// </summary>
        public ServiceCustomerInfoDto serviceCustomerInfoDTO { get; set; }
        /// <summary>
        /// 售后地址信息
        /// </summary>
        public ServiceAftersalesAddressInfoDto serviceAftersalesAddressInfoDTO { get; set; }
        /// <summary>
        /// 客户发货信息
        /// </summary>
        public ServiceExpressInfoDto serviceExpressInfoDTO { get; set; }
        /// <summary>
        /// 退款明细
        /// </summary>
        public List<ServiceFinanceDetailInfoDto> serviceFinanceDetailInfoDTOs { get; set; }
        /// <summary>
        /// 服务单追踪信息
        /// </summary>
        public List<ServiceTrackInfoDto> serviceTrackInfoDTOs { get; set; }
        /// <summary>
        /// 服务单商品明细
        /// </summary>
        public List<ServiceDetailInfoDto> serviceDetailInfoDTOs { get; set; }
        /// <summary>
        /// 获取服务单允许的操作列表
        /// 列表为空代表不允许操作
        ///列表包含1代表取消
        ///列表包含2代表允许填写或者修改客户发货信息
        /// </summary>
        public List<int> allowOperations { get; set; }


    }
}
