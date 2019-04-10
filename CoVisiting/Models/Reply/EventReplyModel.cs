using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoVisiting.Models.Reply
{
    public class EventReplyModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImageUrl { get; set; }
        public int AuthorRating { get; set; }
        public DateTime Created { get; set; }

        public string Content { get; set; }
    }
}
