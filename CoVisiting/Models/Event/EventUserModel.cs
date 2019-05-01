using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoVisiting.Models.Event
{
    public class EventUserModel
    {
        public IEnumerable<EventListingModel> Events { get; set; }
        public string UserName { get; set; }
    }
}
