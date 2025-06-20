﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers.Interfaces;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class UserProvider : IUserProvider
    {
        private readonly IMongoDBProvider _db;

        public UserProvider(IMongoDBProvider db) => _db = db;

        /// <summary>
        /// Lấy thông tin người dùng từ database theo username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<User?> GetByUsernameAsync(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);
            var user = _db.GetOne("users", filter);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Lấy tất cả người dùng từ database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            var users = _db.GetAll<User>("users");
            return users;
        }

        public User? GetUserById(long id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var user = _db.GetOne("users", filter);
            return user;
        }
    }
}
