using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace DeepSearching.Models
{
    public class WebPage
    {
        public WebPage(string pageUrl)
        {
            Url = pageUrl;
            
            HtmlDocument document = new HtmlWeb().Load(pageUrl);

            Title = document.DocumentNode.SelectSingleNode("//title").InnerText;
            
            string textContent = document.DocumentNode.Descendants().Where
                (n => n.NodeType == HtmlNodeType.Text 
                && !string.IsNullOrWhiteSpace(n.InnerText) 
                && n.ParentNode.Name != "script" 
                && n.ParentNode.Name != "style")
                    .Aggregate(String.Empty, (current, n) => string.Concat(current, " ", n.InnerText));

            Text = textContent;
        }
        public string Url { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}
