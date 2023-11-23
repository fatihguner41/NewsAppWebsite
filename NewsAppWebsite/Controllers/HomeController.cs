using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using NewsAppWebsite.Interfaces;
using NewsAppWebsite.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

namespace NewsAppWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFirebaseService _firebaseService;
        private readonly int getNewsAmount = 250;

        public HomeController(ILogger<HomeController> logger,IFirebaseService firebaseService)
        {
            _logger = logger;
            _firebaseService = firebaseService;
        }

        public  IActionResult  IndexAsync()
        {

            IndexViewModel indexViewModel = new IndexViewModel("home","home");
            return View(indexViewModel);
        }

        public IActionResult Category(string category)
        {
            IndexViewModel indexViewModel = new IndexViewModel("category", category);
            return View("index",indexViewModel);
        }

        public IActionResult Newspaper(string newspaper)
        {
           
            IndexViewModel indexViewModel = new IndexViewModel("newspaper", newspaper);
            // Burada gazeteye göre işlemleri gerçekleştirin
            return View("index",indexViewModel);
        }

        public ActionResult GetMoreNews(string type,string name,string lastNewsDate="")
        {
            name = System.Web.HttpUtility.HtmlDecode(name);
            Dictionary<string, News> news= new Dictionary<string, News>();
            int amount = getNewsAmount;
            while(news.Count < 10 && amount<8000)
            {

                (lastNewsDate, news) = GetNewsFromFirebase(type, name, lastNewsDate, news,amount);
                amount = amount * 2;
            }

            NewsPartialViewModel partialViewModel = new NewsPartialViewModel(news);
            return PartialView("_NewsPartial", partialViewModel);
            
            
        }

        private (string ,Dictionary<string, News>) GetNewsFromFirebase(string type, string name, string lastNewsDate, Dictionary<string, News> news,int amount)
        {
            QueryBuilder query = QueryBuilder.New();

            Console.WriteLine(type+ name+ lastNewsDate+"----"+amount);
            if (lastNewsDate == "")
            {
                query = query.OrderBy("haber_date").LimitToLast(amount);

            }
            else
            {
                query = query
            .OrderBy("haber_date")
            .EndAt(lastNewsDate)
            .LimitToLast(amount); // İlk bin haber için
            }



            FirebaseResponse response = _firebaseService.FirebaseClient.Get("haberler", query);

            Dictionary<String, News> newsDict = new Dictionary<string, News>();
            Dictionary<string, News> filteredNewsDict = new Dictionary<string, News>();

            if (response.Body != "null")
            {
                newsDict = JsonConvert.DeserializeObject<Dictionary<String, News>>(response.Body.ToString());
                var selectedNews = newsDict.Values.OrderBy(news => news.haber_date).Reverse();

                lastNewsDate = selectedNews.Last().haber_date;

                filteredNewsDict = selectedNews.ToDictionary(news => news.haber_link);
                filteredNewsDict = GetFilteredNews(type, name, filteredNewsDict);


                // dictionary2 içindeki çiftleri birleşikDictionary'e ekleyin
                foreach (var pair in filteredNewsDict)
                {
                    // Aynı anahtar değerine sahip bir çift dictionary1'de ise silin
                    if (!news.ContainsKey(pair.Key))
                    {

                        news[pair.Key] = pair.Value;
                    }

                }


                return (lastNewsDate,news);
            }

            return ("",null);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private Dictionary<string, News> GetFilteredNews(string type,string name, Dictionary<string, News> news)
        {
            if (type == "category")
            {
                // news adlı Dictionary'den haber_type değeri name'e eşit olanları al
                var selectedNews = news.Where(pair => pair.Value.haber_type == name)
                                       .ToDictionary(pair => pair.Key, pair => pair.Value);
                return selectedNews;

            }
            else if(type == "newspaper")
            {
                // news adlı Dictionary'den haber_type değeri name'e eşit olanları al
                var selectedNews = news.Where(pair => pair.Value.haber_source == name)
                                       .ToDictionary(pair => pair.Key, pair => pair.Value);
                return selectedNews;
            }
            return news;
        }
    }
}