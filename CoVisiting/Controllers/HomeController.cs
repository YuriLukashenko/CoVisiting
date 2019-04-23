using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;
using Microsoft.AspNetCore.Mvc;
using CoVisiting.Models;
using CoVisiting.Models.Category;
using CoVisiting.Models.Event;
using CoVisiting.Models.Home;

namespace CoVisiting.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEvent _eventService;

        public HomeController(IEvent eventService)
        {
            _eventService = eventService;
        }

        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();
            return View(model);
        }

        private HomeIndexModel BuildHomeIndexModel()
        {
            var latestEvents = _eventService.GetLatestEvents(12);
            var events = latestEvents.Select(newEvent => new EventListingModel
            {
                Id = newEvent.Id,
                Category = GetCategoryListingForEvent(newEvent),
                Title = newEvent.Title,
                EventRating = newEvent.EventRating,
                EventCity = newEvent.EventCity,
                EventPlace = newEvent.EventPlace,
                StartDateTime = newEvent.StartDateTime,
                AuthorId = newEvent.Author.Id,
                AuthorName = newEvent.Author.UserName,
                AuthorRating = newEvent.Author.Rating,
                RepliesCount = newEvent.Replies.Count()
            });

            return new HomeIndexModel()
            {
                LatestEvents = events,
                SearchQuery = ""
            };
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
