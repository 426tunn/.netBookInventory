using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreManager.Domain.DTOs;
using BookStoreManager.Domain.Entities;
using BookStoreManager.Domain.Enum;

namespace BookStoreManager.Data.Repo.intt
{
    public interface IAuthorRepository
    {
         Task<IEnumerable<Author>> GetAllAsync();
         Task<Author?>  GetAuthor(string email);
         Task<string> CreateAuthor( AuthorDTO author);
         Task<Author?> GetAuthorById(Guid id);
         Task DeleteAuthor(Guid id); 
         Task<string> UpdateUserRole(Guid id, string newRole);
         Task<string> UpdateProfile(Guid id,UpdateProfileDTO profile);
         Task<string> GetUserRole(Guid id);
         Task SaveChangesAsync();   
    }
}