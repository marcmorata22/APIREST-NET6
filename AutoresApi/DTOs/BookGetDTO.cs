namespace AutoresApi.DTOs
{
    public class BookGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public List<AutorGetDTO> Autores { get; set; }
    }
}
