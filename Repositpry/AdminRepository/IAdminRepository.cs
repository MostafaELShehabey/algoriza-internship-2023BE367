using Core.Helpers;
using Core.Models;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface IAdminRepository<T> where T : class
    {
        int NumOFDoctors();
        int NumOFPatients();
        int NumOfRequests();
        IEnumerable<Specialization> GetTopSpecializations(int topCount);
        IEnumerable<Doctor> GetTopDoctors(int topCount);
        Task<List<UserResponseModel>> GetDoctorsAsync(int page, int pageSize, string search);
        Task<UserResponseModel> GetDoctorByIdAsync(string id);
        Task<bool> DeleteDoctorByIdAsync(string id);
        Task<bool> UpdateDoctorAsync(string id, string firstName, string lastName, string email, string phone, string specialize, int gender, DateTime dateOfBirth);
        Task<List<UserResponseModel>> GetPatientsAsync(int page, int pageSize, string search);
        Task<UserResponseModel> GetPatientByIdAsync(string id);
    }
}
