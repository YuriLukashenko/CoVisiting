using System;
using System.Collections.Generic;
using System.Text;
using CoVisiting.Data.Enums;

namespace CoVisiting.Data.Models
{
    public class Moving
    {
        public int Id { get; set; }
        public TransportType TransportType { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
