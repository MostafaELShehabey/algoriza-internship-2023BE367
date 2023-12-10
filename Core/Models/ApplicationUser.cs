using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public string User_FullName { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Address { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public AccountType AccountType { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Male,
        Female
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AccountType
    {
        Doctor,
        Patient,
        Admin
    }
}
