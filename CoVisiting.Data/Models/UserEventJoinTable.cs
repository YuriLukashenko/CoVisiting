using System;
using System.Collections.Generic;
using System.Text;

namespace CoVisiting.Data.Models
{
    public class UserEventJoinTable
    {
        public int EventId { get; set; }
        public Event Event { get; set; }

        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
