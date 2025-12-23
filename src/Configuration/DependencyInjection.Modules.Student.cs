using LaboratoryApp.Domain.Interfaces.Services.English;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.English.FlashcardFunction;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Modules.Student.English.FlashcardFunction.ViewModels;
using LaboratoryApp.src.Modules.Student.English.FlashcardFunction.Views;
using LaboratoryApp.src.Modules.Student.English.LectureFunction.ViewModels;
using LaboratoryApp.src.Modules.Student.English.LectureFunction.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LaboratoryApp.src.Configuration
{
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Khởi tạo và đăng ký các module liên quan đến học sinh
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddStudentModule(this IServiceCollection services)
        {
            // Register student-related modules here
            #region ViewModels
            // Chemistry

            // English
            // Flashcard Function
            services.AddTransient<FlashcardManagerViewModel>();
            services.AddTransient<FlashcardViewModel>();
            services.AddTransient<FlashcardSetViewModel>();
            services.AddTransient<FlashcardStudyViewModel>();

            // Lecture Function
            services.AddTransient<LectureContentViewModel>();
            services.AddTransient<LectureMainPageViewModel>();

            // Maths

            // Physics

            #endregion

            #region Views
            // Chemistry

            // English
            // Flashcard Function
            services.AddTransient<FlashcardManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<FlashcardManagerViewModel>();
                return new FlashcardManagerPage { DataContext = vm };
            });
            services.AddTransient<FlashcardStudyWindow>();
            services.AddTransient<FlashcardWindow>();
            services.AddTransient<UpdateFlashcardSetWindow>();

            // Lecture Function
            services.AddTransient<LectureContentPage>(sp =>
            {
                var vm = sp.GetRequiredService<LectureContentViewModel>();
                return new LectureContentPage { DataContext = vm };
            });
            services.AddTransient<LectureMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<LectureMainPageViewModel>();
                return new LectureMainPage { DataContext = vm };
            });

            // Maths

            // Physics

            #endregion

            #region Factories
            // Chemistry

            // English
            // Flashcard Function
            services.AddTransient<Func<IFlashcardService, FlashcardSet, FlashcardSetViewModel>>(sp => (service, set) => new FlashcardSetViewModel(service, set));
            services.AddTransient<Func<IServiceProvider, IFlashcardService, long, Flashcard, FlashcardViewModel>>(sp => (sp, service, setId, card) => new FlashcardViewModel(sp, service, setId, card));
            services.AddTransient<Func<IDialogService, ISpeechService, FlashcardSet, IFlashcardService, FlashcardStudyViewModel>>(sp => (dialogService, speechService, set, flashcardService) => new FlashcardStudyViewModel(dialogService, speechService, set, flashcardService));

            // Lecture Function

            // Maths

            // Physics

            #endregion

            return services;
        }
    }
}
