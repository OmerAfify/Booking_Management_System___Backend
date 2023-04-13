using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagementSystem.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {

        public string Details { get; set; }

        public ApiExceptionResponse(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }


    }
}
