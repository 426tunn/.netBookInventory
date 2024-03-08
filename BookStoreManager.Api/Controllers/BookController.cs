using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BookStoreManager.Data;
using BookStoreManager.Domain.DTOs;

// using BookStoreManager.Domain.DTO;
using BookStoreManager.Domain.Entities;
using BookStoreManager.Service.Service.Implementation;
using BookStoreManager.Service.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreManager.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody]BookDTO book)
        {
            await _bookService.AddAsync(book);
            return Ok("Book created successfully by Author with Id:" + book.AuthorId);
            
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(books);
        }



      [HttpGet("{bookId}")]
        public async Task<ActionResult> GetBookById(Guid bookId)
        {
            var book = await _bookService.GetByIdAsync(bookId);
            return Ok(book);
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult> UpdateBook(Guid bookId, [FromBody] BookDTO updatedBook)
        {

            try{
             await _bookService.UpdateAsync(bookId, updatedBook);
            return Ok("Book Updated successfully");

            } catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating book: " + ex.Message);
            }
        }

        [HttpDelete("{bookId}")]
        public async Task<ActionResult> DeleteBook(Guid bookId)
        {
            try
            {
               await _bookService.DeleteAsync(bookId);
            return Ok("Book Deleted successfully");
            } catch(Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating book: " + ex.Message);
            }
            
        }        
    }
}