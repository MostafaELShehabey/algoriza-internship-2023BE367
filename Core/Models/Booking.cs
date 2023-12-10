using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Booking
    {
        [Key]
        public string Booking_Id { get; set; }
        public Status Booking_Status { get; set; }
        [ForeignKey("Doctor")]
        public string Dcotor_Id { get; set; }
        [ForeignKey("Patient")]
        public string Patient_Id { get; set; }
        [ForeignKey("Appointment")]
        public string Appointment_Id { get; set; }
        [ForeignKey("Discount")]
        public string Discount_Id { get; set; }
        // Navigation
        public Doctor Doctors { get; set; }
        public Patient Patient { get; set; }
        public Appointment Appointment { get; set; }
        public Discount Discount { get; set; }
    }
    public enum Status
{
    Confirmed,
    Pending,
    Cancelled
}
}
