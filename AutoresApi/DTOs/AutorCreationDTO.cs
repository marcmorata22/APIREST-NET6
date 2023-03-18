using AutoresApi.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoresApi.DTOs
{
    public class AutorCreationDTO
    {
        [Required(ErrorMessage = "Name is Required!")]
        [StringLength(maximumLength: 6, ErrorMessage = " max length")]
        [FirstLetterCapital]
        public string Name { get; set; }
    }
}
