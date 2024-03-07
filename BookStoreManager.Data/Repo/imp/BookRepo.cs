using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreManager.Data.Repo.intt;
using BookStoreManager.Domain.DTOs;

// using BookStoreManager.Domain.DTO;
using BookStoreManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreManager.Data.Repo.imp
{
    public class BookRepo : IBookRepo
    {
       private readonly AppDbContext _context;
       private readonly IMapper _mapper;
       
        protected readonly DbSet<Book> _dbSet;

    public BookRepo(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
        _dbSet =  _context.Set<Book>();
    }

        public async Task AddAsync(BookDTO book)
        {
            var newBook = _mapper.Map<Book>(book);
           await _context.AddAsync(newBook);
           await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            
           var books = await _dbSet.ToListAsync();
            foreach (var book in books)
            {
                if (book.DateDeleted == DateTime.MinValue)
                {
                    // Set a default value for DateDeleted
                    book.DateDeleted = DateTime.MinValue;
                }
            }          
                return books;
                }

        public async Task<Book> GetByIdAsync(Guid id)
        {
           var fBook =  await _dbSet.FindAsync(id);
           if (fBook != null)
           {
             return fBook;
           
           } else {
                 throw new Exception("Book not found");
       }
        }

        public async Task UpdateAsync(Guid id, Book book)
        {
            var filter = await _dbSet.FindAsync(id);
            if (filter != null)
            {
                _context.Entry(filter).CurrentValues.SetValues(book);
                await _context.SaveChangesAsync();
            }
        
        }

            public async Task DeleteAsync(Guid id)
        {
            var filter = await _dbSet.FindAsync(id);
               if (filter != null)
            {
                _dbSet.Remove(filter);
                await _context.SaveChangesAsync();
            }
        }
 
    }
}