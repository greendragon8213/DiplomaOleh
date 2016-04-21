using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser_Google1
{
    public class Result
    {
        public string GsearchResultClass { get; set; }
        public string unescapedUrl { get; set; }
        public string url { get; set; }
        public string visibleUrl { get; set; }
        public string cacheUrl { get; set; }
        public string title { get; set; }
        public string titleNoFormatting { get; set; }
        public string content { get; set; }
    }
}
