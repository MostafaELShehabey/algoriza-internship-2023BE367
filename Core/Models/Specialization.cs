using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Specialization
    {
        [Key]
        public string Id { get; set; }
        public string Specialize_Name { get; set; }

        // Navigation properties
        public ICollection<Doctor> Doctors { get; set; }

        //public static implicit operator Specialization(ApplicationUser v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
