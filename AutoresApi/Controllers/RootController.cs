using AutoMapper;
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
    [Route("api")]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this._authorizationService = authorizationService;
        }


        [HttpGet(Name =" ObtainRoot")]
        public async Task<ActionResult<IEnumerable<DataHATEOASDTO>>> Get()
        {
            var dataHateoas = new List<DataHATEOASDTO>();

            var isAdmin = await _authorizationService.AuthorizeAsync(User, "IsAdmin");

            dataHateoas.Add(new DataHATEOASDTO(link: Url.Link("ObtainRoot",
                new {}), description: "self", method: "GET"));
            
            dataHateoas.Add(new DataHATEOASDTO(link: Url.Link("obtenerAutores",
                new {}), description: "obtener autores", method: "GET"));
            
            dataHateoas.Add(new DataHATEOASDTO(link: Url.Link("obtenerLibros",
                new {}), description: "obtener libro", method: "GET"));
            if (isAdmin.Succeeded)
            {
                dataHateoas.Add(new DataHATEOASDTO(link: Url.Link("crearAutores",
                new {}), description: "crear autor", method: "POST"));
            
                dataHateoas.Add(new DataHATEOASDTO(link: Url.Link("crearLibro",
                new {}), description: "crear libro", method: "POST"));
            }
            

            return dataHateoas;
        }
    }
}
