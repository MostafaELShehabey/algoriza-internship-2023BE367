using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Models

{
    public class Appointment
    {
        [Key]
        public string Id { get; set; }
        [DataType(DataType.Time)]
        public DateTime AppointmentDay { get; set; }
        public A_Status AppointmentStatus { get; set; }

        // Foreign keys
        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        
        
        [ForeignKey("Time")]
        public string Time_Id { get; set; }

        // Navigation properties
        public Doctor Doctor { get; set; }
        //public Patient Patient { get; set; }
        public Discount Discount { get; set; }
        public ICollection<Time> Times { get; set; }
        public ICollection<Booking> Bookings { get; set; }


    }
    public enum A_Status
    {
        Availabale,
        NotAvailable
    }

}
