using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace TicketsCore.Controllers
{
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {
        private List<Ticket> _tickets = new List<Ticket>();
        private int _searchId;
        private bool _canSearchLater = true;
        private CookieContainer _cookies = new CookieContainer();

        [HttpGet("[action]")]
        public IEnumerable<Ticket> GetTickets(string from, string to, string date)
        {
            //var from = Uri.EscapeDataString("Aarhus H");
            //var to = Uri.EscapeDataString("Hurup Thy");
            //var date = Uri.EscapeDataString(ChangeDateTimeFormat("03-08-2018"));

            string json = GetJsonFromLocationsAndDate(from, to, date);

            DeserializeJsonToTickets(json);
            
            do
            {
                GetLaterTicketsFromSearchId(_searchId);
            }
            while (_canSearchLater);

            return _tickets.OrderBy(t => t.numberShifts).OrderBy(t => t.journeyTime).OrderBy(t => t.lowestPrice);
        }

        private void GetLaterTicketsFromSearchId(int searchId)
        {
            var client = new RestClient("https://www.dsb.dk/api/netbutik/journeys/" + searchId + "/later");
            client.CookieContainer = _cookies;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = client.Execute(request);
            DeserializeJsonToTickets("["+ response.Content + "]");
        }

        private string GetJsonFromLocationsAndDate(string from, string to, string date)
        {
            var body = $"criteria%5B0%5D%5BDirection%5D=1&" +
                $"criteria%5B0%5D%5BDepartLocation%5D={Uri.EscapeDataString(from)}&" +
                $"criteria%5B0%5D%5BArriveLocation%5D={Uri.EscapeDataString(to)}&" +
                $"criteria%5B0%5D%5BSearchDate%5D={ChangeDateTimeFormat(date)}T00%3A00%3A00&" +
                $"criteria%5B0%5D%5BSearchTime%5D=04%3A00%3A00&" +
                $"criteria%5B0%5D%5BType%5D=0&" +
                $"criteria%5B0%5D%5BSearchType%5D=1&" +
                $"criteria%5B0%5D%5BSeatReservations%5D=0&" +
                $"criteria%5B0%5D%5BPassengersAdults%5D=1";

            var client = new RestClient("https://www.dsb.dk/api/netbutik/search");
            client.CookieContainer = _cookies;
            var request = new RestRequest(Method.POST);
            request.AddParameter("text/xml", body, ParameterType.RequestBody);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        private void DeserializeJsonToTickets(string json)
        {
            var tickets = JsonConvert.DeserializeObject<List<Tickets>>(json);
            foreach (Ticket t in tickets.First().journeys)
            {
                _tickets.Add(t);
            }
            if (_canSearchLater = tickets.Last().canSearchLater)
            {
                _searchId = tickets.Last().journeys.Last().searchId;
            }            
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

        public class Ticket
        {
            public int searchId { get; set; }
            public double lowestPrice { get; set; }
            public int numberShifts { get; set; }
            public DateTime arrivalDate { get; set; }
            public DateTime departureDate { get; set; }
            public int journeyTime { get; set; }
        }

        public class Tickets
        {
            public List<Ticket> journeys { get; set; }
            public bool canSearchLater { get; set; }
        }
    }
}
