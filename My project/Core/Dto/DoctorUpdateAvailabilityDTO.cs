using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class DoctorUpdateAvailabilityDTO
    {
        public string DoctorId { get; set; }
        public string TimeId { get; set; }
        public TimeSpan NewTime { get; set; }
    }

}
