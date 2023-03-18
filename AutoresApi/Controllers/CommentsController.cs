using AutoMapper;
using AutoresApi.DTOs;
using AutoresApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoresApi.Controllers
{
    [ApiController]
    [Route("api/Books/{bookId:int}/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentsController(ApplicationDbContext context,
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        [HttpGet()]
        public async Task<ActionResult<List<CommentGetDTO>>> Get(int bookId)
        {
            var existBook = await _context.Books.AnyAsync(x => x.Id == bookId);

            if (!existBook)
            {
                return NotFound();
            }

            var comments = await _context.Comments.Where(x => x.BookId == bookId).ToArrayAsync();
            return _mapper.Map<List<CommentGetDTO>>(comments);

        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult>Post(int bookId, CommentsCreationDTO commentDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var user = await _userManager.FindByEmailAsync(email);
            var userId = user.Id;
            var existBook = await _context.Books.AnyAsync(x => x.Id == bookId);

            if (!existBook)
            {
                return NotFound();
            }

            var comment = _mapper.Map<Comment>(commentDTO);
            comment.BookId = bookId;
            comment.UserID = userId;
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

}
