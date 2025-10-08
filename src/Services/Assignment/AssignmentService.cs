using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;
using LaboratoryApp.src.Core.Models.Authorization;
using LaboratoryApp.src.Core.Models.Authorization.Enums;
using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Data.Providers.Assignment;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Data.Providers.Authorization;
using LaboratoryApp.src.Data.Providers.Interfaces;
using LaboratoryApp.src.Services.Counter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LaboratoryApp.src.Services.Assignment
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentProvider _assignmentProvider;
        private readonly IExerciseSetAccessProvider _exerciseSetAccessProvider;
        private readonly ICounterService _counterService;
        private readonly IUserProvider _userProvider;

        public AssignmentService (IAssignmentProvider assignmentProvider,
                                  IExerciseSetAccessProvider exerciseSetAccessProvider,
                                  ICounterService counterService,
                                  IUserProvider userProvider)
        {
            _assignmentProvider = assignmentProvider;
            _exerciseSetAccessProvider = exerciseSetAccessProvider;
            _counterService = counterService;
            _userProvider = userProvider;
        }



        #region ExerciseSet
        public void SaveNewExerciseSet(ExerciseSet set)
        {
            set.Id = _counterService.GetNextId(CollectionName.ExerciseSets);

            if (string.IsNullOrWhiteSpace(set.Title))
            {
                MessageBox.Show("Vui lòng nhập tên bộ!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            set.OwnerId = AuthenticationCache.CurrentUser?.Id ?? 0;
            set.Password = string.IsNullOrEmpty(set.Password) ? null : SecureConfigHelper.Encrypt(set.Password);
            set.Code = CodeGenerator.GenerateCode(8); // Tạo mã code 8 ký tự

            var newAccess = new ExerciseSetAccess
            {
                ExerciseSetId = set.Id,
                UserId = set.OwnerId,
                Level = AccessLevel.Owner
            };

            // Lưu bộ bài tập
            try
            {
                _assignmentProvider.CreateNewExerciseSet(set);
                _exerciseSetAccessProvider.CreateNewAccess(newAccess);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu bộ bài tập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public List<ExerciseSet> GetAllExerciseSetsByUserId(long userId)
        {
            var accessList = AuthorizationCache.AllExerciseSetAccess.Where(esa => esa.UserId == userId);
            var results = new List<ExerciseSet>();

            foreach(var access in accessList)
            {
                var sets = AssignmentCache.AllExerciseSets.Where(es => es.Id == access.ExerciseSetId);
                foreach(var set in sets)
                {
                    var owner = _userProvider.GetUserById(set.OwnerId);
                    set.OwnerInfo = new UserDTO
                    {
                        Id = owner.Id,
                        Username = owner.Username,
                        Email = owner.Email,
                        PhoneNumber = owner.PhoneNumber,
                        DateOfBirth = owner.DateOfBirth,
                    };
                    results.Add(set);
                }
            }
            return results;
        }

        public void DeleteExerciseSet(long setId)
        {

        }
        #endregion

        #region Exercise

        #endregion
    }
}
