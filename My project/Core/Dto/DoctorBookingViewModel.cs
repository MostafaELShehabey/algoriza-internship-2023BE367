using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class DoctorBookingViewModel
    {
        public string PatientName { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DoctorBookingAppointmentViewModel Appointment { get; set; }
    }

}
