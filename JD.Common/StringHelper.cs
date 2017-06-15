using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JD.Common
{
    public class StringHelper
    {
        public static string ToJson<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }

        public static T JsonToObj<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        public static JObject GetJObject(string jsonStr)
        {
            JObject jo = JObject.Parse(jsonStr);
            return jo;
        }

        public static string[] GetValuesFromJObject(JObject jo)
        {
            string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();
            return values;
        }

        public static string[] GetValuesFromJson(string jsonStr)
        {
            JObject jo = JObject.Parse(jsonStr);
            var values = jo.Properties().Select(item =>item.Value.ToString()).ToArray();
            return values;
        }

        public static dynamic GetValueByKeyFromJson(string key,string jsonStr)
        {
            JObject jo = JObject.Parse(jsonStr);
            IEnumerable<JProperty> props = jo.Properties();
            JProperty p = props.FirstOrDefault(a => a.Name == key);
            JToken v = p.Value;
            return v;
        }
    }
}
