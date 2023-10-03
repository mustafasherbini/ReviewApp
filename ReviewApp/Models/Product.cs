namespace ReviewApp.Models
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ProductOwner> ProductOwners { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }


         
    }
}
