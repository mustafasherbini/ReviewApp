using System.ComponentModel.DataAnnotations;

namespace ReviewApp.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Owner> Owners { get; set; }


    }
}
