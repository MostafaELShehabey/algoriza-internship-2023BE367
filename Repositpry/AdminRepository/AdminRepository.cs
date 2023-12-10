using Core.Helpers;
using Core.Models;
using Core.Dto;
using Microsoft.EntityFrameworkCore;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Context;

namespace Repository.Repository
{
    public class AdminRepository<T> : IAdminRepository<T> where T : class
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private DbSet<T> Entities;
        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            //Entities = _applicationDbContext.Set<T>();
        }

        public IEnumerable<ApplicationUser> GetUsersByAccountType(AccountType accountType)
        {
            var result = _applicationDbContext.Set<ApplicationUser>()
                .Where(u => u.AccountType == accountType).ToList();

            return result;

        }
        public int NumOFDoctors()
        {
            return GetUsersByAccountType(AccountType.Doctor).ToList().Count();
        }
        public int NumOFPatients()
        {
            return GetUsersByAccountType(AccountType.Patient).ToList().Count();
        }
        public int NumOfRequests()
        {
            return _applicationDbContext.Set<Appointment>().ToList().Count();
        }
        public IEnumerable<Specialization> GetTopSpecializations(int topCount)
        {
            return _applicationDbContext.Set<Specialization>()
                .OrderByDescending(s => s.Doctors.Count())
                .Take(topCount)
                .ToList();
        }
        public IEnumerable<Doctor> GetTopDoctors(int topCount)
        {
            return _applicationDbContext.Set<Doctor>()
                .OrderByDescending(d => d.Appointments.Count())
                .Take(topCount)
                .ToList();
        }


            public async Task<List<UserResponseModel>> GetDoctorsAsync(int page, int pageSize, string search)
            {
                var query = _applicationDbContext.Set<ApplicationUser>()
                    .Where(u => u.AccountType == AccountType.Doctor);

                if (!string.IsNullOrEmpty(search))
                {
                    // Add search criteria based on your requirements
                    query = query.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
                }

                var doctors = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UserResponseModel
                    {
                        FullName = u.User_FullName,
                        Email = u.Email,
                        Phone = u.Phone,
                        Gender = u.Gender,
                        DateOfBirth = u.DOB
                    })
                    .ToListAsync();

                return doctors;
            }

        public async Task<UserResponseModel> GetDoctorByIdAsync(string id)
        {
            var doctor = await _applicationDbContext.Set<ApplicationUser>()
                .Where(u => u.Id == id && u.AccountType == AccountType.Doctor)
                .Select(u => new UserResponseModel
                {
                    FullName = u.User_FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Specialize = u.Doctor.Specialization.Specialize_Name,
                    Gender = u.Gender,
                    DateOfBirth = u.DOB
                })
                .FirstOrDefaultAsync();

            return doctor;
        }
        public async Task<bool> DeleteDoctorByIdAsync(string id)
        {
            var doctorToDelete = await _applicationDbContext.Set<ApplicationUser>() 
                .Where(u => u.Id == id && u.AccountType == AccountType.Doctor)
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync();

            if (doctorToDelete == null)
            {
                return false; // Doctor not found
            }

            // Remove related entities (e.g., appointments, times) if needed
            // ...

            _applicationDbContext.Users.Remove(doctorToDelete);
            await _applicationDbContext.SaveChangesAsync();

            return true; // Successfully deleted the doctor
        }

        public async Task<bool> UpdateDoctorAsync(string id, string firstName, string lastName, string email, string phone, string specialize, int gender, DateTime dateOfBirth)
        {
            try
            { 
                var user = await _applicationDbContext.Set<ApplicationUser>().FindAsync(id);

                if (user == null)
                {
                    return false;
                }

                // Update user properties
                user.User_FullName = firstName  + lastName;
                user.Email = email;
                user.Phone = phone;
                user.DOB = dateOfBirth;
                user.Gender = (Gender)gender; // Assuming GenderEnum is defined as an enum

                // Find the associated Doctor
                var doctor = await _applicationDbContext.Set<Doctor>().SingleOrDefaultAsync(d => d.User_Id == id);

                if (doctor == null)
                {
                    return false;
                }

                // Update Doctor properties
                doctor.Price = 0; // Set the desired default value for the price
                doctor.Specialize_Id = specialize; // Assuming specialize is the specialization id

                // Save changes to the database
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // Handle exceptions as needed
                return false;
            }
        }

        public async Task<List<UserResponseModel>> GetPatientsAsync(int page, int pageSize, string search)
        {
            var query = _applicationDbContext.Set<ApplicationUser>()
                .Where(u => u.AccountType == AccountType.Patient);

            if (!string.IsNullOrEmpty(search))
            {
                // Add search criteria based on your requirements
                query = query.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
            }

            var Patients = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponseModel
                {
                    FullName = u.User_FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Gender = u.Gender,
                    DateOfBirth = u.DOB
                })
                .ToListAsync();

            return Patients;
        }

        public async Task<UserResponseModel> GetPatientByIdAsync(string id)
        {
            var patient = await _applicationDbContext.Set<ApplicationUser>()
                .Where(u => u.Id == id && u.AccountType == AccountType.Patient)
                .Select(u => new UserResponseModel
                {
                    FullName = u.User_FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Specialize = u.Doctor.Specialization.Specialize_Name,
                    Gender = u.Gender,
                    DateOfBirth = u.DOB
                })
                .FirstOrDefaultAsync();

            return patient;
        }

    }
}
