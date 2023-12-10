
using Core.Dto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DoctorService
{
    public interface IDoctorService
    {
        Task<List<Booking>> GetDoctorBookingsAsync(string doctorId, int pageSize, int pageNumber);
        bool ConfirmCheckup(string bookingId);
        Task<bool> CreateDoctorAppointment(DoctorAppointmentDTO doctorAppointmentDTO, string doctorId);
        Task<bool> UpdateDoctorAvailability(string doctorId, string timeId, TimeSpan newTime);
        bool DeleteDoctorTime(string doctorId, string timeId);
    }
}
