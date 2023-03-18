using AutoMapper;
using AutoresApi.DTOs;
using AutoresApi.Entities;

namespace AutoresApi.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreationDTO, Autor>();
            CreateMap<Autor, AutorGetDTO>();

            CreateMap<BookCreationDTO, Book>()
                .ForMember(x => x.AutoresBooks, options => options.MapFrom(MapAutoresBooks));
            CreateMap<Book, BookGetDTO>()
                .ForMember(x => x.Autores, options => options.MapFrom(MapBooksDTOAutores));

            CreateMap<CommentsCreationDTO, Comment>();
            CreateMap<Comment, CommentGetDTO>();
        }

        private List<AutorGetDTO> MapBooksDTOAutores (Book book, BookGetDTO bookDTO)
        {
            var result = new List<AutorGetDTO>();

            if(book.AutoresBooks == null)
            {
                return result;
            }

            foreach (var autorBook in book.AutoresBooks)
            {
                result.Add(new AutorGetDTO()
                {
                    Id = autorBook.AutorId,
                    Name = autorBook.autor.Name
                });
            }
            return result;
        }


        private List<AutorBook> MapAutoresBooks(BookCreationDTO bookDTO, Book book) 
        {
            var result = new List<AutorBook>();

            if(bookDTO.AutoresIds == null)
            {
                return result;
            }

            foreach(var autorId in bookDTO.AutoresIds)
            {
                result.Add(new AutorBook() { AutorId = autorId });
            }

            return result;
        }
    }
}
