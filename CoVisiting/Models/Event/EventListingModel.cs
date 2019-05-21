using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Models.Category;

namespace CoVisiting.Models.Event
{
    public class EventListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int AuthorRating { get; set; }
        public string AuthorId { get; set; }
        public string EventCity { get; set; }
        public string EventPlace { get; set; }
        public int EventSubscribersCount { get; set; }
        public DateTime StartDateTime { get; set; }

        public CategoryListingModel Category { get; set; }

        public int RepliesCount { get; set; }
        public string EventImageUrl { get; set; }
    }
}
