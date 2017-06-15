using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JD.Common;
using System.Xml.Linq;

namespace JD.SDK
{
    public class SynchronousDataService
    {
        /// <summary>
        /// 1.3 获取Access Token
        /// </summary>
        /// <returns></returns>
        public async virtual Task<string> AccessToken()
        {
            var token = await CacheHelper.Get(ConstHelper.ACCESS_TOKEN);
            if (string.IsNullOrWhiteSpace(token))
            {
                var url = ConstHelper.ACCESS_TOKEN_URL;
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                var grant_type = ConstHelper.ACCESS_TOKEN;
                var client_id = ConstHelper.CLIENT_ID;
                var client_secret = ConstHelper.CLIENT_SECRET;
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var username = "门财科技";
                var password = EncryptHelper.MD5Hash("123456");
                var scope = string.Empty;
                var tmp = client_secret
                            + timestamp
                            + client_id
                            + username
                            + password
                            + grant_type
                            + scope
                            + client_secret;
                var md5Tmp = EncryptHelper.MD5Hash(tmp.Trim());
                var upperStr = md5Tmp.ToUpper();
                var sign = upperStr;
                dic.Add("grant_type", grant_type);
                dic.Add("client_id", client_id);
                dic.Add("client_secret", client_secret);
                dic.Add("timestamp", timestamp);
                dic.Add("username", username);
                dic.Add("password", password);
                dic.Add("scope", scope);
                dic.Add("sign", sign);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var tokenResult = StringHelper.JsonToObj<BaseResult<TokenResult>>(x)
                    as BaseResult<TokenResult>;

                if (tokenResult.Success)
                {
                    if (tokenResult.Result != null && !string.IsNullOrWhiteSpace(tokenResult.Result.access_token))
                    {
                        await CacheHelper.Set(ConstHelper.ACCESS_TOKEN, tokenResult.Result.access_token);
                        await CacheHelper.Set(ConstHelper.REFRESH_TOKEN, tokenResult.Result.refresh_token);
                        token = tokenResult.Result.access_token;
                    }
                }

            }
            return token;
        }

        /// <summary>
        /// 1.4 使用Refresh Token 刷新 Access Token
        /// 每天自动刷新token
        /// </summary>
        /// <returns></returns>
        public async virtual Task<string> RefreshToken()
        {
            var token = string.Empty;
            var refresh_token = await CacheHelper.Get(ConstHelper.REFRESH_TOKEN);
            if (!string.IsNullOrWhiteSpace(refresh_token))
            {
                var url = ConstHelper.REFRESH_TOKEN_URL;
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                var client_id = ConstHelper.CLIENT_ID;
                var client_secret = ConstHelper.CLIENT_SECRET;
                dic.Add("refresh_token", refresh_token);
                dic.Add("client_id", client_id);
                dic.Add("client_secret", client_secret);
                var x = await HttpHelper.HttpClientPost(url, dic);
                var tokenResult = StringHelper.JsonToObj<BaseResult<TokenResult>>(x)
                    as BaseResult<TokenResult>;

                if (tokenResult.Success)
                {
                    if (tokenResult.Result != null && !string.IsNullOrWhiteSpace(tokenResult.Result.access_token))
                    {
                        await CacheHelper.Set(ConstHelper.ACCESS_TOKEN, tokenResult.Result.access_token);
                        await CacheHelper.Set(ConstHelper.REFRESH_TOKEN, tokenResult.Result.refresh_token);
                        token = tokenResult.Result.access_token;
                    }
                }
            }
            else
            {
                //缓存中没有refresh_token 时，先获取token，refresh_token 等信息，并进行缓存，
                token = await AccessToken();
            }
            return token;
        }
        /// <summary>
        /// 3.1 获取商品池编号接口
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <returns>商品池名次和编号对象的数组</returns>
        public async virtual Task<List<PageNumResult>> GetPageNum(string token)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETPAGENUM_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<PageNumResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.2 获取池内商品编号接口
        /// 池子编号为getPageNum 接口返回的值
        /// 如："name":"笔记本","page_num":"672"
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="pageNum">池子编号</param>
        /// <returns>用,分割的多个商品编号</returns>
        public async virtual Task<string> GetSku(string token, string pageNum)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETSKU_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("pageNum", pageNum);
                var x = await HttpHelper.HttpClientPost(url, dic);
                //调用京东接口报错
                if (string.IsNullOrWhiteSpace(x))
                {
                    return x;
                }

