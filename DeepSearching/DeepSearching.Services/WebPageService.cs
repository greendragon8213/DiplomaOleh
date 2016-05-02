using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeepSearching.Algorithms;
using DeepSearching.Models;
using HtmlAgilityPack;

namespace DeepSearching.Services
{
    public class WebPageService
    {
            public WebPage CurrentPage { get; set; }

            private HashSet<WebPage> _childPages = new HashSet<WebPage>();

            public WebPageService(string pageUrl)
            {
                CurrentPage = new WebPage(pageUrl);
            }
            public HashSet<EstimatedUrl> MakeEstimationForChildPages()
            {
                if (_childPages.Count == 0)
                    GetChildPages(1);

                var result = new HashSet<EstimatedUrl>();

                foreach (var page in _childPages)
                {
                    result.Add(new EstimatedUrl(page.Url, page.Title,
                        Algorithm.Calculate(CurrentPage.Text, page.Text)
                        ));
                }

                return result;
            }

            public HashSet<WebPage> GetChildPages(int searchDepthPoint)
            {
                var wps = new HashSet<WebPage>();
                _childPages = GetChildPages(CurrentPage, ref searchDepthPoint, ref wps);

                #region Timer
                _stopWatch.Stop();
                TimeSpan ts = _stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}.{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
                Console.WriteLine("______________________________");
                Console.WriteLine("RunTime: " + elapsedTime);
                Console.WriteLine("______________________________");
                #endregion

                return _childPages;
            }

            #region Helpers
            private readonly HashSet<string> _currentWebPageChildren = new HashSet<string>();
            private Stopwatch _stopWatch = new Stopwatch();
            private HashSet<WebPage> GetChildPages(WebPage inputPage, ref int depthPointsLeft, ref HashSet<WebPage> resultPages)
            {
                if (depthPointsLeft == 2)
                    _stopWatch.Start();

                #region Getting all urls in current depth

                HashSet<string> refUrls = GetAllRefInUrl(inputPage.Url);
                HashSet<WebPage> webPagesInCurrentDepth = new HashSet<WebPage>();

                #region Compare gotten urls (in current depth) with _currentWebPageChildren urls
                foreach (var currentChildUrl in _currentWebPageChildren)
                {
                    refUrls.Remove(currentChildUrl);
                }

                foreach (var url in refUrls)
                {
                    _currentWebPageChildren.Add(url);
                }
                #endregion

                #region getting WebPages fron distinced urls
                foreach (var url in refUrls)
                {
                    try
                    {
                        webPagesInCurrentDepth.Add(new WebPage(url));
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(url + " //Some error in this url");
                        //Log.Add
                    }
                }
                #endregion
                #endregion

                if (depthPointsLeft > 0)
                {
                    //Console.WriteLine(depthPointsLeft);

                    depthPointsLeft -= 1;

                    foreach (var webPage in webPagesInCurrentDepth)
                    {
                        //Console.WriteLine(webPage.Url);

                        var localResult = new HashSet<WebPage>(webPagesInCurrentDepth);
                        var childPages = GetChildPages(webPage, ref depthPointsLeft,
                            ref localResult);

                        foreach (var page in childPages)
                        {
                            resultPages.Add(page);
                        }
                    }

                    //Console.WriteLine("_________");
                }
                return resultPages;
            }

            public static HashSet<string> GetAllRefInUrl(string url)
            {
                HashSet<string> resultURLs = new HashSet<string>();
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(url);

                var res = document.DocumentNode.Descendants("a");//SelectNodes("//a");
                foreach (var item in res)
                {
                    string urlToAdd = item.Attributes["href"].Value;
                    if (!urlToAdd.Contains("http://") && !urlToAdd.Contains("https://") || (urlToAdd.Contains("../")))
                        urlToAdd = RelativeToAbsolutePath(urlToAdd, url);

                    resultURLs.Add(urlToAdd);
                }

                return resultURLs;
            }

            private static string RelativeToAbsolutePath(string relativeUrl, string currentURL)
            {
                string fileName = Path.GetFileName(relativeUrl);
                string newDir = currentURL;

                while (relativeUrl.Contains("../"))
                {
                    relativeUrl = relativeUrl.Remove(relativeUrl.IndexOf("../"), 3);
                    newDir = GetParentDirectoryPath(newDir);
                }

                if (!relativeUrl.Contains("http://") && !relativeUrl.Contains("https://"))
                {
                    if ((relativeUrl[0] != '/') && (currentURL[currentURL.Length - 1] != '/'))
                    {
                        newDir += "/" + relativeUrl;
                    }
                    else
                    {
                        newDir += relativeUrl;
                    }
                }
                else
                {
                    newDir += "/" + fileName;
                }
                //newDir = currentURL.Remove(currentURL.LastIndexOf(Path.GetFileName(currentURL)), Path.GetFileName(currentURL).Length);

                return newDir;
            }

            private static string GetParentDirectoryPath(string url)
            {
                string fn = Path.GetFileName(url);
                string dn = url.Remove(url.LastIndexOf(fn), fn.Length);//url - fn;//Path.GetDirectoryName(url);
                string result = dn;//url -fn
                if (dn == "")
                    dn = "http://";
                Match match = Regex.Matches(dn, @"[-_\w]+", RegexOptions.RightToLeft)[0];
                result = result.Remove(match.Index - 1, match.Length);
                return result;
            }
            #endregion
        }
    }
