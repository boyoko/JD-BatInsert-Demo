using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JD.Common
{
    public class HttpHelper
    {
        public async static Task<string> HttpClientPost(string url, IDictionary<string, string> dic)
        {
            try
            {
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                //创建HttpClient（注意传入HttpClientHandler）  
                //using (var http = new HttpClient(handler))
                //http://blog.devscrum.net/index.php/2014/05/11/building-a-transient-retry-handler-for-the-net-httpclient/
                using (var http = new HttpClient(new RetryDelegatingHandler(new HttpClientHandler()), false))
                {
                    http.DefaultRequestHeaders.Connection.Add("keep-alive");
                    var content = new FormUrlEncodedContent(dic);
                    http.Timeout = TimeSpan.FromSeconds(200);
                    //await异步等待回应  
                    var response = await http.PostAsync(url, content);
                    //确保HTTP成功状态值  
                    response.EnsureSuccessStatusCode();
                    var msg = await response.Content.ReadAsStringAsync();
                    return msg;
                }
            }
            catch (Exception)
            {
                return null;
            }


        }


        public async static Task<string> HttpClientGet(string url, string sku)
        {
            url = "http://localhost:1754/api/product/GetDetail/";
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };
            using (var httpclient = new HttpClient(handler))
            {
                httpclient.BaseAddress = new Uri(url);
                httpclient.DefaultRequestHeaders.Accept.Clear();
                httpclient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await httpclient.GetAsync("?sku=" + sku);

                if (response.IsSuccessStatusCode)
                {
                    Stream myResponseStream = await response.Content.ReadAsStreamAsync();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                    string retString = myStreamReader.ReadToEnd();
                    myStreamReader.Dispose();
                    myResponseStream.Dispose();

                    return retString;
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
        }
    }
}
