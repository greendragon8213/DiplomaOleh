namespace DeepSearching.Models
{
    public class EstimatedUrl
    {
        public EstimatedUrl(string url, string title, int similarityCoeffitient)
        {
            Url = url;
            Title = title;
            SimilarityCoeffitient = similarityCoeffitient;
        }

        public string Url { get; set; }
        public string Title { get; set; }
        public int SimilarityCoeffitient { get; set; }


    }
}
