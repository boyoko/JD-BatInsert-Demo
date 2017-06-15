using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class CheckAreaResult
    {
        public bool success { get; set; }
        public int resultCode { get; set; }
        public int addressId { get; set; }
        public string message { get; set; }
    }
}
