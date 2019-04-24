using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Enums;

namespace CoVisiting.Models.Reply
{
    public class NewReplyModel
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }

        public string EventName { get; set; }
        public int EventId { get; set; }

        public string UserName { get; set; }
        public string Content { get; set; }
        public ReplyScope ReplyScope { get; set; }

        public Data.Models.ApplicationUser Reciever { get; set; }

    }
}
