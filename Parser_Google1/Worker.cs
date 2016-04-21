using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Text.RegularExpressions;
using System.IO;


namespace Parser_Google1
{
    class Worker
    {
        // Summary:
        //     Gets child URLs by url.
        //
        // Returns:
        //     List of result URL.
        public List<string> GetChildRefByURL(string url)
        {
            //ToDo checking literals!!!
            //ToDo change List<string> to HashSet<string>!!!!!
            List<string> resultURLs = new List<string>();
            
            string content = new WebClient().DownloadString(url);

            //ToDo use smth inside Regular expresion
            string pattern = @"<\s*a\s+href\s*=\s*" + "\"" + @"[\w:/.\d-_%]+";
            Regex regex = new Regex(pattern);
            foreach (Match match in regex.Matches(content))
            {
                resultURLs.Add(match.ToString());
            }

            //corecting results
            for (int i = 0; i < resultURLs.Count;i++)
            {
                resultURLs[i] = Regex.Replace(resultURLs[i], @"<\s*a\s+href\s*=\s*" + "\"", string.Empty);
                if (resultURLs[i].Contains("../")||!resultURLs[i].Contains("http://"))
                    resultURLs[i] = RelativeToAbsolutePath(resultURLs[i], url);
            }
           
            return resultURLs.Distinct().ToList();
        }

        public void GetContentByURL(string url)
        {
            string content = new WebClient().DownloadString(url);
            //get json by url
            //get body from json
        }

        private string RelativeToAbsolutePath(string relativeUrl, string currentURL)
        {
            string fileName = Path.GetFileName(relativeUrl);
            string newDir = currentURL;
            if (!relativeUrl.Contains("http://"))
                newDir = currentURL.Remove(currentURL.LastIndexOf(Path.GetFileName(currentURL)), Path.GetFileName(currentURL).Length);
            while (relativeUrl.Contains("../"))
            {
                relativeUrl = relativeUrl.Remove(relativeUrl.IndexOf("../"), 3);
                newDir = GetParentDirectoryPath(newDir);
            }
            return newDir+fileName;
        }

        private string GetParentDirectoryPath(string url)
        {            
            string fn =  Path.GetFileName(url);
            string dn = url.Remove(url.LastIndexOf(fn),fn.Length);//url - fn;//Path.GetDirectoryName(url);
            string result = dn;//url -fn
            if (dn == "")
                dn = "http://";
            Match match = Regex.Matches(dn, @"[-_\w]+", RegexOptions.RightToLeft)[0];
            result = result.Remove(match.Index-1, match.Length);
            return result;
        }
    }
}
