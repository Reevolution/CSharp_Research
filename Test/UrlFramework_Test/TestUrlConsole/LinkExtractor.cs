using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestUrlConsole
{
    public class LinkExtractor
    {
        public static List<string> Extract(string html)
        {
            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (Match m in linkParser.Matches(html))
            {
                Console.WriteLine(m.Value);
            }

            return new List<string>() ;
        }
    }
}
