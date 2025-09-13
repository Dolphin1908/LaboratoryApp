using LaboratoryApp.src.Core.Models.Authentication;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.Authentication.Interfaces
{
    public interface IUserProvider
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        List<User> GetAllUsers();
        User? GetUserById(long id);
        long GetNextUserId();
    }
}
