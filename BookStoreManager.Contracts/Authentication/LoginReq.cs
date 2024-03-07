using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreManager.Contracts.Authentication
{
    public record LoginReq
    (
        string Email,
        string Password
   );
}