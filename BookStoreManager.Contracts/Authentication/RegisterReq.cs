using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreManager.Contracts.Authentication
{
    public record RegisterReq
    (
        string Firstname,
        string Lastname,
        string Email,
        string Password

    ) ;
    
}