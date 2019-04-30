using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;
using CoVisiting.Models.Category;
using CoVisiting.Models.Event;
using CoVisiting.Models.Search;
using Microsoft.AspNetCore.Mvc;

namespace CoVisiting.Controllers
{
    public class SearchController : Controller
    {
        public readonly IEvent _eventService;

        public SearchController(IEvent eventService)
        {
            _eventService = eventService;
        }

        public IActionResult Results(string searchQuery)
        {
            var events = _eventService.GetFilteredEvents(searchQuery).ToList();
            var areNoResults = (!String.IsNullOrEmpty(searchQuery) && !events.Any());

            var eventListings = events.Select(newEvent => new EventListingModel
            {
                Id = newEvent.Id,
                Category = GetCategoryListingForEvent(newEvent),
                Title = newEvent.Title,
                EventSubscribersCount = newEvent.Subscribers.Count(),
                EventCity = newEvent.EventCity,
                EventPlace = newEvent.EventPlace,
                StartDateTime = newEvent.StartDateTime,
                AuthorId = newEvent.Author.Id,
                AuthorName = newEvent.Author.UserName,
                AuthorRating = newEvent.Author.Rating,
                RepliesCount = newEvent.Replies.Count()
            });

            var model = new SearchResultModel
            {
                Events = eventListings,
                SearchQuery = searchQuery,
                IsEmptySearchResults = areNoResults
            };

            return View(model);
        }

        private CategoryListingModel GetCategoryListingForEvent(Event newEvent)
        {
            var category = newEvent.Category;

            return new CategoryListingModel()
            {
                Id = category.Id,
                ImageUrl = category.ImageUrl
            };
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Results", new { searchQuery });
        }
    }
}