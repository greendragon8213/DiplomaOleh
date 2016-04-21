using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser_Google1
{
    public class Cursor
    {
        public string resultCount { get; set; }
        public Page[] pages { get; set; }
        public string estimatedResultCount { get; set; }
        public int currentPageIndex { get; set; }
        public string moreResultsUrl { get; set; }
        public string searchResultTime { get; set; }
    }
}
