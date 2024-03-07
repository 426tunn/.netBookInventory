using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreManager.Contracts.Authentication;
using BookStoreManager.Domain.DTOs;
using BookStoreManager.Domain.Entities;

namespace BookStoreManager.Service.Authentication.Interface
{
    public interface IAuthenticationService
    {
        Task<ApiResponse> Login( userLogin author);
        Task<string> Register(AuthorDTO author);
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetAuthor(Guid Id);
        Task<string> DeleteAuthor(Guid Id);
    }
}