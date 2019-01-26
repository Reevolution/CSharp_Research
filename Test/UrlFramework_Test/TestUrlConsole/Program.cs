using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestUrlConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example usage:
            WebClient client = new WebClient();
            var buffer = client.DownloadString("https://stackoverflow.com/questions/599275/how-can-i-download-html-source-in-c-sharp");

            // GetString() extension method is from:
            // http://www.shrinkrays.net/code-snippets/csharp/an-extension-method-for-converting-a-byte-array-to-a-string.aspx
            string html = buffer;
            List<string> list = LinkExtractor.Extract(html);
            foreach (var link in list)
            {
                Console.WriteLine(link);
            }

            Console.ReadLine();

            //using (var client = new WebClient())
            //{
            //    var htmlSource = client.DownloadString("https://stackoverflow.com/questions/599275/how-can-i-download-html-source-in-c-sharp");
            //    foreach (var item in GetLinksFromWebsite(htmlSource))
            //    {
            //        // TODO: you could easily write a recursive function
            //        // that will call itself here and retrieve the respective contents
            //        // of the site ...
            //        Console.WriteLine(item);
            //    }
            //}

            Console.ReadLine();
        }
    }
}
