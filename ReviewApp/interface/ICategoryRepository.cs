using System.Collections.Generic;
using ReviewApp.Models;

namespace ReviewApp
{
    public interface ICategoryRepository
    {
        // Retrieve a collection of all categories.
        IEnumerable<Category> GetAllCategories();

        // Retrieve a specific category by its ID.
        Category GetCategoryById(int categoryId);
        Category GetCategoryByName(string name);

        // Retrieve a collection of Pokémon associated with a specific category.
        IEnumerable<Product> GetProductByCategoryId(int categoryId);

        // Check if a category with a given ID exists.
        bool DoesCategoryExist(int? categoryId);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
