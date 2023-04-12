using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Train
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FirstClassSeats { get; set; }
        public int SecondClassSeats { get; set; }

    }
}