                var result = ConvertJsonToResult<string>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.3 获取池内商品编号接口-品类商品池（兼容老接口）
        /// 池子编号为getPageNum 接口返回的值
        /// 如："name":"笔记本","page_num":"672"
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="pageNum">池子编号</param>
        /// <param name="pageNo">页码，默认取第一页；每页最多10000条数据，品类商品池可能存在多页数据，具体根据返回的页总数判断是否有下一页数据</param>
        /// <returns>用,分割的多个商品编号</returns>
        public async virtual Task<string> GetSkuByPage(string token, string pageNum, string pageNo)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETSKUBYPAGE_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("pageNum", pageNum);
                dic.Add("pageNo", pageNo);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return x;
                }
                var result = ConvertJsonToResult<string>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.4 获取商品详细信息接口
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="sku">商品编号，只支持单个查询</param>
        /// <param name="isShow">false:查询商品基本信息 | true:商品基本信息 + 商品售后信息 + 移动商品详情介绍信息</param>
        /// <returns></returns>
        public async virtual Task<ProductDetailResult> GetDetail(string token, string sku, bool isShow = false)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETDETAIL_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("sku", sku);
                dic.Add("isShow", isShow.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<ProductDetailResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.5 获取商品上下架状态接口
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="skuIds">商品编号，支持批量，以，分隔  (最高支持100个商品)1为上架，0为下架</param>
        /// <returns></returns>
        public async virtual Task<List<SkuStateResult>> SkuState(string token, string skuIds)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.SKUSTATE_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("sku", skuIds);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<SkuStateResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 3.6 获取所有图片信息
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="skuIds">商品编号，支持批量，以，分隔  (最高支持100个商品)</param>
        /// <returns></returns>
        public async virtual Task<Dictionary<string, List<SkuImageResult>>> SkuImage(string token, string skuIds)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.SKUIMAGE_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("sku", skuIds);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                /*
                 {"success":true,"resultMessage":"","resultCode":"0000","result":{
                    "3670994":[
                        {"id":29282968,"skuId":3670994,"path":"jfs/t3223/115/8236016400/73090/482b5c42/58c1f8b1Ncee9dc14.jpg","created":"2017-03-10 10:35:51","modified":"2017-03-10 10:35:51","yn":1,"isPrimary":1,"orderSort":null,"position":null,"type":0,"features":null},
                        {"id":29282971,"skuId":3670994,"path":"jfs/t3184/156/8237415165/105488/b8f306a5/58c1f8b0N56aa7e8b.jpg","created":"2017-03-10 10:35:51","modified":"2017-03-10 10:35:51","yn":1,"isPrimary":0,"orderSort":1,"position":null,"type":0,"features":null},
                        {"id":29282974,"skuId":3670994,"path":"jfs/t3229/89/8292452804/67216/d2fca19c/58c1f8bdN6f205ad4.jpg","created":"2017-03-10 10:35:51","modified":"2017-03-10 10:35:51","yn":1,"isPrimary":0,"orderSort":2,"position":null,"type":0,"features":null},
                        {"id":29282977,"skuId":3670994,"path":"jfs/t4336/267/1450577171/98029/d87104f3/58c1f8bdN39551be0.jpg","created":"2017-03-10 10:35:51","modified":"2017-03-10 10:35:51","yn":1,"isPrimary":0,"orderSort":3,"position":null,"type":0,"features":null},
                        {"id":29282980,"skuId":3670994,"path":"jfs/t4162/157/1446016274/241037/a6509d0b/58c1f8bdN9ef41ade.jpg","created":"2017-03-10 10:35:51","modified":"2017-03-10 10:35:51","yn":1,"isPrimary":0,"orderSort":4,"position":null,"type":0,"features":null}
                        ],
                    "3670993":[
                        {"id":26920852,"skuId":3670993,"path":"jfs/t3544/241/1666100864/227049/f2d45291/582e4b2eN4c175d8d.jpg","created":"2016-11-18 09:03:45","modified":"2016-11-18 09:03:45","yn":1,"isPrimary":1,"orderSort":null,"position":null,"type":0,"features":null},
                        {"id":26920853,"skuId":3670993,"path":"jfs/t3541/183/1671411259/187656/911c854e/582e4b35N1cea6179.jpg","created":"2016-11-18 09:03:45","modified":"2016-11-18 09:03:45","yn":1,"isPrimary":0,"orderSort":1,"position":null,"type":0,"features":null},
                        {"id":26920854,"skuId":3670993,"path":"jfs/t3772/70/1670063674/311405/ac0d36e9/582e4b3bNc3d08993.jpg","created":"2016-11-18 09:03:45","modified":"2016-11-18 09:03:45","yn":1,"isPrimary":0,"orderSort":2,"position":null,"type":0,"features":null},
                        {"id":26920855,"skuId":3670993,"path":"jfs/t3484/156/1696327702/258410/4dd7bb67/582e4b40Na847bb9e.jpg","created":"2016-11-18 09:03:45","modified":"2016-11-18 09:03:45","yn":1,"isPrimary":0,"orderSort":3,"position":null,"type":0,"features":null},
                        {"id":26920856,"skuId":3670993,"path":"jfs/t3439/248/1663862965/237141/618e7665/582e4b45Ncc72b141.jpg","created":"2016-11-18 09:03:45","modified":"2016-11-18 09:03:45","yn":1,"isPrimary":0,"orderSort":4,"position":null,"type":0,"features":null}
                        ]
                        }}
                 */
                var result = ConvertJsonToResult<Dictionary<string, List<SkuImageResult>>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 3.7 商品好评度查询
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="skuIds">商品编号，支持批量，以，分隔  (最高支持50个商品)</param>
        /// <returns></returns>
        public async virtual Task<List<CommentSummarysResult>> GetCommentSummarys(string token, string skuIds)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETCOMMENTSUMMARYS_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("sku", skuIds);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<CommentSummarysResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.8 商品区域购买限制查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<List<CheckAreaLimitResult>> CheckAreaLimit(CheckAreaLimitRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.CHECKAREALIMIT_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                if(request.skuIds != null && request.skuIds.Any())
                {
                    var str = string.Join(",", request.skuIds);
                    dic.Add("skuIds", str);
                }
                else
                {
                    throw new ArgumentNullException(nameof(request.skuIds));
                }
                dic.Add("province", request.province.ToString());
                dic.Add("city", request.city.ToString());
                dic.Add("county", request.county.ToString());
                dic.Add("town", request.town.ToString());

                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }

                var result = ConvertJsonToResult<List<CheckAreaLimitResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.9 商品区域是否支持货到付款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<bool> GetIsCod(IsCodRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETISCOD_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                if (request.skuIds != null && request.skuIds.Any())
                {
                    var str = string.Join(",", request.skuIds);
                    dic.Add("skuIds", str);
                }
                else
                {
                    throw new ArgumentNullException(nameof(request.skuIds));
                }
                dic.Add("province", request.province.ToString());
                dic.Add("city", request.city.ToString());
                dic.Add("county", request.county.ToString());
                dic.Add("town", request.town.ToString());

                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return false;
                }

                var result = ConvertJsonToResult<bool>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.10 查询赠品信息接口
        /// 购买数量大于赠品要求最多购买数量，不加赠品
        ///购买数量小于赠品要求最少购买数量，不加赠品
        ///下单时间不在促销时间范围内，不加赠品
        ///需要计算赠品量的倍数 = 主商品 / 促销要求主商品最少数（minNum为0时，赠品数量倍数为主商品数量）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<SkuGiftResult> GetSkuGift(SkuGiftRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETSKUGIFT_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                dic.Add("skuId", request.skuId.ToString());
                dic.Add("province", request.province.ToString());
                dic.Add("city", request.city.ToString());
                dic.Add("county", request.county.ToString());
                dic.Add("town", request.town.ToString());

                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }

