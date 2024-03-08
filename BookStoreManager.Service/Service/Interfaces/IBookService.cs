using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreManager.Domain.DTOs;
using BookStoreManager.Domain.Entities;

namespace BookStoreManager.Service.Service.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(Guid id);
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(Guid AuthorId);
        Task AddAsync(BookDTO book);
        Task UpdateAsync(Guid BookId, BookDTO book);
        Task DeleteAsync(Guid id);
    }
}