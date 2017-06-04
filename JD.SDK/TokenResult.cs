using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class TokenResult
    {
        public string uid { get; set; }
        public long refresh_token_expires { get; set; }
        public long time { get; set; }
        public long expires_in { get; set; }
        public string refresh_token { get; set; }
        public string access_token { get; set; }
    }
}
