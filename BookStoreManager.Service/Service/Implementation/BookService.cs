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
using AutoMapper;
using System.Drawing;

namespace BookStoreManager.Service.Service.Implementation
{


    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BookService(IBookRepo bookRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

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

        public async Task UpdateAsync(Guid BookId, BookDTO UpdatedBook)
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

                
                var existingBook = await _bookRepository.GetByIdAsync(BookId);

                if (existingBook != null)
                {
                    if (existingBook.AuthorId != AuthorId)
                    {
                        throw new NotFoundException("You are not authorized to update this book.");
                    }

                    var updatedBook = _mapper.Map<Book>(UpdatedBook);
                    updatedBook.Id = BookId;

                await _bookRepository.UpdateAsync(BookId, updatedBook);


                }
                else
                {
                throw new Exception("Book does not exist");
                }
        }

        public async Task DeleteAsync(Guid id)
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

                
            var result = await _bookRepository.GetByIdAsync(id);

            if (result is null)
            {
                throw new Exception("Book does not exist");
            }
            if (result.AuthorId != AuthorId)
            {
                throw new NotFoundException("You are not authorized to delete this book.");
            }
            
                await _bookRepository.DeleteAsync(id);
                await _bookRepository.SaveChangesAsync();
                return ;
            
            
        }

        public Task<IEnumerable<Book>> GetBooksByAuthorAsync(Guid AuthorId)
        {
            return _bookRepository.GetBooksByAuthorAsync(AuthorId);
        }
    }
}