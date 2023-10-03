using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.DTO;
using ReviewApp.Models;
using System.Linq;

namespace ReviewApp.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext con) {
        _context = con;
        }

        public bool CreateProduct(int ownerID, int CategoryID, Product product)
        {
            var ProductOwnerEntity = _context.Owners.Where(x => x.Id == ownerID).FirstOrDefault();
            var category = _context.Categories.Where(x => x.Id == CategoryID).FirstOrDefault();

            var ProductOwner = new ProductOwner()
            {
                Owner = ProductOwnerEntity,
                Product = product,
            };

            var pokemoncategory = new ProductCategory()
            {
                Category = category,
                Product = product,
            };

            _context.Add(pokemoncategory);
            _context.Add(ProductOwner);
            _context.Add(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);
            return Save();
        }

        public Product GetProduct(int id)
        {
        return _context.Product.SingleOrDefault(x=>x.Id == id);
        }

        public Product GetProduct(string Name)
        {
            return _context.Product.SingleOrDefault(x => x.Name == Name);
        }

        public decimal GetProductRating(int Id)
        {

            var all = _context.Reviews.Where(x => x.Id == Id);
            if (all.Count() == 0)
            {
                return 0;
            }

            return  all.Sum(review => review.Rating) / all.Count();


        }

        public ICollection<Product> GetProduct()
        {
        return  _context.Product.ToList();
        }

       

        public bool ProductExist(int id)
        {
            return _context.Product.Any(x => x.Id == id);

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProduct(Product product)
        {
            _context.Update(product);
            return Save();
        }
    }
}
