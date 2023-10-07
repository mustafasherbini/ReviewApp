using System.ComponentModel.DataAnnotations;

namespace ReviewApp.Models
{
    public class Category
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ProductCategory> ProducCategories { get; set; }
            

    }
}
