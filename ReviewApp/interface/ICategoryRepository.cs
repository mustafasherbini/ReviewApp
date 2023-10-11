using System.Collections.Generic;
using ReviewApp.Models;

namespace ReviewApp
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int? categoryId);
        Category GetCategoryByName(string name);
        IEnumerable<Product> GetProductByCategoryId(int categoryId);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
