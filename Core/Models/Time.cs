using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Time
    {
        [Key]
        public string Id { get; set; }
        
        public DateTime _Time { get; set; }
        //Foreign Keys
        [ForeignKey("Appointment")]
        public string Appointment_Id { get; set; }
        // Navigation Properties
        public Appointment Appointment { get; set; }
    }

}



