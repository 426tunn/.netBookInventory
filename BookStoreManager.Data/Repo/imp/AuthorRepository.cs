using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreManager.Data;
using BookStoreManager.Data.Repo.intt;
using BookStoreManager.Domain.DTOs;
using BookStoreManager.Domain.Entities;
using BookStoreManager.Domain.Enum;
using BookStoreManager.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace BookStoreManager.Data.Repo.imp
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<Author> _dbSet;
        private readonly IMapper _mapper;

    public AuthorRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _dbSet =  _context.Set<Author>();

    }

    public async Task<string> CreateAuthor(AuthorDTO author)
     {
           var newAuthor = _mapper.Map<Author>(author);
            if (!ValidationUtils.IsValidEmail(author.Email))
            {
                throw new Exception("Invalid email");
            }
           await _context.AddAsync(newAuthor);
           await _context.SaveChangesAsync();
           return "Author created successfully";
     }

    public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var Authors =  _context.Authors;
            if (Authors != null)
            {
                var getAuthors = await Authors
                .Include(a => a.Books)
                .ToListAsync();
                if(getAuthors == null)
                {
                   throw new NotFoundException("Authors not found");
                }
                return getAuthors;
            }
            throw new NotFoundException("Authors not found");
           
        }

    public async Task<Author?> GetAuthor(string email)
        {
            var authors = _context.Authors;
            if (authors != null)
            {

            var existingAuthor = await authors
            .FirstOrDefaultAsync(a => a.Email == email);
                return existingAuthor;
            }
            return null;
        }
    public async Task<Author?> GetAuthorById(Guid id)
        {
             var author = _context.Authors;
             if (author != null)
             {
                 var authorWithBooks = await author
               .Include(a => a.Books)
             .FirstOrDefaultAsync(a => a.Id == id);
             if (authorWithBooks == null)
             {
                throw new NotFoundException($"Author with id {id} is not found");
             }
                 return authorWithBooks;
             }
            return null;
        }
    public async Task DeleteAuthor(Guid id)
        {
             var authorExists = _context.Authors;
            if (authorExists == null)
            {
                throw new Exception("Context is null");
            }

            var existingAuthor = await authorExists.FirstOrDefaultAsync(a => a.Id == id);
            if (existingAuthor == null)
            {
                throw new Exception("Author does not exist");
            }
           _context.Authors?.Remove(existingAuthor);
            await _context.SaveChangesAsync();
        }


    public async Task SaveChangesAsync()
            {
                await _context.SaveChangesAsync();
            }

    public async Task<string> UpdateUserRole(Guid id, string newRole)
        {
             var authorExists =  _context.Authors;
            if (authorExists == null)
            {
                throw new Exception("Context is null");
            }

            var existingAuthor =  authorExists.FirstOrDefault(a => a.Id == id);
            if (existingAuthor == null)
            {
                throw new Exception("Author does not exist");
            }
            existingAuthor.Role = newRole;
            await _context.SaveChangesAsync();
            return "Author role updated successfully";
        }

        public Task<string> GetUserRole(Guid id)
        {
            var authorExists =  _context.Authors;
            if (authorExists == null)
            {
                throw new Exception("Context is null");
            }

            var existingAuthor =  authorExists.FirstOrDefault(a => a.Id == id);
            if (existingAuthor == null)
            {
                throw new Exception("Author does not exist");
            }      
            return Task.FromResult(existingAuthor.Role ?? string.Empty);  
        }

        public async Task<string> UpdateProfile(Guid id, UpdateProfileDTO profile)
        {
            var authorExists =  _context.Authors;
            if (authorExists == null)
            {
                throw new Exception("Context is null");
            }        
            var existingAuthor =  authorExists.FirstOrDefault(a => a.Id == id);
            if (existingAuthor == null)
            {
                throw new Exception("Author does not exist");
            }      
            if (!string.IsNullOrEmpty(profile.Firstname))
            {
                existingAuthor.Firstname = profile.Firstname;
            }

            if (!string.IsNullOrEmpty(profile.Lastname))
            {
                existingAuthor.Lastname = profile.Lastname;
            }

            if (!string.IsNullOrEmpty(profile.Email))
            {
                existingAuthor.Email = profile.Email;
            }

            await _context.SaveChangesAsync();
            return "Author profile updated successfully";
        }
    }
}