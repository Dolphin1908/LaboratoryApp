using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Caches.Assignment;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Data.Providers.Assignment.ExerciseSetFunction;
using LaboratoryApp.src.Data.Providers.Authentication.Interface;
using LaboratoryApp.src.Data.Providers.Authorization;
using LaboratoryApp.src.Services.Helper.Counter;

namespace LaboratoryApp.src.Services.Assignment.ExerciseSetFunction
{
    public class ExerciseSetService : IExerciseSetService
    {
        private readonly IAssignmentCache _assignmentCache;
        private readonly IAuthorizationCache _authorizationCache;
        private readonly ICounterService _counterService;
        private readonly IExerciseSetAccessProvider _exerciseSetAccessProvider;
        private readonly IExerciseSetProvider _exerciseSetProvider;
        private readonly IUserProvider _userProvider;

        public ExerciseSetService(IAssignmentCache assignmentCache,
                                  IAuthorizationCache authorizationCache,
                                  ICounterService counterService,
                                  IExerciseSetAccessProvider exerciseSetAccessProvider,
                                  IExerciseSetProvider exerciseSetProvider,
                                  IUserProvider userProvider)
        {
            _assignmentCache = assignmentCache;
            _authorizationCache = authorizationCache;
            _counterService = counterService;
            _exerciseSetAccessProvider = exerciseSetAccessProvider;
            _exerciseSetProvider = exerciseSetProvider;
            _userProvider = userProvider;
        }

        /// <summary>
        /// Nhập mã code và mật khẩu để thêm bộ bài tập mới
        /// </summary>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool InsertNewExerciseSet(string code, string password)
        {

            return true;
        }

        /// <summary>
        /// Lưu bộ bài tập mới
        /// </summary>
        /// <param name="set"></param>
        public void SaveNewExerciseSet(ExerciseSet set)
        {

        }

        /// <summary>
        /// Lấy tất cả bộ bài tập mà người dùng có quyền truy cập
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ExerciseSet> GetAllExerciseSetsByUserId(long userId)
        {
            var accessList = _authorizationCache.AllExerciseSetAccess.Where(esa => esa.UserId == userId);
            var results = new List<ExerciseSet>();

            foreach (var access in accessList)
            {
                var set = _assignmentCache.AllExerciseSets.FirstOrDefault(es => es.Id == access.ExerciseSetId); // Tìm bộ bài tập theo Id trong danh sách đã cache

                var owner = _userProvider.GetUserByIdAsync(set!.OwnerId).GetAwaiter().GetResult();
                set.OwnerInfo = new UserDTO
                {
                    Id = owner.Id,
                    Username = owner.Username
                };
                results.Add(set);
            }

            return results;
        }

        public void DeleteExerciseSet(long setId)
        {

        }
    }
}