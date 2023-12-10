using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.AdminServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using VeseetaApi.Services;
using Core.Dto;
namespace VeseetaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService, IAuthService authService)
        {
            _adminService = adminService;
            _authService = authService;
        }

        //Get Number Of Doctors
        [HttpGet("NumOfDoctor")]
        public IActionResult NumOFDoctors()
        {
            var result = _adminService.NumOFDoctors();
            return Ok(result);
        }

        //Get Number Of Patients
        [HttpGet("NumOfPatients")]
        public IActionResult NumOfPatients()
        {
            var res = _adminService.NumOFPatients();
            return Ok(res);
        }

        //Get Number Of Requests
        [HttpGet("NumOfRequests")]
        public IActionResult NumOfRequests()
        {
            var res = _adminService.NumOfRequests();
            return Ok(res);
        }

        //Get Top Specializations as you want 
        [HttpGet("GetTopSpecializations")]
        public IActionResult GetTopSpecializations(int topCount)
        {
            var res = _adminService.GetTopSpecializations(topCount);
            return Ok(res);
        }

        //Get Top Doctors as you want
        [HttpGet("GetTopDoctors")]
        public IActionResult GetTopDoctors(int topCount)
        {
            var topDocs = _adminService.GetTopDoctors(topCount);
            return Ok(topDocs);
        }

        //Get All Doctors
        [HttpGet("GetAllDoctors")]

        public async Task<ActionResult<List<UserResponseModel>>> GetDoctors([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var doctors = await _adminService.GetDoctorsAsync(page, pageSize, search);
            return Ok(doctors);
        }
        [HttpGet("GetDoctorById")]
        public async Task<IActionResult> GetDoctorByIdAsync(string id)
        {
            if (id is null)
            {
                return BadRequest("Please Enter id");
            }
            var doctor = await _adminService.GetDoctorByIdAsync(id);
            return Ok(doctor);
        }

        // Add Doctor 
        [HttpPost("AddDoctor")]
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
        [HttpDelete("doctors/{id}")]
        public async Task<ActionResult<bool>> DeleteDoctorById(string id)
        {
            var isDeleted = await _adminService.DeleteDoctorByIdAsync(id);

            if (!isDeleted)
            {
                return NotFound(); // Doctor not found or deletion failed
            }

            return Ok(isDeleted); // Successfully deleted the doctor
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(string id, string firstName, string lastName, string email, string phone, string specialize, int gender, DateTime dateOfBirth)
        {
            var success = await _adminService.UpdateDoctorAsync(id,firstName, lastName,email,phone,specialize,gender,dateOfBirth);

            return Ok(new { Success = success });
        }

        [HttpGet("GetAllPatients")]

        public async Task<ActionResult<List<UserResponseModel>>> GetAllPatients([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var patients = await _adminService.GetPatientsAsync(page, pageSize, search);
            return Ok(patients);
        }

        [HttpGet("GetPatientById")]
        public async Task<IActionResult> GetPatientByIdAsync(string id)
        {
            if (id is null)
            {
                return BadRequest("Please Enter id");
            }
            var patient = await _adminService.GetPatientByIdAsync(id);
            return Ok(patient);
        }


    }
}
