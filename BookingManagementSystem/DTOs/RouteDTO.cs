using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagementSystem.DTOs
{
    public class AddRouteDTO
    {
        [Required]
        public string Departure { get; set; }

        [Required]
        public string Arrival { get; set; }
    }

    public class RouteDTO : AddRouteDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
