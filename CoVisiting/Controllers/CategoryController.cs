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
                    Description = category.Description
                });

            var model = new CategoryIndexModel()
            {
                CategoryList = categories
            };

            return View(model);
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var category = _categoryService.GetById(id);
            //var events = new List<Event>();

            //events = _eventService.GetFilteredEvents(category, searchQuery).ToList();
            var events = category.Events;

            var eventsListings = events.Select(newEvent => new EventListingModel
            {
                Id = newEvent.Id,
                AuthorId = newEvent.User.Id,
                AuthorRating = newEvent.User.Rating,
                AuthorName = newEvent.User.UserName,
                Title = newEvent.Title,
                EventCity = newEvent.EventCity,
                StartDateTime = newEvent.StartDateTime,
                RepliesCount = newEvent.Replies.Count(),
                Category = BuildCategoryListing(newEvent)
            });

            var model = new CategoryTopicModel
            {
                Events = eventsListings,
                Category = BuildCategoryListing(category)
            };

            return View(model);
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