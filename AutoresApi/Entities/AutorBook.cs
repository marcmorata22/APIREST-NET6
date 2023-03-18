namespace AutoresApi.Entities
{
    public class AutorBook
    {
        public int BookId { get; set; }
        public int AutorId { get; set; }
        public int Order { get; set; }
        public Book book { get; set; }
        public Autor autor { get; set; }
    }
}
