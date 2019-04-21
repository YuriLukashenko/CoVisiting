using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CoVisiting.Models.ApplicationUser
{
    public class ProfileModel
    {
        public string userId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int UserRating { get; set; }
        public string ProfileImageUrl { get; set; }
        public string City { get; set; }
        public int CountEvents { get; set; }

        public DateTime MemberSince { get; set; }
        public IFormFile ImageUpload { get; set; }
    }
}
