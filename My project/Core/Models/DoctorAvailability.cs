using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DoctorAvailability
    {
        [Key]
        public string Id { get; set; }

        public string DoctorId { get; set; }

        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Doctor Doctor { get; set; }
    }
}
