using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class BookingDetailsDto
    {
        public string DoctorName { get; set; }
        public string Specialize { get; set; }
        public DateTime Day { get; set; }
        public TimeSpan Time { get; set; }
        public double Price { get; set; }
        public Status Status { get; set; }
    }

}
