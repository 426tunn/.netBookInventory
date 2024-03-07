using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreManager.Domain.DTOs
{
    public class BookDTO
    {
        public string Title { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public decimal Price { get; set; }
        public int Quantities { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
    }
}