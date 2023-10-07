using System.ComponentModel.DataAnnotations;

namespace ReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
      
        [Required]
        public string Gym { get; set; }
        public Country Country { get; set; }
        public ICollection<ProductOwner> ProductOwners { get; set; }


    }
}
