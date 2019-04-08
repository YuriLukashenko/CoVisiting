using System;
using System.Collections.Generic;
using System.Text;

namespace CoVisiting.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EventCity { get; set; }
        public DateTime StartDateTime { get; set; }

        public DateTime Created { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }

        //Before event
        public TransportType TransportTypeBefore { get; set; }
        public string DepartureCityBefore { get; set; }
        public string ArrivalCityBefore { get; set; }
        public DateTime DepartureTimeBefore { get; set; }
        public DateTime ArrivalTimeBefore { get; set; }

        //After Event
        public TransportType TransportTypeAfter { get; set; }
        public string DepartureCityAfter { get; set; }
        public string ArrivalCityAfter { get; set; }
        public DateTime DepartureTimeAfter { get; set; }
        public DateTime ArrivalTimeAfter { get; set; }
        
        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }

        public virtual IEnumerable<EventReply> Replies { get; set; }
    }
}
