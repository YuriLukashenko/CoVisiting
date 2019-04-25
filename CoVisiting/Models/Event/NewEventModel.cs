using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Enums;
using CoVisiting.Data.Models;

namespace CoVisiting.Models.Event
{
    public class NewEventModel
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string AuthorName { get; set; }
        public string CategoryImageUrl { get; set; }
        public string Title { get; set; }
        public string EventCity { get; set; }
        public string EventPlace { get; set; }
        public DateTime StartDateTime { get; set; }

        public string Content { get; set; }
        public bool isBeforeAfter { get; set; }

        public TransportType TransportTypeBefore { get; set; }
        public string DepartureCityBefore { get; set; }
        public string ArrivalCityBefore { get; set; }
        public DateTime DepartureTimeBefore { get; set; }
        public DateTime ArrivalTimeBefore { get; set; }

        public TransportType TransportTypeAfter { get; set; }
        public string DepartureCityAfter { get; set; }
        public string ArrivalCityAfter { get; set; }
        public DateTime DepartureTimeAfter { get; set; }
        public DateTime ArrivalTimeAfter { get; set; }
    }
}
