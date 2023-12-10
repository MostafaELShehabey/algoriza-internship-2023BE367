using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Doctor
    {
    public string Id { get; set; }
    public double Price { get; set; }

    // Foreign keys
    public string User_Id { get; set; }
    public string Specialize_Id { get; set; }

    // Navigation properties
    public ApplicationUser User { get; set; }
    public Specialization Specialization { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<DoctorAvailability> Availabilities { get; set; }

    }
}
