using AutoMapper;
using AutoresApi.DTOs;
using AutoresApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoresApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet("{id:int}", Name ="obtenerLibros")]
        public async Task<ActionResult<BookGetDTO>> Get(int id)
        {
            var book = await _context.Books
                .Include(x => x.AutoresBooks)
                .ThenInclude(x => x.autor)                
                .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<BookGetDTO>(book);

        }

        [HttpPost(Name ="crearLibro")]
        public async Task<ActionResult> Post(BookCreationDTO bookDTO)
        {
            if(bookDTO.AutoresIds == null)
            {
                return BadRequest("is not posible create a book without Author");
            }

            var existAutoresIds = await _context.Autores
                .Where(x => bookDTO.AutoresIds.Contains(x.Id)).Select(x =>x.Id).ToListAsync();

            if (bookDTO.AutoresIds.Count != existAutoresIds.Count)
            {
                return BadRequest("Author Id does not exist");
            }

            var book = _mapper.Map<Book>(bookDTO);

            if(book.AutoresBooks != null)
            {
                for(int i = 0; i < book.AutoresBooks.Count; i++)
                {
                    book.AutoresBooks[i].Order = i;
                }
            }

            _context.Add(book);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
