using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository

    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateCategory(Category category)
        {
_context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategoryById(int? id)
        {
            return _context.Categories.SingleOrDefault(c => c.Id == id);
        }

        public Category GetCategoryByName(string name)
        {
            return _context.Categories.SingleOrDefault(C => C.Name == name);
        }

        public IEnumerable<Product> GetProductByCategoryId(int categoryId)
        {
            return _context.ProductCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => pc.Product)
                .ToList();
        }

       

        public bool UpdateCategory(Category category)
        {

            _context.Update(category);
           return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
