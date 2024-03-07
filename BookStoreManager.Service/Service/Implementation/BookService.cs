using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreManager.Domain.Entities;
using BookStoreManager.Service.Service.Interface;
using BookStoreManager.Data;
using BookStoreManager.Data.Repo.intt;
using BookStoreManager.Domain.DTOs;
using Microsoft.AspNetCore.Http;

namespace BookStoreManager.Service.Service.Implementation
{


    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookService(IBookRepo bookRepository, IHttpContextAccessor httpContextAccessor)
        {
            _bookRepository = bookRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task AddAsync(BookDTO book)
        {
                if (_httpContextAccessor?.HttpContext?.Session == null)
                {
                    throw new Exception("HttpContext or Session is not available.");
                }

            var AuthIdString =_httpContextAccessor.HttpContext?.Session.GetString("AuthorId");
                if (string.IsNullOrEmpty(AuthIdString))
                {
                    throw new Exception("AuthorId not found in session.");
                }
                if (!Guid.TryParse(AuthIdString, out Guid AuthorId))
                {
                    throw new Exception("Invalid format for AuthorId in session.");
                }
                book.AuthorId = AuthorId;

             await _bookRepository.AddAsync(book);
             await _bookRepository.SaveChangesAsync();

             //TEST THE AUTHOR ID FROM SESSION
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
             return await _bookRepository.GetAllAsync();
             
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Guid id, Book UpdatedBook)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);
        if (existingBook != null)
        {
            existingBook.Title = UpdatedBook.Title;
            existingBook.AuthorId = UpdatedBook.AuthorId;
            existingBook.Price = UpdatedBook.Price;
            existingBook.Quantities = UpdatedBook.Quantities;
            existingBook.Year = UpdatedBook.Year;
            existingBook.Description = UpdatedBook.Description;

          await _bookRepository.UpdateAsync(id, existingBook);

        }
        else
        {
           throw new Exception("Book does not exist");
        }
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await _bookRepository.GetByIdAsync(id);
        if (result is null)
        {
            throw new Exception("Book does not exist");
        }
        else
        {
            await _bookRepository.DeleteAsync(id);
            await _bookRepository.SaveChangesAsync();
            return;
        }
        
        }

    }
}