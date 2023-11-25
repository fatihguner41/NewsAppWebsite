using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using NewsAppWebsite.Interfaces;
using NewsAppWebsite.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
                lastNewsDate=IncrementDate(lastNewsDate);
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
                if(newsDict != null)
                {
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


                }
                
            }

            return (lastNewsDate,news);
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

        private string IncrementDate(string date = "")
        {

            // String'i DateTime nesnesine çevir
            DateTime tarih;
            string tarihString = "";
            if (date != null)
            {

                date = date.Replace("2023", "23");
                date.Replace(".", "/");
            }
            Console.WriteLine(date);
            if (DateTime.TryParseExact(date, "yy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out tarih))
            {
                // Geçerli bir tarih
                // Tarihi bir saniye ileri al
                tarih = tarih.AddSeconds(-1);

                // Sonucu yazdır
                Console.WriteLine("Eski Tarih: " + date);
                
                tarihString = tarih.ToString("yy/MM/dd HH:mm:ss");
                tarihString=tarihString.Replace(".", "/");
                Console.WriteLine("Yeni Tarih: " + tarihString);
                return tarihString;
            }
            else
            {
                // Geçersiz tarih formatı
                Console.WriteLine("Geçersiz tarih formatı: " + date);
            }
            return date;
        }


        void UpdateNewsDates()
        {
            string yearPrefix = "2023";

            QueryBuilder query = QueryBuilder.New()
                .OrderBy("haber_date")
                .StartAt(yearPrefix)
                .EndAt(yearPrefix + "\uf8ff");

            FirebaseResponse response = _firebaseService.FirebaseClient.Get("haberler", query);

            if (response.Body != "null")
            {
                Dictionary<string, News> newsDict = JsonConvert.DeserializeObject<Dictionary<string, News>>(response.Body.ToString());

                foreach (var newsEntry in newsDict)
                {
                    var news = newsEntry.Value;
                    if (news.haber_date.StartsWith(yearPrefix))
                    {
                        string newDate = "23" + news.haber_date.Substring(4);
                        news.haber_date = newDate;

                        // Haber güncelle
                        _firebaseService.FirebaseClient.Set($"haberler/{newsEntry.Key}", news);
                        Console.WriteLine($"Haber güncellendi: {newsEntry.Key} - {news.haber_date}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Düğüm boş.");
            }
        }

    }
}