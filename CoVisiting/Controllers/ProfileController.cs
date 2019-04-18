using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;
using CoVisiting.Models.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoVisiting.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IEvent _eventService;

        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload uploadService, IEvent eventService)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _eventService = eventService;
        }

        public IActionResult Detail(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var model = new ProfileModel
            {
                userId = user.Id,
                Email = user.Email,
                MemberSince = user.MemberSince,
                ProfileImageUrl = user.ProfileImageUrl,
                UserName = user.UserName,
                UserRating = user.Rating.ToString(),
                City = user.City,
                CountEvents = _eventService.GetAll().Count(newEvent => newEvent.User.UserName == user.UserName)
            };
            return View(model);
        }
    }
}