using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagementSystem.DTOs
{
    public class AddTrainDTO
    {
        public string Name { get; set; }
        public int FirstClassSeats { get; set; }
        public int SecondClassSeats { get; set; }
    }

    public class ReturnTrainDTO : AddTrainDTO
    {
        public int Id { get; set; }
    }


   
}
