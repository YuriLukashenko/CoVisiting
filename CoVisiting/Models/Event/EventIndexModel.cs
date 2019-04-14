using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Models;
using CoVisiting.Models.Reply;

namespace CoVisiting.Models.Event
{
    public class EventIndexModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EventCity { get; set; }
        public string EventPlace { get; set; }
        public DateTime StartDateTime { get; set; }

        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImageUrl { get; set; }
        public int AuthorRating { get; set; }
        public DateTime Created { get; set; }

        public string Content { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Moving BeforeEvent { get; set; }
        public Moving AfterEvent { get; set; }

        public IEnumerable<EventReplyModel> Replies { get; set; }

    }
}
