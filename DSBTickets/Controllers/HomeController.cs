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
using System.Globalization;

namespace DSBTickets.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public static Tuple<int, List<Journey>, bool, CookieContainer> getSearchId(string from, string to, string date)
        {
            CookieContainer cc = new CookieContainer();
            RestClient client = new RestClient("https://www.dsb.dk");
            client.CookieContainer = cc;
            RestRequest req1 = new RestRequest("api/netbutik/search");
            req1.Method = Method.POST;
            req1.AddParameter("criteria[0][Direction]", 1)
                .AddParameter("criteria[0][DepartLocation]", from)
                .AddParameter("criteria[0][ArriveLocation]", to)
                .AddParameter("criteria[0][SearchDate]", date)
                .AddParameter("criteria[0][SearchType]", 7)
                .AddParameter("criteria[0][PassengersAdults]", 1)
                .AddParameter("criteria[0][SearchTime]", "04:00:00");
                        
            var c = client.Execute(req1);
            var content = c.Content;

            if (c.StatusCode == HttpStatusCode.OK)
            {
                RootObject root = JsonConvert.DeserializeObject<RootObject>(content.Substring(1, content.Length - 2));
                if (root.journeys.Count > 0)
                {
                    return new Tuple<int, List<Journey>, bool, CookieContainer>(root.journeys[0].searchId, root.journeys, root.canSearchLater, cc);
                }                
            }
            return new Tuple<int, List<Journey>, bool, CookieContainer>(0, null, false, null);
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

        public ActionResult UpdateTickets(string from, string to, string date)
        {            
            List<Journey> result = new List<Journey>();
            Tuple<int, List<Journey>, bool, CookieContainer> t = getSearchId(from, to, ChangeDateTimeFormat(date));
            if(t.Item2 != null) result.AddRange(t.Item2);

            if (t.Item3)
                result.AddRange(getJourneys(t.Item1, t.Item4));
            result = result.OrderBy(j => j.lowestPrice).ToList();
            return View(result);
        }

        public static string ChangeDateTimeFormat(string date)
        {
            DateTime dt;
            DateTime.TryParseExact(date,
                                    "dd-MM-yyyy",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out dt);
            return dt.ToString("yyyy-MM-dd");
        }
    }
}