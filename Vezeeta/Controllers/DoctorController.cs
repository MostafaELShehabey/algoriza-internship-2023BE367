using Core.Helpers;
using Core.Models;
using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Context;
using Repository.DoctorRepository;
using Service.AdminServices;
using ServiceLayer.DoctorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VeseetaApi.Services;

namespace VeseetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDoctorService _doctorService;
        private readonly IAdminService _adminService;


        public DoctorController(IAuthService authService, IDoctorService doctorService, IAdminService adminService)
        {
            _authService = authService;
            _doctorService = doctorService;
            _adminService = adminService;

        }

        [HttpPost("Login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<DoctorBookingViewModel>>> GetDoctorBookings(string doctorId ,int pageSize, int pageNumber)
        {

            // Retrieve the doctor using the repository
            var doctor = await _adminService.GetDoctorByIdAsync(doctorId);

            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            // Get paginated bookings using the repository
            var doctorBookings = await _doctorService.GetDoctorBookingsAsync(doctorId, pageSize, pageNumber);

            // Map the data to a view model
            var doctorBookingViewModels = doctorBookings.Select(b => new DoctorBookingViewModel
            {
                PatientName = b.Patient.User.User_FullName,
                Gender = b.Patient.User.Gender,
                Phone = b.Patient.User.Phone,
                Email = b.Patient.User.Email,
                Appointment = new DoctorBookingAppointmentViewModel
                {
                    AppointmentDay = b.Appointment.AppointmentDay,
                    AppointmentStatus = b.Appointment.AppointmentStatus
                }
            }).ToList();

            return doctorBookingViewModels;
        }

        //
        [HttpPost("ConfirmCheckup")]
        public IActionResult ConfirmCheckup(string bookingId)
        {
            if (string.IsNullOrEmpty(bookingId))
            {
                return BadRequest("Invalid request");
            }

            bool isConfirmed = _doctorService.ConfirmCheckup(bookingId);

            return Ok(new { IsConfirmed = isConfirmed });
        }

        //
        [HttpPost("create-appointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] DoctorAppointmentDTO doctorAppointmentDTO, string doctorId)
        {
            try
            {
                var result = await _doctorService.CreateDoctorAppointment(doctorAppointmentDTO, doctorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost("update-availability")]
        public async Task<IActionResult> UpdateDoctorAvailability([FromBody] DoctorUpdateAvailabilityDTO updateAvailabilityDTO)
        {
            try
            {
                var result = await _doctorService.UpdateDoctorAvailability(updateAvailabilityDTO.DoctorId, updateAvailabilityDTO.TimeId, updateAvailabilityDTO.NewTime);

                if (result)
                {
                    return Ok("Doctor availability updated successfully");
                }
                else
                {
                    return BadRequest("Cannot update doctor availability. The time slot may already be booked.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }

        //
        [HttpDelete("{doctorId}/times/{timeId}")]
        public IActionResult DeleteDoctorTime(string doctorId, string timeId)
        {
            bool deleted = _doctorService.DeleteDoctorTime(doctorId, timeId);

            if (deleted)
            {
                return Ok(); // 204 No Content
            }
            else
            {
                return NotFound(); // 404 Not Found or you can return BadRequest() depending on your requirements
            }
        }




    }

}
