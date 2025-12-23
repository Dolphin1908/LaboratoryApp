using LaboratoryApp.Domain.Interfaces.Providers.Users;
using LaboratoryApp.Domain.Interfaces.Services.English;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.Chemistry.ReactionFunction;
using LaboratoryApp.Domain.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Modules.Chemistry.Dashboard.Views;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;
using LaboratoryApp.src.Modules.Toolkits.CalculatorFunction.Views;
using LaboratoryApp.src.Modules.Tools.Chemistry.Dashboard.ViewModels;
using LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.ReactionFunction.ViewModels;
using LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.ReactionFunction.Views;
using LaboratoryApp.src.Modules.Tools.Chemistry.PeriodicFunction.ViewModels;
using LaboratoryApp.src.Modules.Tools.English.Dashboard.ViewModels;
using LaboratoryApp.src.Modules.Tools.English.Dashboard.Views;
using LaboratoryApp.src.Modules.Tools.English.DiaryFunction.ViewModels;
using LaboratoryApp.src.Modules.Tools.English.DiaryFunction.Views;
using LaboratoryApp.src.Modules.Tools.English.DictionaryFunction.ViewModels;
using LaboratoryApp.src.Modules.Tools.Maths.Dashboard.ViewModels;
using LaboratoryApp.src.Modules.Tools.Maths.Dashboard.Views;
using LaboratoryApp.src.Modules.Tools.Physics.Dashboard.ViewModels;
using LaboratoryApp.src.Modules.Tools.Physics.Dashboard.Views;
using LaboratoryApp.src.Modules.Tools.Toolkits.Dashboard.ViewModels;
using LaboratoryApp.src.Modules.Tools.Toolkits.Dashboard.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LaboratoryApp.src.Configuration
{
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Khởi tạo và đăng ký các module liên quan đến công cụ
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddToolsModule(this IServiceCollection services)
        {
            // Register tool-related modules here
            #region ViewModels
            // Chemistry
            // Dashboard Function
            services.AddTransient<ChemistryMainPageViewModel>();

            // Dictionary Function
            // Compound Function
            services.AddTransient<CompoundManagerViewModel>();

            // Reaction Function
            services.AddTransient<ReactionManagerViewModel>();
            services.AddTransient<Func<IChemistryDataCache, Reaction, ReactionSelectionResultViewModel>>(sp =>
            (cache, selectedReaction) => ActivatorUtilities.CreateInstance<ReactionSelectionResultViewModel>(sp, cache, selectedReaction));

            // Periodic Function
            services.AddTransient<ElementInfoViewModel>();
            services.AddTransient<PeriodicTableViewModel>();

            // English
            // Dashboard Function
            services.AddTransient<EnglishMainPageViewModel>();

            // Diary Function
            services.AddTransient<DiaryViewModel>();
            services.AddTransient<DiaryManagerViewModel>();

            // Dictionary Function
            services.AddTransient<DictionaryViewModel>();

            // Maths
            //Dashboard Function
            services.AddTransient<MathsMainPageViewModel>();

            // Physics
            // Dashboard Function
            services.AddTransient<PhysicsMainPageViewModel>();

            // Toolkits
            // Calculator Function

            // Dashboard Function
            services.AddTransient<ToolkitsViewModel>();

            #endregion

            #region Views
            // Chemistry
            // Dashboard Function
            services.AddTransient<ChemistryMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<ChemistryMainPageViewModel>();
                return new ChemistryMainPage { DataContext = vm };
            });

            // Dictionary Function
            // Compound Function
            services.AddTransient<CompoundManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<CompoundManagerViewModel>();
                return new CompoundManagerPage { DataContext = vm };
            });

            // Reaction Function
            services.AddTransient<ReactionManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<ReactionManagerViewModel>();
                return new ReactionManagerPage { DataContext = vm };
            });
            services.AddTransient<ReactionSelectionResultWindow>();

            // Periodic Function
            services.AddTransient<PeriodicTableWindow>(sp =>
            {
                var vm = sp.GetRequiredService<PeriodicTableViewModel>();
                return new PeriodicTableWindow { DataContext = vm };
            });

            // English
            // Dashboard Function
            services.AddTransient<EnglishMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<EnglishMainPageViewModel>();
                return new EnglishMainPage { DataContext = vm };
            });
            // Diary Function
            services.AddTransient<DiaryDetailWindow>();
            services.AddTransient<DiaryManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<DiaryManagerViewModel>();
                return new DiaryManagerPage { DataContext = vm };
            });
            services.AddTransient<DiaryWindow>(sp =>
            {
                var vm = sp.GetRequiredService<DiaryViewModel>();
                return new DiaryWindow { DataContext = vm };
            });


            // Dictionary Function
            services.AddTransient<DictionaryWindow>(sp =>
            {
                var vm = sp.GetRequiredService<DictionaryViewModel>();
                return new DictionaryWindow { DataContext = vm };
            });

            // Maths
            //Dashboard Function
            services.AddTransient<MathsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<MathsMainPageViewModel>();
                return new MathsMainPage { DataContext = vm };
            });

            // Physics
            // Dashboard Function
            services.AddTransient<PhysicsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<PhysicsMainPageViewModel>();
                return new PhysicsMainPage { DataContext = vm };
            });

            // Toolkits
            // Calculator Function
            services.AddTransient<CalculatorWindow>();

            // Dashboard Function
            services.AddTransient<ToolkitsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<ToolkitsViewModel>();
                return new ToolkitsMainPage { DataContext = vm };
            });

            #endregion

            #region Factories
            // Chemistry
            // Dashboard Function

            // Dictionary Function
            // Compound Function

            // Reaction Function

            // Periodic Function

            // English
            // Dashboard Function

            // Diary Function
            services.AddTransient<Func<IServiceProvider, IAIService, IDialogService, IDiaryService, IFormatConversionService, IUserProvider, DiaryContent, DiaryDetailViewModel>>(sp =>
            (service, aiService, dialogService, diaryService, formatService, userService, diary) =>
            {
                var diaryVmFactory = sp.GetRequiredService<Func<IServiceProvider, IAIService, IDialogService, IDiaryService, IFormatConversionService, DiaryContent, DiaryViewModel>>();
                return new DiaryDetailViewModel(service, aiService, dialogService, diaryService, formatService, userService, diary, diaryVmFactory);
            });
            services.AddTransient<Func<IServiceProvider, IAIService, IDialogService, IDiaryService, IFormatConversionService, DiaryContent, DiaryViewModel>>(sp =>
            (service, aiService, dialogService, diaryService, formatService, diary) =>
            {
                return new DiaryViewModel(service, aiService, dialogService, diaryService, formatService, diary);
            });

            // Dictionary Function
            services.AddTransient<Func<DictionaryWindow>>(sp =>
            {
                return () => sp.GetRequiredService<DictionaryWindow>();
            });

            // Maths
            //Dashboard Function

            // Physics
            // Dashboard Function

            // Toolkits
            // Calculator Function

            // Dashboard Function

            #endregion

            return services;
        }
    }
}