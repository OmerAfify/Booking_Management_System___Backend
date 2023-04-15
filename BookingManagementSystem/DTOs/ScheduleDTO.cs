using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagementSystem.DTOs
{
    public class AddScheduleDTO
    {
        [Required]
        public int TrainId { get; set; }

        [Required]
        public int RouteId { get; set; }
        
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Range(1,10000,ErrorMessage ="First Class range value must be between 1 and 10000")]
        public decimal FirstClassPrice { get; set; }

        [Range(1, 10000, ErrorMessage = "Second Class range value must be between 1 and 10000")]
        public decimal SecondClassPrice { get; set; }

    }


    public class ScheduleDTO : AddScheduleDTO
    {
        public int Id { get; set; }

        public string TrainName { get; set; }
        public string Route { get; set; }

        public int FirstClassAvailableBookings { get; set; }
        public int SecondClassAvailableBookings { get; set; }
    }

}
