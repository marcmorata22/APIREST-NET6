using AutoresApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace AutoresApi.DTOs
{
    public class BookCreationDTO
    {
        [FirstLetterCapital]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
