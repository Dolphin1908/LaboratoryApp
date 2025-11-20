using LaboratoryApp.Domain.Models.Users;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Authentication.Interface;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class UserProvider : IUserProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<User> _userCollection;

        public UserProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthenticationMongoDB);
            _userCollection = _mongoDb.GetCollection<User>(CollectionName.Users);
        }

        /// <summary>
        /// Lấy thông tin người dùng từ database theo username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);

            // Truyền vào username và trả về user tương ứng
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// Lấy thông tin người dùng từ database theo email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);

            // Truyền vào email và trả về user tương ứng
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// Lấy thông tin người dùng từ database theo số điện thoại
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            var filter = Builders<User>.Filter.Eq(u => u.PhoneNumber, phoneNumber);

            // Truyền vào số điện thoại và trả về user tương ứng
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// Lấy tất cả người dùng từ Database
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _userCollection.Find(FilterDefinition<User>.Empty).ToListAsync();
            return users;
        }

        /// <summary>
        /// Lấy thông tin người dùng từ database theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByIdAsync(long id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);

            // Truyền vào userId và trả về user tương ứng
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// Tạo người dùng mới
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateNewUserAsync(User user)
        {
            await _userCollection.InsertOneAsync(user);
        }

        ///// <summary>
        ///// Lấy thông tin người dùng từ database theo username
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //public Task<User?> GetByUsernameAsync(string username)
        //{
        //    var filter = Builders<User>.Filter.Eq(u => u.Username, username);
        //    var user = _mongoDb.GetOne(CollectionName.Users, filter);
        //    return Task.FromResult(user);
        //}

        ///// <summary>
        ///// Lấy thông tin người dùng từ database theo email
        ///// </summary>
        ///// <param name="email"></param>
        ///// <returns></returns> 
        //public Task<User?> GetByEmailAsync(string email)
        //{
        //    var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        //    var user = _mongoDb.GetOne(CollectionName.Users, filter);
        //    return Task.FromResult(user);
        //}

        ///// <summary>
        ///// Lấy thông tin người dùng từ database theo số điện thoại
        ///// </summary>
        ///// <param name="phoneNumber"></param>
        ///// <returns></returns>
        //public Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        //{
        //    var filter = Builders<User>.Filter.Eq(u => u.PhoneNumber, phoneNumber);
        //    var user = _mongoDb.GetOne(CollectionName.Users, filter);
        //    return Task.FromResult(user);
        //}

        ///// <summary>
        ///// Lấy tất cả người dùng từ database
        ///// </summary>
        ///// <returns></returns>
        //public List<User> GetAllUsers()
        //{
        //    var users = _mongoDb.GetAll<User>(CollectionName.Users);
        //    return users;
        //}

        //public User? GetUserById(long id)
        //{
        //    var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        //    var user = _mongoDb.GetOne(CollectionName.Users, filter);
        //    return user;
        //}

        //public string GetUsernameById(long id)
        //{
        //    var user = GetUserById(id);
        //    return user?.Username ?? "Guest";
        //}

        //public Task CreateNewUser(User user)
        //{
        //    _mongoDb.Insert(CollectionName.Users, user);
        //    return Task.CompletedTask;
        //}

        //public long GetNextUserId()
        //{
        //    var users = GetAllUsers();
        //    return users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
        //}
    }
}
