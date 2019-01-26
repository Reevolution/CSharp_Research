using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUrl
{
    public class BaseUrlInfo
    {
        public string Url { get; set; }
        public int UrlCount { get; set; }

        public override string ToString()
        {
            return $"Url {Url}, Url count {UrlCount}";
        }
    }
}
