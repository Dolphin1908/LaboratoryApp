using LaboratoryApp.Domain.Models.Users;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Authentication.Interface;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class OrganizationProvider : IOrganizationProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<Organization> _organizationCollection;

        public OrganizationProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthenticationMongoDB);
            _organizationCollection = _mongoDb.GetCollection<Organization>(CollectionName.Organizations);
        }

        /// <summary>
        /// Lấy tất cả tổ chức
        /// </summary>
        /// <returns></returns>
        public async Task<List<Organization>> GetAllOrganizationsAsync()
        {
            var organizations = await _organizationCollection.Find(FilterDefinition<Organization>.Empty).ToListAsync();
            return organizations;
        }

        /// <summary>
        /// Lấy tất cả tổ chức theo tên
        /// </summary>
        /// <param name="organizationName"></param>
        /// <returns></returns>
        public async Task<List<Organization>?> GetOrganizationByNameAsync(string organizationName)
        {
            var filter = Builders<Organization>.Filter.Eq(o => o.Name, organizationName);
            var organizations = await _organizationCollection.Find(filter).ToListAsync();
            return organizations;
        }

        /// <summary>
        /// Lấy tổ chức theo Id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<Organization?> GetOrganizationByIdAsync(long organizationId)
        {
            var filter = Builders<Organization>.Filter.Eq(o => o.Id, organizationId);
            var organization = await _organizationCollection.Find(filter).FirstOrDefaultAsync();
            return organization;
        }

        /// <summary>
        /// Tạo tổ chức mới
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public async Task CreateOrganizationAsync(Organization organization)
        {
            await _organizationCollection.InsertOneAsync(organization);
        }

        /// <summary>
        /// Cập nhật tổ chức
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public async Task UpdateOrganizationAsync(Organization organization)
        {
            await _organizationCollection.ReplaceOneAsync(o => o.Id == organization.Id, organization);
        }

        /// <summary>
        /// Xóa tổ chức
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task DeleteOrganizationAsync(long organizationId)
        {
            await _organizationCollection.DeleteOneAsync(o => o.Id == organizationId);
        }
    }
}
