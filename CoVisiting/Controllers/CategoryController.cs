﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;
using CoVisiting.Models.Category;
using CoVisiting.Models.Event;
using Microsoft.AspNetCore.Mvc;

namespace CoVisiting.Controllers
{
    public class CategoryController : Controller
    {
        public readonly ICategory _categoryService;
        public readonly IEvent _eventService;
   
        public CategoryController(ICategory categoryService, IEvent eventService)
        {
            _categoryService = categoryService;
            _eventService = eventService;
        }

        public IActionResult Index()
        {
            IEnumerable<CategoryListingModel> categories = _categoryService.GetAll()
                .Select(category => new CategoryListingModel
                {
                    Id = category.Id,
                    Name = category.Title,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    EventsCount = category.Events.Count(),
                    UsersCount = GetUsersCount(category.Events.ToList())
                });

            var model = new CategoryIndexModel()
            {
                CategoryList = categories
            };

            return View(model);
        }

        private int GetUsersCount(List<Event> categoryEvents)
        {
            List<ApplicationUser> allSubscribers = new List<ApplicationUser>(); 
            foreach (var newEvent in categoryEvents)
            {
                foreach (var subscriber in newEvent.Subscribers.ToList())
                {
                    if (!allSubscribers.Contains(subscriber.ApplicationUser))
                    {
                        allSubscribers.Add(subscriber.ApplicationUser);
                    }
                }
            }

            return allSubscribers.Count();
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var category = _categoryService.GetById(id);
            var events = new List<Event>();

            events = _eventService.GetFilteredEvents(category, searchQuery).ToList();

            var eventsListings = events.Select(newEvent => new EventListingModel
            {
                Id = newEvent.Id,
                AuthorId = newEvent.Author.Id,
                AuthorRating = newEvent.Author.Rating,
                AuthorName = newEvent.Author.UserName,
                Title = newEvent.Title,
                EventCity = newEvent.EventCity,
                EventPlace = newEvent.EventPlace,
                EventSubscribersCount = newEvent.Subscribers.Count(),
                StartDateTime = newEvent.StartDateTime,
                RepliesCount = newEvent.Replies.Count(),
                Category = BuildCategoryListing(newEvent),
                EventImageUrl = newEvent.EventImageUrl
            });

            var model = new CategoryTopicModel
            {
                Events = eventsListings,
                Category = BuildCategoryListing(category)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        private CategoryListingModel BuildCategoryListing(Event newEvent)
        {
            var category = newEvent.Category;
            return BuildCategoryListing(category);
        }

        private CategoryListingModel BuildCategoryListing(Category category)
        {
            return new CategoryListingModel
            {
                Id = category.Id,
                Name = category.Title,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };
        }
    }
}