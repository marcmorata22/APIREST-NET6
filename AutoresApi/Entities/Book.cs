using AutoresApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace AutoresApi.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [FirstLetterCapital]
        [StringLength(maximumLength:250)]
        public string Title { get; set; }
        public List<Comment> comments { get; set; }
        public List<AutorBook> AutoresBooks { get; set; } 
    }
}
