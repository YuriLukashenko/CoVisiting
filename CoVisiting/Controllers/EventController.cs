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
using CoVisiting.Models.Category;

namespace CoVisiting.Controllers
{
    public class EventController : Controller
    {
        private readonly IEvent _eventService;
        private readonly ICategory _categoryService;
        private readonly IApplicationUser _applicationService;

        private static UserManager<ApplicationUser> _userManager;

        public EventController(IEvent eventService, ICategory categoryService, IApplicationUser applicationService, UserManager<ApplicationUser> userManager)
        {
            _eventService = eventService;
            _categoryService = categoryService;
            _applicationService = applicationService;
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

            foreach(var subscriber in newEvent.Subscribers)
            {
                if (subscriber.ApplicationUser == null)
                    subscriber.ApplicationUser = _userManager.FindByIdAsync(subscriber.UserId).Result;
            }

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
                IsBeforeAfter = CheckBeforeAfterVisibility(newEvent),
                BeforeEvent = newEvent.BeforeEvent,
                AfterEvent = newEvent.AfterEvent,
                Replies = replies,
                Subscribers = newEvent.Subscribers,
                IsCurrentUserSubscribed = (newEvent.Subscribers == null || newEvent.Subscribers.Count() == 0)
                                          ? false : newEvent.Subscribers.FirstOrDefault(t => t.UserId == _userManager.GetUserId(User)) == null
                                          ? false : true 
            };

            return View(model);
        }

        private bool CheckBeforeAfterVisibility(Event newEvent)
        {
            if (String.IsNullOrEmpty(newEvent.BeforeEvent.DepartureCity) &&
                String.IsNullOrEmpty(newEvent.BeforeEvent.ArrivalCity) &&
                String.IsNullOrEmpty(newEvent.AfterEvent.DepartureCity) &&
                String.IsNullOrEmpty(newEvent.AfterEvent.ArrivalCity))
            {
                return false;
            }
            else return true;
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
                isBeforeAfter = state,
                StartDateTime = DateTime.Today,
                DepartureTimeBefore = DateTime.Today,
                DepartureTimeAfter = DateTime.Today,
                ArrivalTimeBefore = DateTime.Today,
                ArrivalTimeAfter = DateTime.Today
            };

            return View(model);
        }

        public IActionResult UsersEventsListing(string userId)
        {
            var eventsByUser = _eventService.GetEventsByUser(userId);

            var userEventsListing = eventsByUser.Select(newEvent => new EventListingModel
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

            var model = new EventUserModel
            {
                Events = userEventsListing,
                UserName = _userManager.FindByIdAsync(userId).Result.UserName
        };

            return View(model);
        }


        public async Task<IActionResult> Delete(int eventId, int categoryId)
        {

            //-5 to user rating by creating new event, -10 foreach subs
            _applicationService.DecrementRating(_userManager.GetUserId(User), 5 ).Wait();
            var deletedEvent = _eventService.GetById(eventId);
            foreach (var subsriber in deletedEvent.Subscribers)
            {
                _applicationService.DecrementRating(_userManager.GetUserId(User), 10).Wait();
            }

            await _eventService.Delete(eventId);
            int id = categoryId;
            return RedirectToAction("Topic", "Category", new { id });
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

        public async Task<IActionResult> AddSubscriber(int id)
        {
            ApplicationUser user = GetUserFromClaimsPrincipal();
            
            await _eventService.AddEventSubscriber(id, user);

            //increment author rating +10
            var updatingEvent = _eventService.GetById(id);
            _applicationService.IncrementRating(updatingEvent.Author.Id, 10).Wait();

            return RedirectToAction("Index", "Event", new { id });
        }

        public async Task <IActionResult> DeleteSubscriber(int id)
        {
            ApplicationUser user = GetUserFromClaimsPrincipal();

            await _eventService.DeleteEventSubscriber(id, user);

            //decrement author rating -10
            var updatingEvent = _eventService.GetById(id);
            _applicationService.DecrementRating(updatingEvent.Author.Id, 10).Wait();

            return RedirectToAction("Index", "Event", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewEventModel model)
        {
            ApplicationUser user = GetUserFromClaimsPrincipal();

            var newEvent = BuildEvent(model, user);

            _eventService.Add(newEvent).Wait(); //block the current thread until the task is complete

            var updateEvent = _eventService.GetLastEvent();

            _eventService.AddEventSubscriber(updateEvent.Id, user).Wait();

            //+5 to user rating by creating new event
            _applicationService.IncrementRating(user.Id, 5).Wait();

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
                isBeforeAfter = model.isBeforeAfter,
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
                    TransportType = model.TransportTypeAfter,
                    DepartureCity = model.DepartureCityAfter,
                    DepartureTime = model.DepartureTimeAfter,
                    ArrivalCity = model.ArrivalCityAfter,
                    ArrivalTime = model.ArrivalTimeAfter
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
                if (reply.Event.Subscribers == null || reply.Event.Subscribers.Count() == 0)
                {
                    return false;
                }
                else if (reply.Event.Subscribers.FirstOrDefault(t => t.UserId == _userManager.GetUserId(User)) == null)
                {
                    return false;
                }
                else return true;
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