                var result = ConvertJsonToResult<SkuGiftResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.11 运费查询接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<FreightResult> GetFreight(FreightRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETSKUGIFT_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                if(request.sku!=null && request.sku.Any())
                {
                    var jsonStr = StringHelper.ToJson(request.sku);
                    dic.Add("sku", jsonStr);
                }
                else
                {
                    throw new ArgumentNullException(nameof(request.sku));
                }
                dic.Add("province", request.province.ToString());
                dic.Add("city", request.city.ToString());
                dic.Add("county", request.county.ToString());
                dic.Add("town", request.town.ToString());
                dic.Add("paymentType", request.paymentType.ToString());

                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }

                var result = ConvertJsonToResult<FreightResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        //3.12 商品搜索接口 暂时没有对接，不打算使用京东的搜索接口，要使用自己的搜索接口

        /// <summary>
        /// 3.13 商品可售验证接口
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="skuIds">商品编号，支持批量，以,分隔  (最高支持100个商品)</param>
        /// <returns></returns>
        public async virtual Task<List<CheckResult>> Check(string token, string skuIds)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETCOMMENTSUMMARYS_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("skuIds", skuIds);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<CheckResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.14 查询商品延保接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<Dictionary<long,YanbaoSkuResult>> GetYanbaoSku(YanbaoSkuRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETYANBAOSKU_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                if (request.skuIds != null && request.skuIds.Any())
                {
                    var jsonStr = StringHelper.ToJson(request.skuIds);
                    dic.Add("skuIds", jsonStr);
                }
                else
                {
                    throw new ArgumentNullException(nameof(request.skuIds));
                }
                dic.Add("province", request.province.ToString());
                dic.Add("city", request.city.ToString());
                dic.Add("county", request.county.ToString());
                dic.Add("town", request.town.ToString());

                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }

