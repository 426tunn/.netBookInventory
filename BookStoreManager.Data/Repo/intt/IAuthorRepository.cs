using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreManager.Domain.DTOs;
using BookStoreManager.Domain.Entities;

namespace BookStoreManager.Data.Repo.intt
{
    public interface IAuthorRepository
    {
         Task<IEnumerable<Author>> GetAllAsync();
         Task<Author?>  GetAuthor(string email);
         Task<string> CreateAuthor( AuthorDTO author);
         Task<Author?> GetAuthorById(Guid id);
         Task DeleteAuthor(Guid id);    
    }
}