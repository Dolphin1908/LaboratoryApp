using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Data.Providers.Authentication;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;
using System.Security.Cryptography;
using System.Windows;
using LaboratoryApp.src.Core.Caches;

namespace LaboratoryApp.src.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterAsync(string username, string password, string confirmPassword, string email, string phoneNumber);
        Task<AuthenticationResponseDTO?> LoginAsync(string username, string password);
    }
}
