using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Caches.Assignment;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Coursework.QuestionFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Coursework.QuestionFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Dashboard.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Dashboard.Views;
using LaboratoryApp.src.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LaboratoryApp.src.Configuration
{
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Khởi tạo và đăng ký các module liên quan đến giáo viên
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddTeacherModule(this IServiceCollection services)
        {
            // Register teacher-related modules here
            #region ViewModels
            // Chemistry
            // Compound Function
            services.AddTransient<CompoundComponentViewModel>();
            services.AddTransient<CompoundNoteViewModel>();
            services.AddTransient<CompoundViewModel>();


            // Reaction Function
            services.AddTransient<ReactionComponentViewModel>();
            services.AddTransient<ReactionNoteViewModel>();
            services.AddTransient<ReactionViewModel>();

            // Coursework
            // Exercise Function

            // Exercise Set Function
            services.AddTransient<ExerciseSetManagerViewModel>();
            services.AddTransient<ExerciseSetViewModel>();
            services.AddTransient<InsertExerciseSetViewModel>();

            // Question Function
            services.AddTransient<OptionViewModel>();
            services.AddTransient<QuestionViewModel>();

            // Dashboard
            services.AddTransient<TeacherMainPageViewModel>();

            // English

            #endregion

            #region Views
            // Chemistry
            // Compound Function
            services.AddTransient<AddCompoundWindow>(sp =>
            {
                var vm = sp.GetRequiredService<CompoundViewModel>();
                return new AddCompoundWindow { DataContext = vm };
            });

            // Reaction Function
            services.AddTransient<AddReactionWindow>(sp =>
            {
                var vm = sp.GetRequiredService<ReactionViewModel>();
                return new AddReactionWindow { DataContext = vm };
            });

            // Coursework
            // Exercise Function
            services.AddTransient<AddExerciseWindow>();
            services.AddTransient<ExerciseManagerPage>();

            // Exercise Set Function
            services.AddTransient<AddExerciseSetWindow>(sp =>
            {
                var vm = sp.GetRequiredService<ExerciseSetViewModel>();
                return new AddExerciseSetWindow { DataContext = vm };
            });
            services.AddTransient<ExerciseSetManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<ExerciseSetManagerViewModel>();
                return new ExerciseSetManagerPage { DataContext = vm };
            });
            services.AddTransient<InsertExerciseSetWindow>(sp =>
            {
                var vm = sp.GetRequiredService<InsertExerciseSetViewModel>();
                return new InsertExerciseSetWindow { DataContext = vm };
            });

            // Question Function
            services.AddTransient<AddQuestionWindow>(sp =>
            {
                var vm = sp.GetRequiredService<QuestionViewModel>();
                return new AddQuestionWindow { DataContext = vm };
            });
            services.AddTransient<QuestionManagerPage>();

            // Dashboard
            services.AddTransient<TeacherMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<TeacherMainPageViewModel>();
                return new TeacherMainPage { DataContext = vm };
            });

            // English

            #endregion

            #region Factories
            // Chemistry
            // Compound Function

            // Reaction Function

            // Coursework
            // Exercise Function
            services.AddTransient<Func<INavigationService, IServiceProvider, IAssignmentCache, IAuthorizationCache, IExerciseService, ExerciseSet, ExerciseManagerViewModel>>(sp =>
            (navigationService, serviceProvider, assignmentCache, authorizationCache, exerciseService, selectedSet) =>
            {
                var addExerciseVmFactory = sp.GetRequiredService<Func<IExerciseService, ExerciseSet, ExerciseViewModel>>();
                var exerciseDetailVmFactory = sp.GetRequiredService<Func<IServiceProvider, IAuthorizationCache, INavigationService, long, Exercise, QuestionManagerViewModel>>();
                return new ExerciseManagerViewModel(navigationService, serviceProvider, assignmentCache, authorizationCache, exerciseService, selectedSet, addExerciseVmFactory, exerciseDetailVmFactory);
            });
            services.AddTransient<Func<IExerciseService, ExerciseSet, ExerciseViewModel>>(sp =>
            (service, currSet) =>
            {
                return new ExerciseViewModel(service, currSet);
            });

            // Exercise Set Function

            // Question Function
            services.AddTransient<Func<IServiceProvider, IAuthorizationCache, INavigationService, long, Exercise, QuestionManagerViewModel>>(sp =>
            (serviceProvider, authorizationCache, navigationService, setId, selectedExercise) =>
            {
                return new QuestionManagerViewModel(serviceProvider, authorizationCache, navigationService, setId, selectedExercise);
            });

            // Dashboard

            // English

            #endregion

            return services;
        }
    }
}
