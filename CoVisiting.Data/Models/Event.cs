﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoVisiting.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EventCity { get; set; }
        public string EventPlace { get; set; }
        public DateTime StartDateTime { get; set; }

        public DateTime Created { get; set; }
        public string Content { get; set; }

        public bool isBeforeAfter { get; set; }
        public Moving BeforeEvent { get; set; }
        public Moving AfterEvent { get; set; }
        
        public virtual ApplicationUser Author { get; set; }
        public virtual Category Category { get; set; }

        public virtual IEnumerable<EventReply> Replies { get; set; }
        public virtual IEnumerable<UserEventJoinTable> Subscribers { get; set; }

        public string EventImageUrl { get; set; }
    }

    
}
