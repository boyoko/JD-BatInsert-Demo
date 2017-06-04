using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class BaseResult<T>
    {
        public bool Success { get; set; }
        public string ResultMessage { get; set; }
        public string ResultCode { get; set; }
        public T Result { get; set; }
    }
}
