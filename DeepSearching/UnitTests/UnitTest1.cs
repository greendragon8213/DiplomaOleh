using System.Collections.Generic;
using DeepSearching.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace UnitTests
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void GetChildPages_ValidInput_SpetialResultExpected()
        {
            //arrange
            WebPageService serv = new WebPageService("http://button.dekel.ru/");

            HashSet<string> expectation = new HashSet<string>()
            {
                "http://dekel.ru",
                "http://button.dekel.ru/adv.html",
                "http://www.facebook.com/sharer.php",
                "http://vkontakte.ru/share.php",
                "http://twitter.com/share",
                "http://dekel.ru/banners",
                "http://dekel.ru/context",
                "http://dekel.ru/polygraphy",
                "http://dekel.ru/portfolio/sites",
                "http://dekel.ru/portfolio/banners",
                "http://dekel.ru/documentation",
                "http://dekel.ru/contacts",
                "http://dekel.ru/universal-template",
                "http://dekel.ru/site-vizitka",
                "http://dekel.ru/mini-site",
                "http://dekel.ru/individual-site",
                "http://dekel.ru/docs/brief.pdf",
                "http://dekel.ru/mailto:web",
                "http://dekel.ru/docs/domain.pdf",
                "http://dekel.ru/password-generator"
            };

            //act
            HashSet<WebPage> realitywWebPages = serv.GetChildPages(2);

            HashSet<string> reality = new HashSet<string>();
            foreach (var item in realitywWebPages)
            {
                reality.Add(item.Url);
            }

            //assert
            Assert.AreEqual(reality.SetEquals(expectation), true);

        }
    }
}
