using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSBTickets.Models;
using RestSharp;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;

namespace DSBTickets.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Journey> result = new List<Journey>();
            Tuple<int, List<Journey>, bool, CookieContainer> t = getSearchId();
            result.AddRange(t.Item2);

            if (t.Item3)
                result.AddRange(getJourneys(t.Item1, t.Item4));
            result = result.OrderBy(j => j.lowestPrice).ToList();
            return View(result);
        }

        //public static HashSet<Journey> getTickets()
        //{
        //    RestClient client = new RestClient("https://www.dsb.dk");
        //    RestRequest req1 = new RestRequest("api/netbutik/search");
        //    req1.Method = Method.POST;
        //    req1.AddParameter("criteria[0][Direction]", 1)
        //        .AddParameter("criteria[0][DepartLocation]", "Aarhus H")
        //        .AddParameter("criteria[0][ArriveLocation]", "København H")
        //        .AddParameter("criteria[0][SearchDate]", "2018-03-14")
        //        .AddParameter("criteria[0][SearchType]", 7)
        //        .AddParameter("criteria[0][PassengersAdults]", 1);

        //    HashSet<Journey> journeys = new HashSet<Journey>();
        //    for (int i = 12; i < 15; i++)
        //    {
        //        var c = client.Execute(req1);

        //        var content1 = c.Content;

        //        RootObject root = JsonConvert.DeserializeObject<RootObject>(content1.Substring(1, content1.Length - 2));

        //        Console.WriteLine("TIME: " + i + ":00:00");
        //        foreach (Journey j in root.journeys)
        //        {
        //            journeys.Add(j);
        //        }
        //    }
        //    return journeys;
        //}

        public static Tuple<int, List<Journey>, bool, CookieContainer> getSearchId()
        {
            CookieContainer cc = new CookieContainer();
            RestClient client = new RestClient("https://www.dsb.dk");
            client.CookieContainer = cc;
            RestRequest req1 = new RestRequest("api/netbutik/search");
            req1.Method = Method.POST;
            req1.AddParameter("criteria[0][Direction]", 1)
                .AddParameter("criteria[0][DepartLocation]", "Aarhus H")
                .AddParameter("criteria[0][ArriveLocation]", "København H")
                .AddParameter("criteria[0][SearchDate]", "2018-04-1")
                .AddParameter("criteria[0][SearchType]", 7)
                .AddParameter("criteria[0][PassengersAdults]", 1)
                .AddParameter("criteria[0][SearchTime]", "04:00:00");
                        
            var c = client.Execute(req1);
            var content1 = c.Content;

            RootObject root = JsonConvert.DeserializeObject<RootObject>(content1.Substring(1, content1.Length - 2));
                            
            return new Tuple<int, List<Journey>, bool, CookieContainer>(root.journeys[0].searchId, root.journeys, root.canSearchLater, cc);
        }

        public static List<Journey> getJourneys(int searchId, CookieContainer cc)
        {
            RestClient client = new RestClient("https://www.dsb.dk");
            client.CookieContainer = cc;
            RestRequest req1 = new RestRequest("api/netbutik/journeys/"+searchId+"/later");
            req1.Method = Method.GET;

            List<Journey> journeys = new List<Journey>();
            RootObject root;
            
            do
            {
            var c = client.Execute(req1);

            var content1 = c.Content;

            root = JsonConvert.DeserializeObject<RootObject>(content1);

            foreach (Journey j in root.journeys)
            {
                journeys.Add(j);
            }                
            } while (root.canSearchLater);
            
            return journeys;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}