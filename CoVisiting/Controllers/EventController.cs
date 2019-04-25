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

        private ApplicationUser GetUserFromClaimsPrincipal()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return new ApplicationUser();
            return _userManager.FindByIdAsync(userId).Result;
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
                Replies = replies,
                Subscribers = newEvent.Subscribers,
                IsCurrentUserSubscribed = newEvent.Subscribers.Contains(GetUserFromClaimsPrincipal())
                
            };

            return View(model);
        }

        public IActionResult ShowHide(int id, bool state)
        {
            return RedirectToAction("Create", "Event", new { id, state });
        }

        public IActionResult Create(int id, bool state = false)
        {
            var category = _categoryService.GetById(id);

            var model = new NewEventModel()
            {
                CategoryId = category.Id,
                CategoryName = category.Title,
                CategoryImageUrl = category.ImageUrl,
                AuthorName = User.Identity.Name,
                isBeforeAfter = state
            };

            return View(model);
        }

        public async Task<IActionResult> AddSubscriber(int id)
        {
            ApplicationUser user = GetUserFromClaimsPrincipal();

            await _eventService.AddEventSubscriber(id, user);

            return RedirectToAction("Index", "Event", new { id });
        }

        public async Task <IActionResult> DeleteSubscriber(int id)
        {
            ApplicationUser user = GetUserFromClaimsPrincipal();

            await _eventService.DeleteEventSubscriber(id, user);

            return RedirectToAction("Index", "Event", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewEventModel model)
        {
            ApplicationUser user = GetUserFromClaimsPrincipal();

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
                },
                Subscribers = new List<ApplicationUser>()
                {
                    GetUserFromClaimsPrincipal()     
                }
            };
        }

        private IEnumerable<EventReplyModel> BuildEventReplies(IEnumerable<EventReply> replies)
        {
            return replies.Select(reply => new EventReplyModel()
            {
                Id = reply.Id,
                AuthorId = reply.Sender.Id,
                AuthorName = reply.Sender.UserName,
                AuthorImageUrl = reply.Sender.ProfileImageUrl,
                AuthorRating = reply.Sender.Rating,
                Created = reply.Created,
                Content = reply.Content,
                IsVisible = CheckReplyVisibility(reply)
            });
        }

        private bool CheckReplyVisibility(EventReply reply)
        {
            ApplicationUser user = GetUserFromClaimsPrincipal();

            //self check
            if (user == reply.Sender) return true;

            //check roles
            if (reply.ReplyScope == ReplyScope.ForAll)
            {
                return true;
            }
            else if (reply.ReplyScope == ReplyScope.ForSubscribers)
            {
                if (reply.Event.Subscribers.Contains(user)) return true;
                else return false;
            }
            else if (reply.ReplyScope == ReplyScope.ForAuthor)
            {
                if (user == reply.Reciever) return true;
                else return false;
            }

            return false;
        }
    }
}