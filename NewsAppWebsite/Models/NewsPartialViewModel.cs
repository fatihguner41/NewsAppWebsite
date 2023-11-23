namespace NewsAppWebsite.Models
{
    public class NewsPartialViewModel
    {
        public Dictionary<String, News> newsDict { get; set; }
        
        public NewsPartialViewModel(Dictionary<String, News> newsDict)
        {
            this.newsDict = newsDict;
        }
    }
}
