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
                UserRating = user.Rating,
                City = user.City,
                CountEvents = _eventService.GetAll().Count(newEvent => newEvent.Author.UserName == user.UserName)
            };
            return View(model);
        }

        public IActionResult Listing(int id = 0, string searchQuery = null)
        {
            var users = new List<ApplicationUser>();
            string eventName = "";
            int eventId = 0;

            if (id != 0)
            {
                var eventUsers = _eventService.GetById(id).Subscribers.ToList();
                foreach (var user in eventUsers)
                {
                    users.Add(_userService.GetById(user.UserId));
                }

                eventName = _eventService.GetById(id).Title;
                eventId = _eventService.GetById(id).Id;
            }
            else if (searchQuery == null)
            {
                users = _userService.GetAll().ToList();
            }
            else
            {
                users = _userService.GetFiltered(searchQuery).ToList();
            }

            var profileList = BuildProfileListing(users);

            var model = new ProfileListingModel()
            {
                ProfileList = profileList,
                SearchQuery = searchQuery,
                EventName = eventName,
                EventId = eventId
                
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
                UserRating = user.Rating,
                CountEvents = _eventService.GetAll().Count(newEvent => newEvent.Author.UserName == user.UserName)
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

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Listing", new { searchQuery });
        }
    }
}