using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Api.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

       [HttpGet]
       public async Task<ActionResult> GetAll()
       {
           if (!ModelState.IsValid) 
           {
               return BadRequest(ModelState); 
           }

           try
           {
            return Ok (await _service.GetAll());
           } catch (ArgumentException ex) {
               return StatusCode ((int) HttpStatusCode.InternalServerError, ex.Message);
           }


       }

       [HttpGet]
       [Route("{id}", Name="GetWithId")]
       public async Task<ActionResult> GetId(Guid id) 
       {
           if (!ModelState.IsValid) 
           {
               return BadRequest(ModelState); 
           }

           try
           {
            return Ok (await _service.Get(id));
           } catch (ArgumentException ex) {
               return StatusCode ((int) HttpStatusCode.InternalServerError, ex.Message);
           }   

       }

        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] UserEntity user)
        {
          
           if (!ModelState.IsValid) 
           {
               return BadRequest(ModelState); 
           }

            try 
            {

                var result = await _service.Post(user);

                if (result!=null) {
                    // O registro foi de fato gravado no banco, 
                    // Então retorna um Create StatusCod 201, e retorna o próprio objeto criado User no resul,
                    return Created(new Uri(Url.Link("GetWithId", new {id = result.ID})), result);
                } else {
                    return BadRequest();
                }


            } catch (ArgumentException ex) 
            {
              return StatusCode ((int) HttpStatusCode.InternalServerError, ex.Message);   
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put ([FromBody] UserEntity user) 
        {
            //Verifica se o User está dentro dos padrões da entidade.    
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.Put (user);
                if (result != null) 
                {
                    return Ok (result);
                } else 
                {
                  return BadRequest();      
                }

            } catch (ArgumentException ex) 
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        } 

        //Informa para o metodo Delete que é necessário um ID
        [HttpDelete ("{id}")]
       public async Task<ActionResult> Delete(Guid id) 
       {
           if (!ModelState.IsValid) 
           {
               return BadRequest(ModelState); 
           }

           try
           {
            return Ok (await _service.Delete(id));
           } catch (ArgumentException ex) {
               return StatusCode ((int) HttpStatusCode.InternalServerError, ex.Message);
           }   

       }

    }
}