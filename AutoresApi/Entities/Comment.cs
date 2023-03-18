using Microsoft.AspNetCore.Identity;

namespace AutoresApi.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int BookId { get; set; }
        public Book book { get; set; }
        public string UserID { get; set; } 
        public IdentityUser User { get; set; }
    }
}
