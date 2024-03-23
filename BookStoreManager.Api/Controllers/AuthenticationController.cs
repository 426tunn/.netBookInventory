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
using BookStoreManager.Service.Service.Interface;
using BookStoreManager.Domain.Enum;

namespace BookStoreManager.API.Controllers
{
    [ApiController]
    [Route("author")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationservice;
        private readonly IBookService _bookService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationController(IAuthenticationService authenticationservice, IBookService bookService, IHttpContextAccessor httpContextAccessor)
        {
            _authenticationservice = authenticationservice;
            _bookService = bookService;
            _httpContextAccessor = httpContextAccessor;
        }


           [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]AuthorDTO author)
        {
                var options = new JsonSerializerOptions();
            await _authenticationservice.Register(author);
            return Ok(JsonSerializer.Serialize(author, options));
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

        // [HttpPost("logout")]
        // public async Task<IActionResult> Logout()
        // {
        //     // Clear session or token data
        //     _httpContextAccessor.HttpContext?.Session.Clear();
            
        //     // Or remove JWT token if using JWT

        //     return Ok("Logout successful");
        // }
     
        [HttpPut("role/{authorId}")]
        public async Task<IActionResult>UpdateUserRole(Guid authorId, [FromBody] string newRole)
        {
            try
            {             
                var message = "User role updated successfully";
                var result = await _authenticationservice.UpdateUserRole(authorId, newRole);
                if (result == "User not found"){
                    return NotFound(result);
                }
                return new OkObjectResult(new { Result = result, Message = message });
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAllAuthors()
        {
            try{
                 var authors = await _authenticationservice.GetAllAsync();
                 var options = new JsonSerializerOptions
                 {
                    ReferenceHandler = ReferenceHandler.Preserve
                 };
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
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
          
        }

        [HttpGet("books/{authorId}")]
        public async Task<IActionResult> GetBooksByAuthorId(Guid authorId)
        {
            try {
                var books = await _bookService.GetBooksByAuthorAsync(authorId);
                        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };
               return Ok(JsonSerializer.Serialize(books, options));
            } catch (NotFoundException ex){
                return NotFound(ex.Message);
            } catch (Exception ex) {
                 return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }


        [HttpDelete("{authorId}")]
        public async Task<ActionResult> DeleteBook(Guid authorId)
        {
            try
            {
                 var result =await _authenticationservice.DeleteAuthor(authorId);
                 return Ok(result);
            } 
            catch(Exception ex)
            {
                throw new Exception("Error deleting book", ex);
            }
            
        }      
    }
}