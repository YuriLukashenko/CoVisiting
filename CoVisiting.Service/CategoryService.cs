using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoVisiting.Data;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CoVisiting.Service
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category GetById(int id)
        {
            return _context.Categories.Where(c => c.Id == id)
                .Include(c => c.Events).ThenInclude(e => e.Author)
                .Include(c => c.Events).ThenInclude(e => e.Replies).ThenInclude(r => r.Sender)
                .FirstOrDefault();
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.Include(category => category.Events);
        }

        public IEnumerable<ApplicationUser> GetAllActiveUsers()
        {
            throw new NotImplementedException();
        }

        public Task Create(Category category)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryTitle(int categoryId, string newTitle)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryDecription(int categoryId, string newDescription)
        {
            throw new NotImplementedException();
        }
    }
}
