using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public int TrainId { get; set; }
        public Train Train { get; set; }

        public int RouteId { get; set; }
        public Route Route { get; set; }

        public decimal FirstClassPrice { get; set; }
        public decimal SecondClassPrice { get; set; }

        public int FirstClassAvailableBookings { get; set; }
        public int SecondClassAvailableBookings { get; set; }

    }
}
