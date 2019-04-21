using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;
using CoVisiting.Models.ApplicationUser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IHostingEnvironment _appEnvironment;

        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload uploadService, IEvent eventService, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _eventService = eventService;
            _appEnvironment = appEnvironment;
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

        public IActionResult Listing()
        {
            var users = _userService.GetAll();
            var profileList = BuildProfileListing(users);

            var model = new ProfileListingModel()
            {
                ProfileList = profileList
            };
            return View(model);
        }

        private IEnumerable<ProfileModel> BuildProfileListing(IEnumerable<ApplicationUser> users)
        {
            return users.Select(user => new ProfileModel()
            {
                userId = user.Id,
                ProfileImageUrl = user.ProfileImageUrl,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                MemberSince = user.MemberSince,
                UserRating = user.Rating.ToString(),
                CountEvents = _eventService.GetAll().Count(newEvent => newEvent.User.UserName == user.UserName)
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(string userId, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/images/users/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                await _userService.SetProfileImage(userId, path);
            }

            return RedirectToAction("Detail", "Profile", new { id = userId });
        }
    }
}