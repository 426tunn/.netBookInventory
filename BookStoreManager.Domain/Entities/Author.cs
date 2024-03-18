using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreManager.Domain.Enum;

namespace BookStoreManager.Domain.Entities
{
    public class Author
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public ICollection<Book>? Books { get; set; }
        public UserRole  Role { get; set; }
    
        
    }
}