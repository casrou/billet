using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSBTickets.Models
{
    public class TransportType
    {
        //public int id { get; set; }
        //public int transportTypeFamily { get; set; }
        //public string iconCssClass { get; set; }
    }

    public class Stretch
    {
        //public int id { get; set; }
        //public string lineName { get; set; }
        //public TransportType transportType { get; set; }
        //public string departLocation { get; set; }
        //public DateTime departureDate { get; set; }
        //public bool departureLocationIsInTS { get; set; }
        //public string arriveLocation { get; set; }
        //public DateTime arrivalDate { get; set; }
        //public bool arrivalLocationIsInTS { get; set; }
        //public bool isToFromBornholm { get; set; }
        //public int status { get; set; }
    }

    public class StretchInfo
    {
        //public int __invalid_name__10 { get; set; }
        //public int? __invalid_name__20 { get; set; }
        //public int? __invalid_name__30 { get; set; }
    }

    public class TicketDetail
    {
        //public int type { get; set; }
        //public string zones { get; set; }
        //public string validInMinutes { get; set; }
        //public bool immediatelyIsAvailable { get; set; }
    }

    public class TicketInstance
    {
        //public int journeyId { get; set; }
        //public string code { get; set; }
        //public string ticketName { get; set; }
        //public string seatZoneName { get; set; }
        //public int ticketClass { get; set; }
        //public int sortIndex { get; set; }
        //public bool isAvailable { get; set; }
        //public bool isMainProduct { get; set; }
        //public bool isVisible { get; set; }
        //public double seatReservationPrice { get; set; }
        //public double ticketPrice { get; set; }
        //public string ticketCode { get; set; }
        //public bool isSeatsSoldOut { get; set; }
        //public bool isReservationRequired { get; set; }
        //public bool hasTickets { get; set; }
        //public bool hasSeatReservations { get; set; }
        //public List<object> passengers { get; set; }
        //public int seatReservations { get; set; }
        //public string seatReservationCode { get; set; }
        //public List<object> seatingGroups { get; set; }
        //public List<object> productNotifications { get; set; }
        //public StretchInfo stretchInfo { get; set; }
        //public TicketDetail ticketDetail { get; set; }
    }

    public class Journey
    {
        //public List<Stretch> stretches { get; set; }
        //public List<TicketInstance> ticketInstances { get; set; }
        //public List<object> himMessages { get; set; }
        //public int id { get; set; }
        public int searchId { get; set; }
        public double lowestPrice { get; set; }
        //public int numberShifts { get; set; }
        public DateTime arrivalDate { get; set; }
        public DateTime departureDate { get; set; }
        //public int status { get; set; }
        //public int journeyTime { get; set; }
        //public double ticketPrice { get; set; }
        //public double seatReservationPrice { get; set; }
        //public bool isAvailable { get; set; }
        //public List<object> productNotifications { get; set; }

        //public override bool Equals(object obj)
        //{
        //    Journey q = obj as Journey;
        //    return q != null && q.departureDate == this.departureDate && q.arrivalDate == this.arrivalDate && q.lowestPrice == this.lowestPrice;
        //}

        //public override int GetHashCode()
        //{
        //    return this.departureDate.GetHashCode() ^ this.arrivalDate.GetHashCode() ^ this.lowestPrice.GetHashCode();
        //}
    }

    public class RootObject
    {
        //public int direction { get; set; }
        public List<Journey> journeys { get; set; }
        //public bool canSearchEarlier { get; set; }
        public bool canSearchLater { get; set; }
        //public int searchType { get; set; }
    }
}