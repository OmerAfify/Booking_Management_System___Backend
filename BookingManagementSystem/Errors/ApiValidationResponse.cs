using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagementSystem.Errors
{
    public class ApiValidationResponse : ApiResponse
    {

        public IEnumerable<string> Errors { get; set; }
        public ApiValidationResponse() : base(400)
        {

        }
    }
}
