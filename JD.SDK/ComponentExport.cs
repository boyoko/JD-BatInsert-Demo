using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class ComponentExport
    {
        /// <summary>
        /// 服务类型码 退货(10)、换货(20)、维修(30)、上门取件(4)、客户发货(40)、客户送货(7)
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 服务类型名称 退货、换货、维修、上门取件、客户发货、客户送货
        /// </summary>
        public string name { get; set; }
    }
}
