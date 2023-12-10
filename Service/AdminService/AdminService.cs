using Core.Helpers;
using Core.Models;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Repository;

namespace Service.AdminServices
{
    public class AdminService : IAdminService
    {
        private IAdminRepository<ApplicationUser> _repository;
        public AdminService(IAdminRepository<ApplicationUser> repository)
        {
            _repository = repository;
        }
        public int NumOFDoctors()
        {
            return _repository.NumOFDoctors();
        }
        public int NumOFPatients()
        {
            return _repository.NumOFPatients();
        }
        public int NumOfRequests()
        {
            return _repository.NumOfRequests();
        }
        public IEnumerable<Specialization> GetTopSpecializations(int topCount)
        {
            return _repository.GetTopSpecializations(topCount);
        }
        public IEnumerable<Doctor> GetTopDoctors(int topCount)
        {
            return _repository.GetTopDoctors(topCount);
        }
        public async Task<List<UserResponseModel>> GetDoctorsAsync(int page, int pageSize, string search)
        {
            return await _repository.GetDoctorsAsync(page, pageSize, search);
        }

        public async Task<UserResponseModel> GetDoctorByIdAsync(string id)
        {
            return await _repository.GetDoctorByIdAsync(id);
        }
        public async Task<bool> DeleteDoctorByIdAsync(string id)
        {
            return await _repository.DeleteDoctorByIdAsync(id);
        }


        public async Task<bool> UpdateDoctorAsync(string id, string firstName, string lastName, string email, string phone, string specialize, int gender, DateTime dateOfBirth)
        {
            return await _repository.UpdateDoctorAsync(id, firstName, lastName, email, phone, specialize, gender, dateOfBirth);
        }

        public async Task<List<UserResponseModel>> GetPatientsAsync(int page, int pageSize, string search)
        {
            return await _repository.GetPatientsAsync(page, pageSize, search);
        }

        public async Task<UserResponseModel> GetPatientByIdAsync(string id)
        {
            return await _repository.GetPatientByIdAsync(id);
        }

    }
}
