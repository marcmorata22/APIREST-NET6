﻿using AutoMapper;
using AutoresApi.DTOs;
using AutoresApi.Entities;
using AutoresApi.Filters;
using AutoresApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoresApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]//controller var that took ther name of the controller class (Autores)
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

        //Execute this methon in swagger UI for understand the type of service
        [HttpGet("GUID")]
        [AllowAnonymous]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(myFilter))]
        public ActionResult GetGuids()
        {
            return Ok(new
            {
                AutoresControllerTransient = _transient.guid,
                ServiceA_transient = _service.GetTransient(),
                AutoresControllerScoped = _scoped.guid,
                ServiceA_scoped = _service.GetScoped(),
                AutoresControllerSingleton = _singleton.guid,
                ServiceA_singleton = _service.GetSingleton(),

            });
        }

        [HttpGet("configuraciones")]
        public ActionResult<string> getConfiguration()
        {
            return _configuration["lastName"];
        }


        [HttpGet(Name ="obtenerAutores")]
        [HttpGet("list")]
        [HttpGet("/myList")]//The Url only with /myList
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

        [HttpGet("{name}")]
        public async Task<ActionResult<List<AutorGetDTO>>> SpecificAuthor(string name)
        {
            var authors = await _context.Autores.Where(x => x.Name.Contains(name)).ToListAsync();

            if (authors == null)
            {
                return NotFound("Is null");
            }

            return _mapper.Map<List<AutorGetDTO>>(authors);
        }

        [HttpPost (Name ="crearAutores")]
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
