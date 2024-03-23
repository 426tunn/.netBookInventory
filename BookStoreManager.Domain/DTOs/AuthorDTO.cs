using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookStoreManager.Domain.Enum;

namespace BookStoreManager.Domain.DTOs
{
    public class AuthorDTO
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        [RegularExpression("^(Admin|User|SuperAdmin)$", ErrorMessage = "Role must be either 'Admin' or 'User'.")]
        public string? Role { get; set; }
    }
}