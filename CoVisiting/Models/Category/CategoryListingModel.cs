using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoVisiting.Models.Category
{
    public class CategoryListingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int EventsCount { get; set; }
        public int UsersCount { get; set; }
    } 
}
