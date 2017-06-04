using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace JD.Common
{
    public class CacheHelper
    {
        private static readonly ConnectionMultiplexer _redis
            = ConnectionMultiplexer.Connect("123.57.21.63:6379,password=tfhtfhtfh,allowAdmin=true");

        private static readonly int _databaseNumber = 3;
        private static object _asyncState = null;

        private static IDatabase _db
        {
            get
            {
                var db = _redis.GetDatabase(_databaseNumber, _asyncState);
                return db;
            }
        }


        public static async Task<string> Get(string key)
        {
            string value = await _db.StringGetAsync(key);
            return value;
        }

        public static async Task<bool> Set(string key, string value, TimeSpan? expiry = null)
        {
            if (expiry == null)
            {
                expiry = TimeSpan.FromDays(1);
            }
            return await _db.StringSetAsync(key, value, expiry);
        }

        public static async Task SetList<T>(string key, List<T> value, TimeSpan? expiry = null)
        {
            if (expiry == null)
            {
                expiry = TimeSpan.FromDays(1);
            }
            foreach (var single in value)
            {
                var s = JsonConvert.SerializeObject(single); //序列化
                await _db.ListRightPushAsync(key, s); //要一个个的插入
            }
        }

        public List<T> ListGet<T>(string key)
        {
            //ListRange返回的是一组字符串对象
            //需要逐个反序列化成实体
            var vList = _db.ListRange(key);
            List<T> result = new List<T>();
            foreach (var item in vList)
            {
                var model = JsonConvert.DeserializeObject<T>(item); //反序列化
                result.Add(model);
            }
            return result;
        }

        public static async Task<bool> Remove(string key, string value)
        {
            return await _db.SetRemoveAsync(key, value);
        }

        /// <summary>
        /// 从缓存获取token
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetToken()
        {
            return await _db.StringGetAsync(ConstHelper.ACCESS_TOKEN);
        }
    }
}
