using Core.Helpers;
using Core.Models;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeseetaApi.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterDTO model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);

        Task<AuthModel> RegisterPatientAsync(RegisterDTO model);
    }
}
