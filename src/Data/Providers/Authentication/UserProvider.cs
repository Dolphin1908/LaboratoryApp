using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers.Common;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Constants;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class UserProvider : IUserProvider
    {
        private readonly IMongoDBProvider _mongoDb;

        public UserProvider(IEnumerable<IMongoDBProvider> mongoDb) => _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthenticationMongoDB);

        /// <summary>
        /// Lấy thông tin người dùng từ database theo username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<User?> GetByUsernameAsync(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);
            var user = _mongoDb.GetOne(CollectionName.Users, filter);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Lấy thông tin người dùng từ database theo email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns> 
        public Task<User?> GetByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            var user = _mongoDb.GetOne(CollectionName.Users, filter);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Lấy thông tin người dùng từ database theo số điện thoại
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            var filter = Builders<User>.Filter.Eq(u => u.PhoneNumber, phoneNumber);
            var user = _mongoDb.GetOne(CollectionName.Users, filter);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Lấy tất cả người dùng từ database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            var users = _mongoDb.GetAll<User>(CollectionName.Users);
            return users;
        }

        public User? GetUserById(long id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var user = _mongoDb.GetOne(CollectionName.Users, filter);
            return user;
        }

        public string GetUsernameById(long id)
        {
            var user = GetUserById(id);
            return user?.Username ?? "Guest";
        }

        public Task CreateNewUser(User user)
        {
            _mongoDb.Insert(CollectionName.Users, user);
            return Task.CompletedTask;
        }

        public long GetNextUserId()
        {
            var users = GetAllUsers();
            return users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
        }
    }
}
