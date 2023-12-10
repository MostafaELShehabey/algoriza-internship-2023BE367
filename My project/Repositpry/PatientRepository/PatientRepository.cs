using Core.Helpers;
using Core.Models;
using Core.Dto;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository.PatientRepository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PatientRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public IEnumerable<DoctorDto> GetDoctors(int page, int pageSize, string search)
        {
            var query = _dbContext.Set<Doctor>()
                .Include(d => d.User)
                .Include(d => d.Specialization)
                .Include(d => d.Appointments)
                    .ThenInclude(a => a.Times)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.User.User_FullName.Contains(search));
            }

            var doctors = query
            .OrderBy(d => d.User.User_FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DoctorDto
            {
                FullName = d.User.User_FullName,
                Email = d.User.Email,
                Phone = d.User.Phone,
                Specialize = d.Specialization.Specialize_Name,
                Price = d.Price,
                Gender = d.User.Gender,
                Appointments = d.Appointments.Select(a => new AppointmentDto
                {
                    Day = a.AppointmentDay,
                    Times = a.Times.Select(t => new TimeDto
                    {
                        Id = t.Id,
                        Time = t._Time
                    }).ToList()
                }).ToList()
            })
                                 .ToList();

            return doctors;
        }
    
        //

        public bool BookAppointment(string patientId, string timeId)
        {
            // Check if the time slot is available (no existing booking)
            var existingBooking = _dbContext.Bookings
                .FirstOrDefault(b => b.Appointment_Id == timeId);

            if (existingBooking == null)
            {
                // Create a new Booking entity
                var booking = new Booking
                {
                    Booking_Id = Guid.NewGuid().ToString(),
                    Patient_Id = patientId,
                    Appointment_Id = timeId,
                    // other properties...
                };

                // Add the booking to the database
                _dbContext.Bookings.Add(booking);

                // Save changes
                _dbContext.SaveChanges();

                return true;
            }

            // Booking failed, time slot is already booked
            return false;
        }

        //
        public async Task<List<BookingDetailsDto>> GetPatientBookingsAsync(string patientId)
        {
            var patientBookings = await _dbContext.Bookings
                .Include(b => b.Appointment)
                    .ThenInclude(a => a.Doctor)
                        .ThenInclude(d => d.User)
                .Where(b => b.Patient_Id == patientId)
                .Select(b => new BookingDetailsDto
                {
                    DoctorName = b.Doctors.User.User_FullName,
                    Day = b.Appointment.AppointmentDay,
                    Price = b.Doctors.Price,
                    Status = b.Booking_Status,
                // Add other properties as needed
            })
                .ToListAsync();

            return patientBookings;
        }

        public IEnumerable<BookingDetailsDto> GetPatientBookings(string patientId)
        {
            throw new NotImplementedException();
        }
    }
}
