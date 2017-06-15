using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.Common
{
    public class ConstHelper
    {
        public const string ACCESS_TOKEN = "access_token";
        public const string ACCESS_TOKEN_URL = "https://bizapi.jd.com/oauth2/accessToken";

        public const string REFRESH_TOKEN = "refresh_token";
        public const string REFRESH_TOKEN_URL = "https://bizapi.jd.com/oauth2/refreshToken";

        public const string CLIENT_ID = "B8CXYmEn2epXK2NRcWSf";
        public const string CLIENT_SECRET = "RusRZS5iVcloqT9MKh4X";
        public const string BASEURL = "https://bizapi.jd.com/api/";
        public const string GETPAGENUM_URI = "product/getPageNum";
        public const string GETSKU_URI = "product/getSku";
        public const string GETSKUBYPAGE_URI = "product/getSkuByPage";
        public const string GETDETAIL_URI = "product/getDetail";
        public const string SKUSTATE_URI = "product/skuState";
        public const string SKUIMAGE_URI = "product/skuImage";
        public const string GETCOMMENTSUMMARYS_URI = "product/getCommentSummarys";
        public const string CHECKAREALIMIT_URI = "product/checkAreaLimit";
        public const string GETISCOD_URI = "product/getIsCod";
        public const string GETSKUGIFT_URI = "product/getSkuGift";
        public const string GETFREIGHT_URI = "order/getFreight";
        public const string SEARCH_URI = "search/search";
        public const string CHECK_URI = "product/check";
        public const string GETYANBAOSKU_URI = "product/getYanbaoSku";
        public const string GETCATEGORY_URI = "product/getCategory";
        public const string GETCATEGORYS_URI = "product/getCategorys";
        public const string GETSIMILARSKU_URI = "product/getSimilarSku";



        public const string GETPROVINCE_URI = "area/getProvince";
        public const string GETCITY_URI = "area/getCity";
        public const string GETCOUNTY_URI = "area/getCounty";
        public const string GETTOWN_URI = "area/getTown";
        /// <summary>
        /// 4.5 验证四级地址是否正确URL
        /// </summary>
        public const string CHECKAREA_URI = "area/checkArea";




        /// <summary>
        /// 5.1 批量查询京东价格
        /// </summary>
        public const string GETJDPRICE_URI = "price/getJdPrice";
        /// <summary>
        /// 5.2 批量查询协议价价格
        /// </summary>
        public const string GETPRICE_URI = "price/getPrice";
        /// <summary>
        /// 5.3 批量查询商品售卖价
        /// </summary>
        public const string GETSELLPRICE_URI = "price/getSellPrice";


        /// <summary>
        /// 6.2 批量获取库存接口（建议订单详情页、下单使用）
        /// </summary>
        public const string GETNEWSTOCKBYID_URI = "stock/getNewStockById";
        /// <summary>
        /// 6.3 批量获取库存接口(建议商品列表页使用)
        /// </summary>
        public const string GETSTOCKBYID_URI = "stock/getStockById";



        public const string SUBMITORDER_URI = "order/submitOrder";
        public const string CONFIRMORDER_URI = "order/confirmOrder";
        public const string CANCEL_URI = "order/cancel";
        public const string DOPAY_URI = "order/doPay";

        /// <summary>
        /// 7.6 订单反查接口
        /// </summary>
        public const string SELECTJDORDERIDBYTHIRDORDER_URI = "order/selectJdOrderIdByThirdOrder";
        /// <summary>
        /// 7.7 查询京东订单信息接口
        /// </summary>
        public const string SELECTJDORDER_URI = "order/selectJdOrder";
        /// <summary>
        /// 7.8 查询配送信息接口
        /// </summary>
        public const string ORDERTRACK_URI = "order/orderTrack";
        /// <summary>
        /// 7.9 统一余额查询接口
        /// </summary>
        public const string GETBALANCE_URI = "price/getBalance";
        /// <summary>
        /// 7.11 余额明细查询接口
        /// </summary>
        public const string GETBALANCEDETAIL_URI = "price/getBalanceDetail";

        /// <summary>
        /// 8.1 新建订单查询接口
        /// </summary>
        public const string CHECKNEWORDER_URI = "checkOrder/checkNewOrder";
        /// <summary>
        /// 8.2 获取妥投订单接口
        /// </summary>
        public const string CHECKDLOKORDER_URI = "checkOrder/checkDlokOrder";
        /// <summary>
        /// 8.3 获取拒收消息接口
        /// </summary>
        public const string CHECKREFUSEORDER_URI = "checkOrder/checkRefuseOrder";




        /// <summary>
        /// 9.1 信息推送接口
        /// </summary>
        public const string GET_URI = "message/get";
        /// <summary>
        /// 9.2 根据推送id，删除推送信息接口
        /// </summary>
        public const string DEL_URI = "message/del";


        /// <summary>
        /// 10.1 服务单保存申请
        /// </summary>
        public const string CREATEAFSAPPLY_URI = "afterSale/createAfsApply";
        /// <summary>
        /// 10.2 填写客户发运信息
        /// </summary>
        public const string UPDATESENDSKU_URI = "afterSale/updateSendSku";
        /// <summary>
        /// 10.3 校验某订单中某商品是否可以提交售后服务
        /// </summary>
        public const string GETAVAILABLENUMBERCOMP_URI = "afterSale/getAvailableNumberComp";
        /// <summary>
        /// 10.4 根据订单号、商品编号查询支持的服务类型
        /// </summary>
        public const string GETCUSTOMEREXPECTCOMP_URI = "afterSale/getCustomerExpectComp";
        /// <summary>
        /// 10.5 根据订单号、商品编号查询支持的商品返回京东方式
        /// </summary>
        public const string GETWARERETURNJDCOMP_URI = "afterSale/getWareReturnJdComp";
        /// <summary>
        /// 10.6 根据客户账号和订单号分页查询服务单概要信息
        /// </summary>
        public const string GETSERVICELISTPAGE_URI = "afterSale/getServiceListPage";
        /// <summary>
        /// 10.7 根据服务单号查询服务单明细信息
        /// </summary>
        public const string GETSERVICEDETAILINFO_URI = "afterSale/getServiceDetailInfo";
        /// <summary>
        /// 10.8 取消服务单/客户放弃
        /// </summary>
        public const string AUDITCANCEL_URI = "afterSale/auditCancel";





    }
}
