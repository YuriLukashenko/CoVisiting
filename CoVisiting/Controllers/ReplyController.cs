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
        private static UserManager<ApplicationUser> _userManager;

        public ReplyController(IReply replyService, ICategory categoryService, IEvent eventService, UserManager<ApplicationUser> userManager)
        {
            _replyService = replyService;
            _userManager = userManager;
            _categoryService = categoryService;
            _eventService = eventService;
        }

        public IActionResult Create(int id)
        {
            var currentEvent = _eventService.GetById(id);
            var category = currentEvent.Category;

            var model = new NewReplyModel
            {
                EventId = currentEvent.Id,
                EventName = currentEvent.Title,
                UserName = User.Identity.Name,
                CategoryName = category.Title,
                CategoryId = category.Id

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReply(NewReplyModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            var currentEvent = _eventService.GetById(model.EventId);

            var newReply = BuildReply(model, user, currentEvent);

            _replyService.Add(newReply).Wait();

            return RedirectToAction("Index", "Event", new { id = currentEvent.Id });
        }

        private EventReply BuildReply(NewReplyModel model, ApplicationUser user, Event currentEvent)
        {
            return new EventReply()
            {
                User = user,
                Event = currentEvent,
                Content = model.Content,
                Created = DateTime.Now,
                ReplyScope = model.ReplyScope
            };
        }
    }
}