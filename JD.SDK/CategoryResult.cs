using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.SDK
{
    public class CategoryResult
    {
        public int catId { get; set; }
        public int parentId { get; set; }
        public string name { get; set; }
        public int catClass { get; set; }
        public int state { get; set; }
    }
}