                var result = ConvertJsonToResult<Dictionary<long, YanbaoSkuResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.15 查询分类信息接口
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="cid">分类id（可通过商品详情接口查询）</param>
        /// <returns></returns>
        public async virtual Task<CategoryResult> GetCategory(string token, string cid)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETCATEGORY_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("cid", cid);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<CategoryResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.16 查询分类列表信息接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<CategorysResult> GetCategorys(CategorysRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETCATEGORYS_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                dic.Add("pageNo", request.pageNo.ToString());
                dic.Add("pageSize", request.pageSize.ToString());
                dic.Add("parentId", request.parentId.ToString());
                dic.Add("catClass", request.catClass.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                /*
                 {"success":true,"resultMessage":"","resultCode":"0000","result":{"categorys":
                    [
                        {"catId":5272,"parentId":0,"name":"数字内容","catClass":0,"state":1},
                        {"catId":9987,"parentId":0,"name":"手机","catClass":0,"state":1},
                        {"catId":13887,"parentId":0,"name":"邮币","catClass":0,"state":1},
                        {"catId":13996,"parentId":0,"name":"IP","catClass":0,"state":1},
                        {"catId":1713,"parentId":0,"name":"图书","catClass":0,"state":1}
                    ],
                    "totalRows":79,"pageNo":1,"pageSize":5}}
                 */
                var result = ConvertJsonToResult<CategorysResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 3.17 同类商品查询
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async virtual Task<List<SimilarProduct>> GetSimilarSku(string token, string skuId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETSIMILARSKU_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("skuId", skuId);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<SimilarProduct>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 4.1 获取一级地址
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <returns></returns>
        public async virtual Task<ProvinceResult> GetProvince(string token)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETPROVINCE_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<ProvinceResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 4.2 获取二级地址
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="provinceId">必须	一级地址</param>
        /// <returns></returns>
        public async virtual Task<CityResult> GetCity(string token,int provinceId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETCITY_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("id", provinceId.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<CityResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 4.3 获取三级地址
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="cityId">必须	二级地址</param>
        /// <returns></returns>
        public async virtual Task<CountyResult> GetCounty(string token, int cityId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETCOUNTY_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("id", cityId.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<CountyResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 4.4 获取四级地址
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="countyId">必须	三级地址</param>
        /// <returns></returns>
        public async virtual Task<TownResult> GetTown(string token, int countyId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETTOWN_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("id", countyId.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<TownResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 4.5 验证四级地址是否正确
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<CheckAreaResult> CheckArea(CheckAreaRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.CHECKAREA_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                dic.Add("provinceId", request.provinceId.ToString());
                dic.Add("cityId", request.cityId.ToString());
                dic.Add("countyId", request.countyId.ToString());
                dic.Add("townId", request.townId.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<CheckAreaResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 5.1 批量查询京东价价格
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="skus">商品编号，请以，分割。例如：J_129408,J_129409(最高支持100个商品)</param>
        /// <returns></returns>
        public async virtual Task<List<JDPriceResult>> GetJdPrice(string token, string skus)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETJDPRICE_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("sku", skus);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<JDPriceResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 5.2 批量查询协议价价格
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="skus">商品编号，请以，分割。例如：J_129408,J_129409(最高支持100个商品)</param>
        /// <returns></returns>
        public async virtual Task<List<PriceResult>> GetPrice(string token, string skus)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETPRICE_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("sku", skus);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<PriceResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 5.3 批量查询商品售卖价
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="skus">商品编号，请以，分割。例如：J_129408,J_129409(最高支持100个商品)</param>
        /// <returns></returns>
        public async virtual Task<List<SellPriceResult>> GetSellPrice(string token, string skus)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETSELLPRICE_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("sku", skus);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<SellPriceResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 6.2 批量获取库存接口（建议订单详情页、下单使用）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<List<NewStockByIdResult>> GetNewStockById(NewStockByIdRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETNEWSTOCKBYID_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                dic.Add("skuNums", StringHelper.ToJson(request.skuNums));
                dic.Add("area", request.area);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<NewStockByIdResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 6.3 批量获取库存接口(建议商品列表页使用)
        /// </summary>
        /// <param name="token"> 授权时获取的access token</param>
        /// <param name="sku">商品编号 批量以逗号分隔  (最高支持100个商品)</param>
        /// <param name="area">格式：1_0_0 (分别代表1、2、3级地址)</param>
        /// <returns></returns>
        public async virtual Task<List<StockByIdResult>> GetStockById(string token,string sku, string area)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETSTOCKBYID_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("sku", sku);
                dic.Add("area", area);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<StockByIdResult>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 7.1 统一下单接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.SUBMITORDER_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                dic.Add("thirdOrder", request.thirdOrder);
                if(request.sku!= null && request.sku.Any())
                {
                    var skuJson = StringHelper.ToJson(request.sku);
                    dic.Add("sku", skuJson);
                }
                else
                {
                    throw new ArgumentNullException(nameof(request.sku));
                }
                
                dic.Add("name", request.name);
                dic.Add("province", request.province.ToString());
                dic.Add("city", request.city.ToString());
                dic.Add("county", request.county.ToString());
                dic.Add("town", request.town.ToString());
                dic.Add("address", request.address);
                dic.Add("zip", request.zip);
                dic.Add("phone", request.phone);
                dic.Add("mobile", request.mobile);
                dic.Add("email", request.email);
                dic.Add("remark", request.remark);
                dic.Add("invoiceState", request.invoiceState.ToString());
                dic.Add("invoiceType", request.invoiceType.ToString());
                dic.Add("selectedInvoiceTitle", request.selectedInvoiceTitle.ToString());
                dic.Add("companyName", request.companyName);
                dic.Add("invoiceContent", request.invoiceContent.ToString());
                dic.Add("paymentType", request.paymentType.ToString());
                dic.Add("isUseBalance", request.isUseBalance.ToString());
                dic.Add("submitState", request.submitState.ToString());
                dic.Add("invoiceName", request.invoiceName);
                dic.Add("invoicePhone", request.invoicePhone);
                dic.Add("invoiceProvice", request.invoiceProvice.ToString());
                dic.Add("invoiceCity", request.invoiceCity.ToString());
                dic.Add("invoiceCounty", request.invoiceCounty.ToString());
                dic.Add("invoiceAddress", request.invoiceAddress);
                dic.Add("doOrderPriceMode", request.doOrderPriceMode.ToString());
                if (request.orderPriceSnap != null && request.orderPriceSnap.Any())
                {
                    var orderPriceSnap = StringHelper.ToJson(request.orderPriceSnap);
                    dic.Add("orderPriceSnap", orderPriceSnap);
                }
                else
                {
                    throw new ArgumentNullException(nameof(request.orderPriceSnap));
                }
                dic.Add("reservingDate", request.reservingDate.ToString());
                dic.Add("installDate", request.installDate.ToString());
                dic.Add("needInstall", request.needInstall.ToString());
                dic.Add("promiseDate", request.promiseDate);
                dic.Add("promiseTimeRange", request.promiseTimeRange);
                dic.Add("promiseTimeRangeCode", request.promiseTimeRangeCode.ToString());

                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<SubmitOrderResult>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 7.2 确认预占库存订单接口
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="jdOrderId">京东的订单单号(调用7.1接口时返回的父订单号)</param>
        /// <returns></returns>
        public async virtual Task<bool> ConfirmOrder(string token, string jdOrderId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.CONFIRMORDER_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("jdOrderId", jdOrderId);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return false;
                }
                var result = ConvertJsonToResult<bool>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 7.3 取消未确认订单接口
        /// 注意事项
        ///该接口仅能取消未确认的预占库存父订单单号。不能取消已经确认的订单单号。
        ///如果需要取消已确认的订单，可以调用取消订单接口进行取消操作，参数必须为子订单才能取消 。
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="jdOrderId">京东的订单单号(调用7.1接口时返回的父订单号)</param>
        /// <returns></returns>
        public async virtual Task<bool> Cancel(string token, string jdOrderId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.CANCEL_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("jdOrderId", jdOrderId);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return false;
                }
                var result = ConvertJsonToResult<bool>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 7.4 发起支付接口
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="jdOrderId">京东的订单单号(调用7.1接口时返回的父订单号)</param>
        /// <returns></returns>
        public async virtual Task<bool> DoPay(string token, string jdOrderId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.DOPAY_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("jdOrderId", jdOrderId);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return false;
                }
                var cc = StringHelper.GetValueByKeyFromJson("success", x);
                if ((bool)cc)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 7.6 订单反查接口
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="thirdOrder">客户系统订单号</param>
        /// <returns></returns>
        public async virtual Task<string> SelectJdOrderIdByThirdOrder(string token, string thirdOrder)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.SELECTJDORDERIDBYTHIRDORDER_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("thirdOrder", thirdOrder);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return x;
                }
                var result = ConvertJsonToResult<string>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 7.7 查询京东订单信息接口
        /// </summary>
        /// <param name="token"> 授权时获取的access token</param>
        /// <param name="jdOrderId">京东订单号</param>
        /// <returns></returns>
        public async virtual Task<SelectJdOrderResult> SelectJdOrder(string token, string jdOrderId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.SELECTJDORDER_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("jdOrderId", jdOrderId);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<SelectJdOrderResult>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 7.8 查询配送信息接口
        /// </summary>
        /// <param name="token"> 授权时获取的access token</param>
        /// <param name="jdOrderId">京东订单号</param>
        /// <returns></returns>
        public async virtual Task<OrderTrackResult> OrderTrack(string token, string jdOrderId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.ORDERTRACK_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("jdOrderId", jdOrderId);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<OrderTrackResult>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 7.9 统一余额查询接口
        /// </summary>
        /// <param name="token">授权时获取的access token</param>
        /// <param name="payType">支付类型 4：余额 7：网银钱包 101：金采支付</param>
        /// <returns></returns>
        public async virtual Task<decimal> GetBalance(string token, int payType)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.ORDERTRACK_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("payType", payType.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return 0m;
                }
                var result = ConvertJsonToResult<decimal>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 7.11 余额明细查询接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<BalanceDetailResult> GetBalanceDetail(BalanceDetailRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETBALANCEDETAIL_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                dic.Add("pageNum", request.pageNum.ToString());
                dic.Add("pageSize", request.pageSize.ToString());
                dic.Add("orderId", request.orderId);
                dic.Add("startDate", request.startDate);
                dic.Add("endDate", request.endDate);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<BalanceDetailResult>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 8.1 新建订单查询接口
        /// </summary>
        /// <param name="token">必须	授权时获取的access token</param>
        /// <param name="date">必须	2013-11-7 （不包含当天）</param>
        /// <param name="page">必须	当前页码</param>
        /// <returns></returns>
        public async virtual Task<CheckNewOrderResult> CheckNewOrder(string token,string date,int page)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.CHECKNEWORDER_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("date", date);
                dic.Add("page", page.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<CheckNewOrderResult>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 8.2 获取妥投订单接口
        /// </summary>
        /// <param name="token">必须	授权时获取的access token</param>
        /// <param name="date">必须	2013-11-7 （不包含当天）</param>
        /// <param name="page">必须	当前页码</param>
        /// <returns></returns>
        public async virtual Task<CheckDlokOrderResult> CheckDlokOrder(string token, string date, int page)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.CHECKDLOKORDER_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("date", date);
                dic.Add("page", page.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<CheckDlokOrderResult>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 8.3 获取拒收消息接口
        /// </summary>
        /// <param name="token">必须	授权时获取的access token</param>
        /// <param name="date">必须	2013-11-7 （不包含当天）</param>
        /// <param name="page">必须	当前页码</param>
        /// <returns></returns>
        public async virtual Task<CheckDlokOrderResult> CheckRefuseOrder(string token, string date, int page)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.CHECKREFUSEORDER_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("date", date);
                dic.Add("page", page.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<CheckDlokOrderResult>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 9.1 信息推送接口
        /// </summary>
        /// <param name="token">必须	授权时获取的access token</param>
        /// <param name="type">非必须	推送类型，支持多个组合，例如 1,2,3</param>
        /// <returns></returns>
        public async virtual Task<List<MessageResult>> Get(string token, string type="")
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GET_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("date", type);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<MessageResult>>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 9.2 根据推送id，删除推送信息接口
        /// </summary>
        /// <param name="token">必须	授权时获取的access token</param>
        /// <param name="id">必须	上一接口获取的id</param>
        /// <returns></returns>
        public async virtual Task<bool> Del(string token, long id)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.DEL_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("id", id.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return false;
                }
                var result = ConvertJsonToResult<bool>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 10.2 填写客户发运信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<dynamic> UpdateSendSku(UpdateSendSkuRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.UPDATESENDSKU_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("afsServiceId", request.afsServiceId.ToString());
                dic.Add("freightMoney", request.freightMoney.ToString());
                dic.Add("expressCompany", request.expressCompany);
                dic.Add("deliverDate", request.deliverDate);
                dic.Add("expressCode", request.expressCode);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<dynamic>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 10.3 校验某订单中某商品是否可以提交售后服务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async virtual Task<int> GetAvailableNumberComp(GetAvailableNumberCompRequestDto request)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETAVAILABLENUMBERCOMP_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", request.token);
                dic.Add("param", StringHelper.ToJson(request.param));
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return 0;
                }
                var result = ConvertJsonToResult<int>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 10.4 根据订单号、商品编号查询支持的服务类型
        /// </summary>
        /// <param name="jdOrderId">必需 订单号</param>
        /// <param name="skuId">必需 商品编号</param>
        /// <returns></returns>
        public async virtual Task<List<ComponentExport>> GetCustomerExpectComp(long jdOrderId, long skuId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETCUSTOMEREXPECTCOMP_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("jdOrderId", jdOrderId.ToString());
                dic.Add("skuId", skuId.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<ComponentExport>>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 10.5 根据订单号、商品编号查询支持的商品返回京东方式
        /// </summary>
        /// <param name="jdOrderId">必需 订单号</param>
        /// <param name="skuId">必需 商品编号</param>
        /// <returns></returns>
        public async virtual Task<List<ComponentExport>> GetWareReturnJdComp(long jdOrderId, long skuId)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETWARERETURNJDCOMP_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("jdOrderId", jdOrderId.ToString());
                dic.Add("skuId", skuId.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<List<ComponentExport>>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 10.6 根据客户账号和订单号分页查询服务单概要信息
        /// </summary>
        /// <param name="jdOrderId">订单号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码 1代表第一页</param>
        /// <returns></returns>
        public async virtual Task<AfsServicebyCustomerPinPage> GetServiceListPage(long jdOrderId, int pageSize,int pageIndex)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETWARERETURNJDCOMP_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("jdOrderId", jdOrderId.ToString());
                dic.Add("pageSize", pageSize.ToString());
                dic.Add("pageIndex", pageIndex.ToString());
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<AfsServicebyCustomerPinPage>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 10.7 根据服务单号查询服务单明细信息
        /// </summary>
        /// <param name="afsServiceId">服务单号 需要调用10.6 查询得到服务单号</param>
        /// <param name="appendInfoSteps"></param>
        /// <returns></returns>
        public async virtual Task<CompatibleServiceDetailDto> GetServiceDetailInfo(long afsServiceId, List<int> appendInfoSteps)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETSERVICEDETAILINFO_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("afsServiceId", afsServiceId.ToString());
                dic.Add("appendInfoSteps", StringHelper.ToJson(appendInfoSteps));
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return null;
                }
                var result = ConvertJsonToResult<CompatibleServiceDetailDto>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 10.8 取消服务单/客户放弃
        /// </summary>
        /// <param name="serviceIdList">服务单号集合param>
        /// <param name="approveNotes">审核意见</param>
        /// <returns></returns>
        public async virtual Task<bool> AuditCancel(List<int> serviceIdList, string approveNotes)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.AUDITCANCEL_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("serviceIdList", StringHelper.ToJson(serviceIdList));
                dic.Add("approveNotes", approveNotes);
                var x = await HttpHelper.HttpClientPost(url, dic);
                if (string.IsNullOrWhiteSpace(x))
                {
                    return false;
                }
                var result = ConvertJsonToResult<bool>(x);
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #region 公共方法，可以override
        public virtual BaseResult<T> ConvertJsonToBaseResult<T>(string jsonStr)
        {
            var result = StringHelper.JsonToObj<BaseResult<T>>(jsonStr)
                    as BaseResult<T>;
            return result;
        }


        public virtual T ConvertJsonToResult<T>(string jsonStr)
        {
            var baseResult = StringHelper.JsonToObj<BaseResult<T>>(jsonStr)
                    as BaseResult<T>;

            if (baseResult.Success)
            {
                if (baseResult.Result != null)
                {
                    return baseResult.Result;
                }
            }
            return default(T);
        }

        #endregion
    }
}
