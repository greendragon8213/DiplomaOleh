using System.Collections.Generic;
using System.Linq;
using DeepSearching.Models;
using Services;

namespace DeepSearching
{
    //like the Worker is a controller
    class Worker
    {
        private WebPageService _webPageService;
        public List<EstimatedUrl> Find(string inputUrl, int searchDepthCoeff)
        {
            _webPageService = new WebPageService(inputUrl);

            _webPageService.GetChildPages(searchDepthCoeff);

            HashSet<EstimatedUrl> resultEstimatedUrls = _webPageService.MakeEstimationForChildPages();

            return resultEstimatedUrls.OrderBy(x => x.SimilarityCoeffitient).ToList();
        }
    }
}
