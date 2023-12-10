using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
    public class Patient
    {
        [Key]
        public string Id { get; set; }

        // Foreign keys
        [ForeignKey("ApplicationUser")]
        public string User_Id { get; set; }
        [ForeignKey("Discount")]
        public string Discount_Id { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; }
        public Discount Discount { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Booking> Bookings { get; set; }

    }
}
