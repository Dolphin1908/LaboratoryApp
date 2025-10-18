using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using LaboratoryApp.src.Constants;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.Assignment;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;
using LaboratoryApp.src.Core.Models.Authorization;
using LaboratoryApp.src.Core.Models.Authorization.Enums;

using LaboratoryApp.src.Data.Providers.Assignment;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Data.Providers.Authorization;

using LaboratoryApp.src.Services.Helper.Counter;

namespace LaboratoryApp.src.Services.Assignment
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentProvider _assignmentProvider;
        private readonly IAuthorizationCache _authorizationCache;
        private readonly IExerciseSetAccessProvider _exerciseSetAccessProvider;
        private readonly ICounterService _counterService;
        private readonly IUserProvider _userProvider;
        private readonly IAssignmentCache _assignmentCache;

        public AssignmentService (IAssignmentProvider assignmentProvider,
                                  IAuthorizationCache authorizationCache,
                                  IExerciseSetAccessProvider exerciseSetAccessProvider,
                                  ICounterService counterService,
                                  IUserProvider userProvider,
                                  IAssignmentCache assignmentCache)
        {
            _assignmentProvider = assignmentProvider;
            _authorizationCache = authorizationCache;
            _exerciseSetAccessProvider = exerciseSetAccessProvider;
            _counterService = counterService;
            _userProvider = userProvider;
            _assignmentCache = assignmentCache;
        }

        #region ExerciseSet
        /// <summary>
        /// Nhập mã code và mật khẩu để thêm bộ bài tập mới
        /// </summary>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool InsertNewExerciseSet(string code, string password)
        {
            var set = _assignmentCache.AllExerciseSets.FirstOrDefault(es => es.Code == code);

            if (set == null)
            {
                MessageBox.Show("Không tìm thấy bộ bài tập với mã code đã nhập!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(set.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu của bộ!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (!string.IsNullOrEmpty(set.Password) && SecureConfigHelper.Encrypt(password) != set.Password)
            {
                MessageBox.Show("Mật khẩu không đúng!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrEmpty(set.Password) && !string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Thông tin bộ bài tập không đúng!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var newAccess = new ExerciseSetAccess
            {
                ExerciseSetId = set.Id,
                UserId = AuthenticationCache.CurrentUser?.Id ?? 0,
                Level = AccessLevel.Attempt | AccessLevel.View
            };

            _exerciseSetAccessProvider.CreateNewAccess(newAccess);
            _authorizationCache.AllExerciseSetAccess.Add(newAccess);

            MessageBox.Show($"Thêm bộ bài tập '{set.Title}' thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            return true;
        }

        /// <summary>
        /// Lưu bộ bài tập mới
        /// </summary>
        /// <param name="set"></param>
        public void SaveNewExerciseSet(ExerciseSet set)
        {
            set.Id = _counterService.GetNextId(CollectionName.ExerciseSets);

            if (string.IsNullOrWhiteSpace(set.Title))
            {
                MessageBox.Show("Vui lòng nhập tên bộ!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            set.OwnerId = AuthenticationCache.CurrentUser?.Id ?? 0;
            set.Password = string.IsNullOrEmpty(set.Password) ? string.Empty : SecureConfigHelper.Encrypt(set.Password);
            set.Code = CodeGenerator.GenerateCode(8); // Tạo mã code 8 ký tự
            set.ExerciseIds = new List<long>();

            var newAccess = new ExerciseSetAccess
            {
                ExerciseSetId = set.Id,
                UserId = set.OwnerId,
                Level = AccessLevel.Owner | AccessLevel.Grade | AccessLevel.Edit | AccessLevel.Attempt | AccessLevel.View
            };

            // Lưu bộ bài tập
            try
            {
                _assignmentProvider.CreateNewExerciseSet(set);
                _exerciseSetAccessProvider.CreateNewAccess(newAccess);
                _authorizationCache.AllExerciseSetAccess.Add(newAccess);
                _assignmentCache.AllExerciseSets.Add(set);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu bộ bài tập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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

                var owner = _userProvider.GetUserById(set.OwnerId)!;
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
        #endregion

        #region Exercise
        /// <summary>
        /// Lưu bài tập mới
        /// </summary>
        /// <param name="set"></param>
        /// <param name="exercise"></param>
        public void SaveNewExercise(ExerciseSet set, Exercise exercise)
        {
            exercise.Id = _counterService.GetNextId(CollectionName.Exercises);

            if (string.IsNullOrWhiteSpace(exercise.Title))
            {
                MessageBox.Show("Vui lòng nhập tên bài tập!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            exercise.Password = string.IsNullOrEmpty(exercise.Password) ? string.Empty : SecureConfigHelper.Encrypt(exercise.Password);
            exercise.QuestionIds = new List<long>();

            try
            {
                set.ExerciseIds.Add(exercise.Id);
                set.UpdatedAt = DateTime.UtcNow;
                _assignmentCache.AllExercises.Add(exercise);
                _assignmentProvider.CreateNewExercise(exercise);
                _assignmentProvider.UpdateExerciseSet(set);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu bài tập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Lấy tất cả bài tập trong bộ bài tập theo Id
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public List<Exercise> GetAllExercisesBySetId(long setId)
        {
            var set = _assignmentCache.AllExerciseSets.FirstOrDefault(es => es.Id == setId);
            if (set == null) return new List<Exercise>();

            var exercises = _assignmentCache.AllExercises.Where(e => set.ExerciseIds.Contains(e.Id)).ToList();
            return exercises;
        }
        #endregion
    }
}
