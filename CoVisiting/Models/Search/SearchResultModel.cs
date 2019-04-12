using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Models.Event;

namespace CoVisiting.Models.Search
{
    public class SearchResultModel
    {
        public IEnumerable<EventListingModel> Events { get; set; }
        public string SearchQuery { get; set; }
        public bool IsEmptySearchResults { get; set; }
    }
}
