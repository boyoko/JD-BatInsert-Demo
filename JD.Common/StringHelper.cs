using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
    }
}
