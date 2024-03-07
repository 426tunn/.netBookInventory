using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreManager.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantities { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
        public Guid AuthorId { get; set; }
        public Author? AuthorNavigation { get; set; }
        public DateTime? DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime? DateDeleted { get; set; } 
        public DateTime? DateModified { get; set; } 

    }



}