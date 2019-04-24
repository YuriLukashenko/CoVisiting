using System;
using System.Collections.Generic;
using System.Text;
using CoVisiting.Data.Enums;

namespace CoVisiting.Data.Models
{
    public class EventReply
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public ReplyScope ReplyScope { get; set; }

        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Reciever { get; set; }
        public virtual Event Event { get; set; }
    }
}
