using Microsoft.AspNetCore.Mvc;
using BookStoreManager.Contracts.Authentication;
using BookStoreManager.Domain.Entities;
using BookStoreManager.Service.Authentication.Interface;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using BookStoreManager.Domain.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookStoreManager.Data;

namespace BookStoreManager.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("author")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationservice;

        public AuthenticationController(IAuthenticationService authenticationservice)
        {
            _authenticationservice = authenticationservice;
        }


           [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]AuthorDTO author)
        {
            await _authenticationservice.Register(author/*author.Id, author.Firstname, author.Lastname, author.Email, author.Password*/);
            return Ok(
            " Author created successfully");
        }

         [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] userLogin author)
        {
             var response = await _authenticationservice.Login(author);
             if(response.StatusCode == (int)HttpStatusCode.OK)
             {
                return Ok(response);
             }
             else
             {
                return BadRequest(response);
             }
          
        }


        [HttpGet]
        public async Task<ActionResult> GetAllAuthors()
        {
            try{
                 var authors = await _authenticationservice.GetAllAsync();
                 return Ok(authors);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult> GetAuthor(Guid authorId)
        {
            try {
                var author = await _authenticationservice.GetAuthor(authorId);
                 if (author == null)
                {
                    return NotFound($"Author with id {authorId} is not found");
                }
                 var options = new JsonSerializerOptions
                 {
                    ReferenceHandler = ReferenceHandler.Preserve
                 };
                 return Ok(JsonSerializer.Serialize(author, options));
            } catch(NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            } 
                catch (Exception ex)
                {
                    // throw new NotFoundException(ex.Message);
                    // Log the exception or handle it as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
                }
          
        }


        [HttpDelete("{authorId}")]
        public async Task<ActionResult> DeleteBook(Guid authorId)
        {
            try{
            await _authenticationservice.DeleteAuthor(authorId);
            return Ok("Book Deleted successfully");
            } catch(Exception ex) {
                throw new Exception("Error deleting book", ex);
            }
            
        }      
    }
}