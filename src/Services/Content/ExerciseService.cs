using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Interfaces.Providers.Users;
using LaboratoryApp.Domain.Interfaces.Services.Common;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches.Assignment;
using LaboratoryApp.src.Core.Interfaces;
using System.Windows;

namespace LaboratoryApp.src.Services.Content
{
    public class ExerciseService : IExerciseService
    {
        private readonly IDialogService _dialogService;

        private readonly IAssignmentCache _assignmentCache;
        private readonly ICounterService _counterService;
        private readonly IExerciseProvider _exerciseProvider;
        private readonly IExerciseSetProvider _exerciseSetProvider;
        private readonly IUserProvider _userProvider;

        public ExerciseService(IDialogService dialogService,
                               IAssignmentCache assignmentCache,
                               ICounterService counterService,
                               IExerciseProvider exerciseProvider,
                               IExerciseSetProvider exerciseSetProvider,
                               IUserProvider userProvider)
        {
            _dialogService = dialogService;
            _assignmentCache = assignmentCache;
            _counterService = counterService;
            _exerciseProvider = exerciseProvider;
            _exerciseSetProvider = exerciseSetProvider;
            _userProvider = userProvider;
        }

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
                _dialogService.ShowMessage("Vui lòng nhập tên bài tập!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                set.UpdatedAt = DateTime.UtcNow;
                _assignmentCache.AllExercises.Add(exercise);
                _exerciseProvider.CreateNewExerciseAsync(exercise);
                _exerciseSetProvider.UpdateExerciseSetAsync(set);
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Lỗi khi lưu bài tập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
            var results = new List<Exercise>();

            if (set == null)
                return results;

            return results;
        }
    }
}
