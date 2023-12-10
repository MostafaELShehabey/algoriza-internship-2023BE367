using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Discount
    {
        [Key]
        public string Id { get; set; }
        public string Discount_Code { get; set; }
        public string Discount_Type { get; set; }
        // Other properties...

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Patient> Patients { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
