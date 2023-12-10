using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class AppointmentDto
    {
        public string PatientName { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime Day { get; set; }
        public List<TimeDto> Times { get; set; }
    }
}
