using Core.Models;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Dto;

namespace Repository.DoctorRepository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DoctorRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
        public async Task<List<Booking>> GetDoctorBookingsAsync(string doctorId, int pageSize, int pageNumber)
        {
            return await _dbContext.Bookings
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .ThenInclude(a => a.Doctor)
                .Where(b => b.Doctors.Id == doctorId)
                .OrderByDescending(b => b.Appointment.AppointmentDay)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        //
        public bool ConfirmCheckup(string bookingId)
        {
           
            var booking = _dbContext.Bookings.FirstOrDefault(b => b.Booking_Id == bookingId);

            if (booking != null)
            {
                booking.Booking_Status = Status.Confirmed;
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        //
        public async Task<bool> CreateDoctorAppointment(DoctorAppointmentDTO doctorAppointmentDTO , string doctorId)
        {
            // Your implementation logic here
            try
            {

                // Create a new appointment
                var appointment = new Appointment
                {
                    Id = Guid.NewGuid().ToString(), // Generate a unique ID for the appointment
                    AppointmentDay = DateTime.Now, // You can set the initial day to the current date
                    AppointmentStatus = A_Status.Availabale,
                    DoctorId = doctorId,
                    Times = new List<Time>(), // Initialize the list of times
                };

                // Set the price for the doctor
                var doctor = await _dbContext.Set<Doctor>().FindAsync(doctorId);
                if (doctor != null)
                {
                    doctor.Price = doctorAppointmentDTO.Price;
                }

                // Add the days and times to the appointment
                foreach (var dayAvailability in doctorAppointmentDTO.DaysAvailability)
                {
                    var day = dayAvailability.Day;
                    var availableTimes = dayAvailability.AvailableTimes;

                    foreach (var availableTime in availableTimes)
                    {
                        // Create a new time slot
                        var time = new Time
                        {
                            Id = Guid.NewGuid().ToString(), // Generate a unique ID for the time slot
                            _Time = new DateTime(2000, 1, 1).Add(availableTime),
                        };

                        // Add the time slot to the appointment
                        appointment.Times.Add(time);
                    }
                }

                // Add the appointment to the database
                _dbContext.Appointments.Add(appointment);

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //
        public async Task<bool> UpdateDoctorAvailability(string doctorId, string timeId, TimeSpan newTime)
        {
            try
            {
                // Check if the time slot is already booked
                var isTimeSlotBooked = await _dbContext.Set<Booking>()
                    .AnyAsync(b => b.Dcotor_Id == doctorId && b.Appointment.AppointmentDay.TimeOfDay == newTime);

                if (isTimeSlotBooked)
                {
                    // Time slot is already booked, cannot update
                    return false;
                }

                // Get the doctor
                var doctor = await _dbContext.Set<Doctor>()
                    .Include(d => d.Availabilities)
                    .FirstOrDefaultAsync(d => d.Id == doctorId);

                if (doctor == null)
                {
                    // Doctor not found
                    return false;
                }

                // Check if the time slot is already in the doctor's availabilities
                var existingAvailability = doctor.Availabilities.FirstOrDefault(a => a.StartTime == newTime);

                if (existingAvailability != null)
                {
                    // Time slot already exists in the doctor's availabilities, update it
                    existingAvailability.StartTime = newTime;
                    existingAvailability.EndTime = newTime.Add(new TimeSpan(1, 0, 0)); // Assuming one-hour time slots
                }
                else
                {
                    // Time slot doesn't exist, add a new availability
                    doctor.Availabilities.Add(new DoctorAvailability
                    {
                        Id = Guid.NewGuid().ToString(),
                        DoctorId = doctorId,
                        StartTime = newTime,
                        EndTime = newTime.Add(new TimeSpan(1, 0, 0)), // Assuming one-hour time slots
                    });
                }

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //
        public bool DeleteDoctorTime(string doctorId, string timeId)
        {
            // Check if the time is already booked
            bool isTimeBooked = _dbContext.Set<Booking>().Any(b => b.Dcotor_Id == doctorId && b.Appointment.Time_Id == timeId);

            if (isTimeBooked)
            {
                // Time is already booked, cannot delete
                return false;
            }

            // Time is not booked, proceed with deletion
            var doctor = _dbContext.Set<Doctor>().Include(d => d.Availabilities).FirstOrDefault(d => d.Id == doctorId);

            if (doctor != null)
            {
                var availabilityToRemove = doctor.Availabilities.FirstOrDefault(da => da.Id == timeId);

                if (availabilityToRemove != null)
                {
                    doctor.Availabilities.Remove(availabilityToRemove);

                    // Save changes to the database
                    _dbContext.SaveChanges();
                    return true; // Deletion successful
                }
            }

            return false; // Doctor or time not found
        }

        Task<List<Booking>> IDoctorRepository.GetDoctorBookingsAsync(string doctorId, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

    }
}
