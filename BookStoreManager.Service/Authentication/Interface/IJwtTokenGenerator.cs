using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreManager.Service.Authentication.Interface
{
    public interface IJwtTokenGenerator
    {
         string GeneratedToken(Guid userId, string firstname, string lastname);
         
    }
}