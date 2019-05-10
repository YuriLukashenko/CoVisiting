using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoVisiting.Data;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;

namespace CoVisiting.Service
{
    public class ApplicationUserService: IApplicationUser
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetById(string id)
        {
            return GetAll().FirstOrDefault(user => user.Id == id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public IEnumerable<ApplicationUser> GetFiltered(string searchQuery)
        {
            return String.IsNullOrEmpty(searchQuery)
                ? _context.ApplicationUsers 
                : _context.ApplicationUsers.Where(user 
                    => user.UserName.Contains(searchQuery) ||
                       user.Email.Contains(searchQuery) ||
                       user.City.Contains(searchQuery));
        }

        public async Task SetProfileImage(string id, string path)
        {
            var user = GetById(id);
            user.ProfileImageUrl = path;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task IncrementRating(string id, int value)
        {
            var user = GetById(id);
            user.Rating += value;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DecrementRating(string id, int value)
        {
            var user = GetById(id);
            user.Rating -= value;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}
