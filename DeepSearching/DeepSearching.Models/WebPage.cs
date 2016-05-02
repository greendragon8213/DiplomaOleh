using System;
using System.Text.RegularExpressions;

namespace DeepSearching.Models
{
    public class WebPage
    {
        public string Url { get; set; }

        private string _title = string.Empty;

        public string Title
        {
            get
            {
                if (String.IsNullOrEmpty(_title) && (AllContent!=null)) 
                    _title = Regex.Match(AllContent, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", 
                        RegexOptions.IgnoreCase).Groups["Title"].Value;
                return _title;
            }
        }
        public string AllContent { get; set; }

        public string Text
        {
            //ToDo extract text from allContent
            get
            {
                return AllContent;
            }
        }
    }
}
