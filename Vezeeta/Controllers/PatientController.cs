using Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.PatientService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeseetaApi.Services;

namespace VeseetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPatientService _patientService;
        public PatientController(IAuthService authService, IPatientService patientService)
        {
            _authService = authService;
            _patientService = patientService;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(new
            {
                Token = result.Token,
                ExpiresOn = result.ExpiresOn,
                Email = result.Email,
                UserName = result.Username,
                Registered = result.IsAuthenticated
            });
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

        [HttpGet("getalldoctors")]
        public IActionResult GetAllDoctors(int page = 1, int pageSize = 10, string search = "")
        {
            try
            {
                var doctors = _patientService.GetDoctors(page, pageSize, search);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Booking")]
        public IActionResult BookAppointment(string timeId , string userId)
        {


            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated.");
            }

            // Call the repository method to attempt the booking
            bool isBookingSuccessful = _patientService.BookAppointment(userId, timeId);

            if (isBookingSuccessful)
            {
                return Ok(true);
            }

            // Booking failed (e.g., time already booked)
            return BadRequest("Booking unsuccessful. The selected time slot is not available.");
        }

        //
        [HttpGet("GetPatientBookings/{patientId}")]
        public  IActionResult GetPatientBookings(string patientId)
        {
            var patientBookings =  _patientService.GetPatientBookings(patientId);


            return Ok(patientBookings);
        }

    }
}
