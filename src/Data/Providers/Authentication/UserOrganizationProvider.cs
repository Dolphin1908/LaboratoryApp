using LaboratoryApp.Domain.Models.Users;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Authentication.Interface;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class UserOrganizationProvider : IUserOrganizationProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<UserOrganization> _userOrganizationCollection;

        public UserOrganizationProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthenticationMongoDB);
            _userOrganizationCollection = _mongoDb.GetCollection<UserOrganization>(CollectionName.UserOrganization);
        }

        /// <summary>
        /// Lấy tất cả liên kết người dùng - tổ chức (Không dùng hoặc ít dùng)
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserOrganization>> GetAllUserOrganizationsAsync()
        {
            var userOrganizations = await _userOrganizationCollection.Find(FilterDefinition<UserOrganization>.Empty).ToListAsync();
            return userOrganizations;
        }

        /// <summary>
        /// Lấy tất cả liên kết người dùng - tổ chức theo userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserOrganization>> GetUserOrganizationsByUserIdAsync(long userId)
        {
            var filter = Builders<UserOrganization>.Filter.Eq(uo => uo.UserId, userId);
            var userOrganizations = await _userOrganizationCollection.Find(filter).ToListAsync();
            return userOrganizations;
        }

        /// <summary>
        /// Lấy tất cả liên kết người dùng - tổ chức theo organizationId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<List<UserOrganization>> GetUserOrganizationsByOrganizationIdAsync(long organizationId)
        {
            var filter = Builders<UserOrganization>.Filter.Eq(uo => uo.OrganizationId, organizationId);
            var userOrganizations = await _userOrganizationCollection.Find(filter).ToListAsync();
            return userOrganizations;
        }

        /// <summary>
        /// Tạo liên kết người dùng - tổ chức
        /// </summary>
        /// <param name="userOrganization"></param>
        /// <returns></returns>
        public async Task CreateUserOrganizationAsync(UserOrganization userOrganization)
        {
            await _userOrganizationCollection.InsertOneAsync(userOrganization);
        }

        /// <summary>
        /// Cập nhật liên kết người dùng - tổ chức
        /// </summary>
        /// <param name="userOrganization"></param>
        /// <returns></returns>
        public async Task UpdateUserOrganizationAsync(UserOrganization userOrganization)
        {
            await _userOrganizationCollection.ReplaceOneAsync(uo => uo.Id == userOrganization.Id, userOrganization);
        }

        /// <summary>
        /// Xóa liên kết người dùng - tổ chức
        /// </summary>
        /// <param name="userOrganizationId"></param>
        /// <returns></returns>
        public async Task DeleteUserOrganizationAsync(long userOrganizationId)
        {
            await _userOrganizationCollection.DeleteOneAsync(uo => uo.Id == userOrganizationId);
        }
    }
}