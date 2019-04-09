using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoVisiting.Data.Models;

namespace CoVisiting.Data.Interfaces
{
    public interface ICategory
    {
        Category GetById(int id);
        IEnumerable<Category> GetAll();
        IEnumerable<ApplicationUser> GetAllActiveUsers();

        Task Create(Category category);
        Task Delete(int categoryId);
        Task UpdateCategoryTitle(int categoryId, string newTitle);
        Task UpdateCategoryDecription(int categoryId, string newDescription);

    }
}
