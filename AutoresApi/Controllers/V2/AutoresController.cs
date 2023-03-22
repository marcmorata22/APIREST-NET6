using AutoMapper;
using AutoresApi.DTOs;
using AutoresApi.Entities;
using AutoresApi.Filters;
using AutoresApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoresApi.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]//controller var that took ther name of the controller class (Autores)
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]// autorizacoin in the whole controller
    public class AutoresController: ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IService _service;
        private readonly ServiceTransient _transient;
        private readonly ServiceScoped _scoped;
        private readonly ServiceSingleton _singleton;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AutoresController(ApplicationDbContext context, IService service, 
            ServiceTransient transient, ServiceScoped scoped, ServiceSingleton singleton, IMapper mapper, IConfiguration configuration)
        {
            this._context = context;
            this._service = service;
            this._transient = transient;
            this._scoped = scoped;
            this._singleton = singleton;
            this._mapper = mapper;
            _configuration = configuration;
        }
        
        [HttpGet(Name ="obtenerAutoresV2")]
        public async Task<List<AutorGetDTO>> Get()
        {
            //_service.performTask();
            var autores = await _context.Autores.ToListAsync();
            return _mapper.Map<List<AutorGetDTO>>(autores);
        }

        [HttpGet("{id:int}")]//How to add second param -> /{param2?} ? => makes it opcional
        public async Task<ActionResult<AutorGetDTO>> SpecificAuthor(int id/*, string param2*/)
        {
            var authors = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if(authors == null)
            {
                return NotFound("Is null");
            }

            return _mapper.Map<AutorGetDTO>(authors);
        }

        [HttpPost (Name ="crearAutoresV2")]
        public async  Task<ActionResult> Post(AutorCreationDTO autorDTO)
        {
            var exist = await _context.Autores.AnyAsync(x => x.Name == autorDTO.Name);

            if (exist)
            {
                return BadRequest("exist the same name");
            }

            //Solution Mapper
            var autor = _mapper.Map<Autor>(autorDTO);

            //Solution Easy(You can forget some propiety
            //var autor = new Autor()
            //{
            //    Name = autorDTO.Name
            //};

            _context.Add(autor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] //api/autores/+id
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if(autor.Id != id)
            {
                return BadRequest("the id does not match");
            }

            var exist = await _context.Autores.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            _context.Update(autor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete (int id)
        {
            var exist = await _context.Autores.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            _context.Remove(new Autor() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
