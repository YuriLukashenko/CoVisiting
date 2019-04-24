using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Enums;
using CoVisiting.Data.Models;
using CoVisiting.Models.Event;
using CoVisiting.Models.Reply;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoVisiting.Controllers
{
    public class EventController : Controller
    {
        private readonly IEvent _eventService;
        private readonly ICategory _categoryService;

        private static UserManager<ApplicationUser> _userManager;

        public EventController(IEvent eventService, ICategory categoryService, UserManager<ApplicationUser> userManager)
        {
            _eventService = eventService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            var newEvent = _eventService.GetById(id);

            var replies = BuildEventReplies(newEvent.Replies);


            var model = new EventIndexModel
            {
                Id = newEvent.Id,
                Title = newEvent.Title,
                EventCity = newEvent.EventCity,
                EventPlace = newEvent.EventPlace,
                StartDateTime = newEvent.StartDateTime,
                AuthorId = newEvent.Author.Id,
                AuthorName = newEvent.Author.UserName,
                AuthorImageUrl = newEvent.Author.ProfileImageUrl,
                AuthorRating = newEvent.Author.Rating,
                Created = newEvent.Created,
                Content = newEvent.Content,
                CategoryId = newEvent.Category.Id,
                CategoryName = newEvent.Category.Title,
                BeforeEvent = newEvent.BeforeEvent,
                AfterEvent = newEvent.AfterEvent,
                Replies = replies
            };

            return View(model);
        }

        public IActionResult Create(int id)
        {
            var category = _categoryService.GetById(id);

            var model = new NewEventModel()
            {
                CategoryId = category.Id,
                CategoryName = category.Title,
                CategoryImageUrl = category.ImageUrl,
                AuthorName = User.Identity.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewEventModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            var newEvent = BuildEvent(model, user);

            _eventService.Add(newEvent).Wait(); //block the current thread until the task is complete

            //TODO: Implement Author Rating Management

            return RedirectToAction("Index", "Event", new { id = newEvent.Id });
        }

        private Event BuildEvent(NewEventModel model, ApplicationUser user)
        {
           var category = _categoryService.GetById(model.CategoryId);
           return new Event()
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                EventCity = model.EventCity,
                EventPlace = model.EventPlace,
                StartDateTime = model.StartDateTime,
                Author = user,
                Category = category,
                BeforeEvent = new Moving()
                {
                    TransportType = model.TransportTypeBefore,
                    DepartureCity = model.DepartureCityBefore,
                    DepartureTime = model.DepartureTimeBefore,
                    ArrivalCity = model.ArrivalCityBefore,
                    ArrivalTime = model.ArrivalTimeBefore
                },
                AfterEvent = new Moving()
                {
                    TransportType = model.TransportTypeBefore,
                    DepartureCity = model.DepartureCityBefore,
                    DepartureTime = model.DepartureTimeBefore,
                    ArrivalCity = model.ArrivalCityBefore,
                    ArrivalTime = model.ArrivalTimeBefore
                }
            };
        }


        private IEnumerable<EventReplyModel> BuildEventReplies(IEnumerable<EventReply> replies)
        {
            return replies.Select(reply => new EventReplyModel()
            {
                Id = reply.Id,
                AuthorId = reply.User.Id,
                AuthorName = reply.User.UserName,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                Content = reply.Content,
                //todo implement
                IsVisible = true
            });
        }
    }
}