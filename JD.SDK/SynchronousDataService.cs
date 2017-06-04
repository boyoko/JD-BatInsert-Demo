using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JD.Common;

namespace JD.SDK
{
    public class SynchronousDataService
    {
        private string GetMethodName(string message = "",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            //Console.WriteLine(memberName);
            return memberName;
        }
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
        public async virtual Task<List<GetCommentSummarysResult>> GetCommentSummarys(string token, string skuIds)
        {
            try
            {
                var url = Path.Combine(ConstHelper.BASEURL, ConstHelper.GETCOMMENTSUMMARYS_URI);
                IDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("token", token);
                dic.Add("sku", skuIds);
                var x = await HttpHelper.HttpClientPost(url, dic);
                var result = ConvertJsonToResult<List<GetCommentSummarysResult>>(x);
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
        public async virtual Task<CategorysResult> GetCategorys(GetCategorysRequestDto request)
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
                var result = ConvertJsonToResult<List<SimilarProduct>>(x);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

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
    }
}
