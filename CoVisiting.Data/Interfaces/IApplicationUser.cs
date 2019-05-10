using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoVisiting.Data.Models;

namespace CoVisiting.Data.Interfaces
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();
        IEnumerable<ApplicationUser> GetFiltered(string searchQuery);
        Task SetProfileImage(string id, string path);
        Task IncrementRating(string id, int value);
        Task DecrementRating(string id, int value);
    }
}
