using AutoresApi.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoresApi.Entities
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is Required!")]
        [StringLength(maximumLength: 6, ErrorMessage =" max length")]
        [FirstLetterCapital]
        public string Name { get; set; }
        public List<AutorBook> AutoresBooks { get; set; }

        //    [Range(18,120)]
        //    [NotMapped]
        //    public int Age { get; set; }
    }
}
