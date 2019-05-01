using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoVisiting.Models.ApplicationUser
{
    public class ProfileListingModel
    {
        //public string userId { get; set; }
        //public string ProfileImageUrl { get; set; }
        //public string UserName { get; set; }
        //public string Email { get; set; }
        //public string City { get; set; }
        //public DateTime MemberSince { get; set; }
        //public string UserRating { get; set; }
        //public int CountEvents { get; set; }

        public IEnumerable<ProfileModel> ProfileList { get; set; }
        public string SearchQuery { get; set; }
    }
}
