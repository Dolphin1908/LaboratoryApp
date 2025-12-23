using LaboratoryApp.Domain.DTOs.Content.Assessment;
using LaboratoryApp.Domain.Enums.Authorization;
using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Interfaces.Providers.Users;
using LaboratoryApp.Domain.Interfaces.Services.Common;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Interfaces.Services.Operations;
using LaboratoryApp.Domain.Models.Authorization;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Core.Interfaces.Services;
using MongoDB.Driver;

namespace LaboratoryApp.src.Services.Content
{
    public class ExerciseService : IExerciseService
    {
        private readonly IAuthorizationCache _authorizationCache;

        private readonly IExerciseProvider _exerciseProvider;
        private readonly IUserProvider _userProvider;

        private readonly IAnswerOptionService _answerOptionService;
        private readonly ICounterService _counterService;
        private readonly IDialogService _dialogService;
        private readonly IExerciseAccessService _exerciseAccessService;
        private readonly IQuestionBlockService _questionBlockService;
        private readonly IQuestionService _questionService;

        public ExerciseService(IAuthorizationCache authorizationCache,
                               IExerciseProvider exerciseProvider,
                               IUserProvider userProvider,
                               IAnswerOptionService answerOptionService,
                               ICounterService counterService,
                               IDialogService dialogService,
                               IExerciseAccessService exerciseAccessService,
                               IQuestionBlockService questionBlockService,
                               IQuestionService questionService)
        {
            _authorizationCache = authorizationCache;

            _exerciseProvider = exerciseProvider;
            _userProvider = userProvider;

            _answerOptionService = answerOptionService;
            _counterService = counterService;
            _dialogService = dialogService;
            _exerciseAccessService = exerciseAccessService;
            _questionBlockService = questionBlockService;
            _questionService = questionService;
        }

        public async Task<List<Exercise>> GetAllExercisesByUserId(long userId)
        {
            var exerciseIds = _authorizationCache.AllExerciseAccess.Where(ea => ea.UserId == userId);
            var exercises = await _exerciseProvider.GetExercisesByIdsAsync(exerciseIds.Select(ea => ea.ExerciseId).ToList());
            return exercises;
        }

        public async Task<List<ExerciseSummaryDTO>> GetAllExerciseDashboardAsync(long teacherId)
        {
            var dashboardItems = new List<ExerciseSummaryDTO>();

            var exercises = (await _exerciseProvider.GetAllExercisesAsync()).Where(e => e.AuthorId == teacherId && e.Status != ExerciseStatus.Archived).ToList();

            foreach (var exercise in exercises)
            {
                if (exercise.Status == ExerciseStatus.Draft)
                {
                    dashboardItems.Add(new ExerciseSummaryDTO
                    {
                        ExerciseId = exercise.Id,
                        Title = exercise.Title,
                        Status = ExerciseStatus.Draft,
                        ClassName = "Chưa giao"
                    });
                    continue;
                }

            }

            return dashboardItems;
        }

        public async Task<Exercise?> GetExerciseByIdAsync(long exerciseId)
        {
            var exercise = await _exerciseProvider.GetExerciseByIdAsync(exerciseId);
            if (exercise == null) throw new Exception("Exercise not found");

            exercise.Questions = await _questionService.GetQuestionsByExerciseIdAsync(exerciseId);
            exercise.QuestionBlocks = await _questionBlockService.GetQuestionBlocksByExerciseIdAsync(exerciseId);

            return exercise;
        }

        public async Task SaveNewExerciseAsync(Exercise newExercise)
        {
            newExercise.Id = _counterService.GetNextId(CollectionName.Exercises);
            await _exerciseProvider.CreateNewExerciseAsync(newExercise);
            if (newExercise.Questions.Count > 0)
            {
                foreach (var question in newExercise.Questions)
                {
                    question.RootExerciseId = newExercise.Id;
                    await _questionService.SaveNewQuestionAsync(question);
                }
            }
            if (newExercise.QuestionBlocks.Count > 0)
            {
                foreach (var questionBlock in newExercise.QuestionBlocks)
                {
                    questionBlock.RootExerciseId = newExercise.Id;
                    await _questionBlockService.SaveNewQuestionBlockAsync(questionBlock);
                }
            }

            var newExerciseAccess = new ExerciseAccess
            {
                Id = _counterService.GetNextId(CollectionName.ExerciseAccess),
                ExerciseId = newExercise.Id,
                UserId = newExercise.AuthorId,
                Level = AccessLevel.Owner,
                GrantedAt = DateTime.UtcNow
            };

            await _exerciseAccessService.SaveNewExerciseAccessAsync(newExerciseAccess);
        }
    }
}
