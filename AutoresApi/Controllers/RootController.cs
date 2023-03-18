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
        [HttpGet(Name =" ObtainRoot")]
        public ActionResult<IEnumerable<DataHATEOASDTO>> Get()
        {
            var dataHateoas = new List<DataHATEOASDTO>();

            dataHateoas.Add(new DataHATEOASDTO(link: Url.Link("ObtainRoot",
                new {}), description: "self", method: "GET"));

            return dataHateoas;
        }
    }
}
