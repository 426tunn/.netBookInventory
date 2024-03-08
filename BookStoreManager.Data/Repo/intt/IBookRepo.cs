using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreManager.Domain.DTOs;
using BookStoreManager.Domain.Entities;

namespace BookStoreManager.Data.Repo.intt
{
    public interface IBookRepo
    {
         Task<IEnumerable<Book>> GetAllAsync();
         Task<IEnumerable<Book>> GetBooksByAuthorAsync(Guid AuthorId);
        Task<Book> GetByIdAsync(Guid id);
        Task AddAsync(BookDTO book);
        Task UpdateAsync(Guid BookId, Book book);
        Task DeleteAsync(Guid BookId);
        Task SaveChangesAsync();
    }
}