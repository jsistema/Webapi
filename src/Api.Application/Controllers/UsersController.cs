using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services.User;
using Api.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

       [HttpGet]
       public async Task<ActionResult> GetAll([FromServices] UserService service)
       {
           if (!ModelState.IsValid) 
           {
               return BadRequest(ModelState); 
           }

           try
           {
            return Ok (await service.GetAll());
           } catch (ArgumentException ex) {
               return StatusCode ((int) HttpStatusCode.InternalServerError, ex.Message);
           }


       } 

    }
}