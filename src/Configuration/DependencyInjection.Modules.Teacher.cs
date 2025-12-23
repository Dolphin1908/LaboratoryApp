using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.DashboardFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.DashboardFunction.Views;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Modules.Teacher.DashboardFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.DashboardFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.Views;
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

            // Assessment
            // Dashboard
            services.AddTransient<AssessmentMainPageViewModel>();

            // Exercise
            services.AddTransient<ContentSelectionViewModel>();
            services.AddTransient<CreateNewExerciseViewModel>();
            services.AddTransient<ExerciseDetailViewModel>();
            services.AddTransient<InformationViewModel>();
            services.AddTransient<ContentViewModel>();
            services.AddTransient<SettingViewModel>();

            // Dashboard
            services.AddTransient<TeacherMainPageViewModel>();
            services.AddTransient<TeacherToolsMainPageViewModel>();

            // English

            // Shared
            services.AddTransient<QuestionEditorViewModel>();
            services.AddTransient<QuestionBlockEditorViewModel>();

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

            // Assessment
            // Dashboard
            services.AddTransient<AssessmentMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<AssessmentMainPageViewModel>();
                return new AssessmentMainPage { DataContext = vm };
            });

            // Exercise
            services.AddTransient<ContentSelectionWindow>(sp =>
            {
                var vm = sp.GetRequiredService<ContentSelectionViewModel>();
                return new ContentSelectionWindow { DataContext = vm };
            });
            services.AddTransient<CreateNewExerciseWindow>(sp =>
            {
                var vm = sp.GetRequiredService<CreateNewExerciseViewModel>();
                return new CreateNewExerciseWindow { DataContext = vm };
            });
            services.AddTransient<ExerciseDetailWindow>(sp =>
            {
                var vm = sp.GetRequiredService<ExerciseDetailViewModel>();
                return new ExerciseDetailWindow { DataContext = vm };
            });

            // Dashboard
            services.AddTransient<TeacherMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<TeacherMainPageViewModel>();
                return new TeacherMainPage { DataContext = vm };
            });
            services.AddTransient<TeacherToolsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<TeacherToolsMainPageViewModel>();
                return new TeacherToolsMainPage { DataContext = vm };
            });

            // English

            // Shared
            services.AddTransient<QuestionEditorWindow>();
            services.AddTransient<QuestionBlockEditorWindow>();

            #endregion

            #region Factories
            // Chemistry

            // Assessment

            // Dashboard

            // English

            // Shared
            services.AddTransient<Func<IAssetService, IDialogService, IFormatConversionService, Question, QuestionEditorViewModel>>(sp => (assetService, dialogService, formatService, model) => new QuestionEditorViewModel(assetService, dialogService, formatService, model));
            services.AddTransient<Func<IServiceProvider, IAssetService, IDialogService, IFormatConversionService, QuestionBlock, Func<IAssetService, IDialogService, IFormatConversionService, Question, QuestionEditorViewModel>, QuestionBlockEditorViewModel>>(sp => (service, assetService, dialogService, formatService, model, factory) => new QuestionBlockEditorViewModel(service, assetService, dialogService, formatService, model, factory));

            #endregion

            return services;
        }
    }
}
