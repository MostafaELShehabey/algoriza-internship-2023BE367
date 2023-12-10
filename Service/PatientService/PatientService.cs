
using Core.Dto;
using Repository.PatientRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.PatientService
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public IEnumerable<DoctorDto> GetDoctors(int page, int pageSize, string search)
        {
            return _patientRepository.GetDoctors(page, pageSize, search);
        }
        public bool BookAppointment(string patientId, string timeId)
        {
            return _patientRepository.BookAppointment(patientId, timeId);
        }
        public IEnumerable<BookingDetailsDto> GetPatientBookings(string patientId)
        {
            return _patientRepository.GetPatientBookings(patientId);
        }
    }
}
