using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagementSystem.DTOs
{
    public class AddTrainDTO
    {
        [Required]
        public string Name { get; set; }


        [Range(10,1000,ErrorMessage ="First Class Seat Range must be between 10 and 1000")]
        public int FirstClassSeats { get; set; }

        [Range(10, 1000, ErrorMessage = "Second Class Seat Range must be between 10 and 1000")]
        public int SecondClassSeats { get; set; }
    }

    public class ReturnTrainDTO : AddTrainDTO
    {
        public int Id { get; set; }
    }


   
}
