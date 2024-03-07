using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreManager.Domain.Entities
{
    public class ApiResponse
    {
        public string message { get; set; }
        public int StatusCode { get; set; }
        public object? result { get; set; }

        public ApiResponse (string message, int StatusCode, object result){
            this.message = message;
            this.StatusCode = StatusCode;
            this.result = result;
        }
        //         public ApiResponse (string message, int StatusCode){
        //     this.message = message;
        //     this.StatusCode = StatusCode;
        // }
    }
}