using System.ComponentModel.DataAnnotations;

namespace ReviewApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required,Range(0,5)]
        public int Rating { get; set; }
        public Reviewer Reviewer { get; set; }
        public Product Product { get; set; }

    }
}

