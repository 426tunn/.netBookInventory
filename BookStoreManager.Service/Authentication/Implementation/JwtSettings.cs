using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreManager.Service.Authentication.Implementation
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string Secrets { get; init; } = null!;
        public string Issuer { get; init; }  = null!;
        public string Audience { get; init; }  = null!;
        public int ExpiryInHour { get; init; } 

    }
}