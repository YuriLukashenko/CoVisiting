using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;
using CoVisiting.Models.Reply;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace CoVisiting.Controllers
{
    public class ReplyController : Controller
    {
        public readonly IReply _replyService;
        public readonly ICategory _categoryService;
        public readonly IEvent _eventService;
        public readonly IApplicationUser _applicationUser;
        private static UserManager<ApplicationUser> _userManager;

        public ReplyController(IReply replyService, ICategory categoryService, IEvent eventService, IApplicationUser applicationUser, UserManager<ApplicationUser> userManager)
        {
            _replyService = replyService;
            _userManager = userManager;
            _categoryService = categoryService;
            _eventService = eventService;
            _applicationUser = applicationUser;
        }

        private ApplicationUser GetUserFromClaimsPrincipal()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return new ApplicationUser();
            return _userManager.FindByIdAsync(userId).Result;
        }


        public IActionResult Create(int eventId, string recieverId)
        {
            var currentEvent = _eventService.GetById(eventId);
            var category = currentEvent.Category;

            var model = new NewReplyModel
            {
                EventId = currentEvent.Id,
                EventName = currentEvent.Title,
                UserName = User.Identity.Name,
                CategoryName = category.Title,
                CategoryId = category.Id,
                Sender = GetUserFromClaimsPrincipal(),
                Reciever = _applicationUser.GetById(recieverId),
                RecieverId = recieverId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReply(NewReplyModel model)
        {
            var currentEvent = _eventService.GetById(model.EventId);

            var newReply = BuildReply(model, currentEvent);

            _replyService.Add(newReply).Wait();

            return RedirectToAction("Index", "Event", new { id = currentEvent.Id });
        }

        private EventReply BuildReply(NewReplyModel model, Event currentEvent)
        {
            return new EventReply()
            {
                Sender = GetUserFromClaimsPrincipal(),
                Reciever = _applicationUser.GetById(model.RecieverId),
                Event = currentEvent,
                Content = model.Content,
                Created = DateTime.Now,
                ReplyScope = model.ReplyScope
            };
        }
    }
}