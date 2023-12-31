﻿using ReviewApp.Models;

namespace ReviewApp
{
    public interface IProductRepository
    {
        ICollection<Product>GetProducts();
        Product GetProduct(int? id);
        Product GetProduct(string Name);
        decimal  GetProductRating(int id);
        bool CreateProduct(int ownerID ,int CategoryID , Product Product);
        bool UpdateProduct(Product Product);
        bool DeleteProduct(Product Product);
        bool Save();

        



    }

}
