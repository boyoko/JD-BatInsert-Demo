using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class SubmitOrderRequestDto
    {
        /// <summary>
        /// 必须 授权时获取的access token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 必须 第三方的订单单号
        /// </summary>
        public string thirdOrder { get; set; }
        /// <summary>
        /// 必须 对象列表,
        /// </summary>
        public List<SkuObject> sku { get; set; }
        /// <summary>
        /// 必须 收货人
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 必须 一级地址
        /// </summary>
        public int province { get; set; }

        /// <summary>
        /// 必须 二级地址
        /// </summary>
        public int city { get; set; }

        /// <summary>
        /// 必须 三级地址
        /// </summary>
        public int county { get; set; }

        /// <summary>
        /// 必须 四级地址  
        /// (如果该地区有四级地址，则必须传递四级地址，没有四级地址则传0)
        /// </summary>
        public int town { get; set; }
        /// <summary>
        /// 必须 详细地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 非必须	邮编
        /// </summary>
        public string zip { get; set; }
        /// <summary>
        /// 非必须	座机号 
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 必须	手机号 
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 必须	邮箱 
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 非必须	备注（少于100字）
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 必须	开票方式(1为随货开票，0为订单预借，2为集中开票 )
        /// </summary>
        public int invoiceState { get; set; }

        /// <summary>
        /// 必须	 1普通发票2增值税发票
        /// </summary>
        public int invoiceType { get; set; }

        /// <summary>
        /// 必须	 发票类型：4个人，5单位
        /// </summary>
        public int selectedInvoiceTitle { get; set; }
        /// <summary>
        /// 必须	发票抬头  (如果selectedInvoiceTitle=5则此字段必须)
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 必须	1:明细，3：电脑配件，19:耗材，22：办公用品备注:若增值发票则只能选1 明细
        /// </summary>
        public int invoiceContent { get; set; }

        /// <summary>
        /// 必须	支付方式 (1：货到付款，2：邮局付款，4：在线支付，5：公司转账，6：银行转账，7：网银钱包，101：金采支付)
        /// </summary>
        public int paymentType { get; set; }
        /// <summary>
        /// 必须	使用余额paymentType=4时，此值固定是1 其他支付方式0
        /// </summary>
        public int isUseBalance { get; set; }

        /// <summary>
        /// 必须	是否预占库存，0是预占库存（需要调用确认订单接口），1是不预占库存     金融支付必须预占库存传0
        /// </summary>
        public int submitState { get; set; }

        /// <summary>
        /// 非必须	增值票收票人姓名备注：当invoiceType=2 且invoiceState=1时则此字段必填
        /// </summary>
        public string invoiceName { get; set; }
        /// <summary>
        /// 非必须	增值票收票人电话  备注：当invoiceType=2 且invoiceState=1时则此字段必填
        /// </summary>
        public string invoicePhone { get; set; }
        /// <summary>
        /// 非必须	增值票收票人所在省(京东地址编码)备注：当invoiceType=2 且invoiceState=1时则此字段必填
        /// </summary>
        public int invoiceProvice { get; set; }
        /// <summary>
        /// 非必须	增值票收票人所在市(京东地址编码)
        ///备注：当invoiceType=2 且invoiceState=1时则此字段必填
        /// </summary>
        public int invoiceCity { get; set; }

        /// <summary>
        /// 非必须	增值票收票人所在区/县(京东地址编码)
        ///备注：当invoiceType=2 且invoiceState=1时则此字段必填
        /// </summary>
        public int invoiceCounty { get; set; }
        /// <summary>
        /// 非必须	增值票收票人所在地址
        ///备注：当invoiceType=2 且invoiceState=1时则此字段必填
        /// </summary>
        public string invoiceAddress { get; set; }
        /// <summary>
        /// 必须 下单价格模式
        /// 0: 客户端订单价格快照不做验证对比，还是以京东价格正常下单;
        /// 1:必需验证客户端订单价格快照，如果快照与京东价格不一致返回下单失败，需要更新商品价格后，重新下单;
        /// </summary>
        public int doOrderPriceMode { get; set; }
        /// <summary>
        /// 必须 客户端订单价格快照
        /// </summary>
        public List<OrderPriceSnap> orderPriceSnap { get; set; }
        /// <summary>
        /// 大家电配送日期
        /// 默认值为-1，0表示当天，1表示明天，2：表示后天; 如果为-1表示不使用大家电预约日历
        /// </summary>
        public int reservingDate { get; set; } = -1;
        /// <summary>
        /// 大家电安装日期
        /// 不支持默认按-1处理，0表示当天，1表示明天，2：表示后天
        /// </summary>
        public int installDate { get; set; } = -1;
        /// <summary>
        /// 大家电是否选择了安装
        /// 是否选择了安装，默认为true，选择了“暂缓安装”，此为必填项，必填值为false。
        /// </summary>
        public bool needInstall { get; set; }
        /// <summary>
        /// 中小件配送预约日期 格式：yyyy-MM-dd
        /// </summary>
        public string promiseDate { get; set; }
        /// <summary>
        /// 中小件配送预约时间段  时间段如： 9:00-15:00
        /// </summary>
        public string promiseTimeRange { get; set; }
        /// <summary>
        /// 中小件预约时间段的标记
        /// </summary>
        public int promiseTimeRangeCode { get; set; }


    }

    public class SkuObject
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        //public decimal price { get; set; }
        /// <summary>
        /// 附件
        /// 表示是否需要附件，默认每个订单都给附件，默认值为：true，
        /// 如果客户实在不需要附件bNeedAnnex可以给false，
        /// 该参数配置为false时请谨慎，真的不会给客户发附件的
        /// </summary>
        public bool bNeedAnnex { get; set; } = true;
        /// <summary>
        /// 赠品
        /// 表示是否需要增品，默认不给增品，默认值为：false，
        /// 如果需要增品bNeedGift请给true,建议该参数都给true,
        /// 但如果实在不需要增品可以给false;
        /// </summary>
        public bool bNeedGift { get; set; } = false;
        /// <summary>
        /// 商品编号列表
        /// </summary>
        public List<long> yanbao { get; set; }
    }

    public class OrderPriceSnap
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public long skuId { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal price { get; set; }
    }

}
