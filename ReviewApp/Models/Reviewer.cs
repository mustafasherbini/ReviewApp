using System.ComponentModel.DataAnnotations;

namespace ReviewApp.Models
{
    public class Reviewer
    {
     


        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public ICollection<Review> Reviews  { get; set; }
    }

}
