using System.Configuration;
using System.Data;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.FlashcardFunction;

using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Data.Providers.Interfaces;
using LaboratoryApp.src.Data.Providers.Authentication;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;

using LaboratoryApp.src.Modules.Authentication.Views;
using LaboratoryApp.src.Modules.Authentication.ViewModels;

using LaboratoryApp.src.Modules.Chemistry.Common.Views;
using LaboratoryApp.src.Modules.Chemistry.Common.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels;

using LaboratoryApp.src.Modules.English.Common.Views;
using LaboratoryApp.src.Modules.English.Common.ViewModels;
using LaboratoryApp.src.Modules.English.DiaryFunction.Views;
using LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;
using LaboratoryApp.src.Modules.English.DictionaryFunction.ViewModels;
using LaboratoryApp.src.Modules.English.FlashcardFunction.Views;
using LaboratoryApp.src.Modules.English.FlashcardFunction.ViewModels;
using LaboratoryApp.src.Modules.English.LectureFunction.Views;
using LaboratoryApp.src.Modules.English.LectureFunction.ViewModels;
using LaboratoryApp.src.Modules.English.PracticeFunction.Views;
using LaboratoryApp.src.Modules.English.PracticeFunction.ViewModels;

using LaboratoryApp.src.Modules.Maths.Common.Views;
using LaboratoryApp.src.Modules.Maths.Common.ViewModels;

using LaboratoryApp.src.Modules.Physics.Common.Views;
using LaboratoryApp.src.Modules.Physics.Common.ViewModels;

using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels;

using LaboratoryApp.src.Modules.Toolkits.Common.Views;
using LaboratoryApp.src.Modules.Toolkits.Common.ViewModels;

using LaboratoryApp.src.UI.Views;
using LaboratoryApp.src.UI.ViewModels;

using LaboratoryApp.src.Services.AI;
using LaboratoryApp.src.Services.Authentication;
using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Services.English;
using LaboratoryApp.src.Services.English.FlashcardFunction;

using LaboratoryApp.src.Shared;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels;

namespace LaboratoryApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Khởi tạo MainWindow
            var mainWindow = new MainWindow();
            var frame = mainWindow.mainFrame;

            // Tạo instance của NavigateService
            var navigateService = new NavigateService(frame);

            // Khởi tạo dịch vụ ServiceCollection
            var service = new ServiceCollection();

            // Đăng ký các dịch vụ cần thiết
            #region Services
            service.AddSingleton<INavigationService>(navigateService);
            service.AddSingleton<IAIService, AIService>();
            service.AddTransient<IAuthenticationService, AuthenticationService>();
            service.AddSingleton<IChemistryService, ChemistryService>();
            service.AddSingleton<IFlashcardService, FlashcardService>();
            service.AddSingleton<IEnglishService, EnglishService>();
            #endregion

            // Đăng ký các providers
            #region Providers
            service.AddSingleton<IMongoDBProvider>(sp =>
            {
                var conn = SecureConfigHelper.Decrypt(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
                return new MongoDBProvider(conn, "authentication");
            });
            service.AddSingleton<IUserProvider>(sp => new UserProvider(sp.GetRequiredService<IMongoDBProvider>()));
            service.AddSingleton<IRoleProvider>(sp => new RoleProvider(sp.GetRequiredService<IMongoDBProvider>()));
            service.AddSingleton<IUserRoleProvider>(sp => new UserRoleProvider(sp.GetRequiredService<IMongoDBProvider>()));
            service.AddSingleton<IRefreshTokenProvider>(sp => new RefreshTokenProvider(sp.GetRequiredService<IMongoDBProvider>()));
            #endregion

            // Đăng ký các caches
            #region Caches
            service.AddSingleton<ChemistryDataCache>();
            service.AddSingleton<EnglishDataCache>();
            #endregion

            // Đăng ký các ViewModels theo modules
            #region UI
            service.AddTransient<MainWindowViewModel>();
            service.AddTransient<DashboardViewModel>();
            service.AddTransient<ControlBarViewModel>();
            #endregion

            #region Authentication
            service.AddTransient<AuthenticationViewModel>();
            #endregion

            #region Chemistry
            service.AddTransient<ChemistryMainPageViewModel>();

            service.AddTransient<CompoundManagerViewModel>();

            service.AddTransient<ElementInfoViewModel>();
            service.AddTransient<PeriodicTableViewModel>();

            service.AddTransient<ReactionManagerViewModel>();
            #endregion

            #region English
            service.AddTransient<EnglishMainPageViewModel>();

            service.AddTransient<DiaryViewModel>();
            service.AddTransient<Func<DiaryContent, DiaryViewModel>>(sp => (diary) => ActivatorUtilities.CreateInstance<DiaryViewModel>(sp, diary));
            service.AddTransient<DiaryManagerViewModel>();
            service.AddTransient<Func<IUserProvider, IServiceProvider, IEnglishService, EnglishDataCache, DiaryContent, DiaryDetailViewModel>>(sp => 
            (userService, service, engService, engDataCache, diary) =>
            {
                var diaryVmFactory = sp.GetRequiredService<Func<DiaryContent, DiaryViewModel>>();
                return new DiaryDetailViewModel(userService, service, engService, engDataCache, diary, diaryVmFactory);
            });

            service.AddTransient<DictionaryViewModel>();

            service.AddTransient<FlashcardManagerViewModel>();
            service.AddTransient<FlashcardViewModel>();
            service.AddTransient<FlashcardStudyViewModel>();

            // Đăng ký các hàm tạo cho FlashcardViewModel và FlashcardStudyViewModel
            service.AddTransient<Func<FlashcardSet, Action<FlashcardSet>, FlashcardViewModel>>(sp => (set, callback) => new FlashcardViewModel(set, callback));
            service.AddTransient<Func<Flashcard, Action<Flashcard>, Func<DictionaryWindow>, FlashcardViewModel>>(sp =>
            (flashcard, callback, dictFactory) =>
            {
                var serviceProvider = sp;
                return new FlashcardViewModel(sp, flashcard, callback, dictFactory);
            });
            service.AddTransient<Func<FlashcardSet, IFlashcardService, FlashcardStudyViewModel>>(sp => (set, service) => new FlashcardStudyViewModel(set, service));

            service.AddTransient<LectureMainPageViewModel>();
            service.AddTransient<LectureContentViewModel>();

            service.AddTransient<PracticeManagerViewModel>();
            #endregion

            #region Maths
            service.AddTransient<MathsMainPageViewModel>();
            #endregion

            #region Physics
            service.AddTransient<PhysicsMainPageViewModel>();
            #endregion

            #region Teacher
            service.AddTransient<CompoundNoteViewModel>();
            service.AddTransient<CompoundViewModel>();

            service.AddTransient<ReactionComponentViewModel>();
            service.AddTransient<ReactionNoteViewModel>();
            service.AddTransient<ReactionViewModel>();
            #endregion

            #region Toolkits
            service.AddTransient<ToolkitsViewModel>();
            #endregion


            // Đăng ký các Views và ViewModels theo modules
            #region UI
            service.AddTransient<Dashboard>(sp =>
            {
                var vm = sp.GetRequiredService<DashboardViewModel>();
                return new Dashboard { DataContext = vm };
            });

            service.AddTransient<ToolkitsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<ToolkitsViewModel>();
                return new ToolkitsMainPage { DataContext = vm };
            });
            #endregion

            #region Authentication
            service.AddTransient<AuthenticationWindow>(sp =>
            {
                var vm = sp.GetRequiredService<AuthenticationViewModel>();
                return new AuthenticationWindow { DataContext = vm };
            });
            #endregion

            #region Chemistry
            service.AddTransient<ChemistryMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<ChemistryMainPageViewModel>();
                return new ChemistryMainPage { DataContext = vm };
            });

            service.AddTransient<PeriodicTableWindow>(sp =>
            {
                var vm = sp.GetRequiredService<PeriodicTableViewModel>();
                return new PeriodicTableWindow { DataContext = vm };
            });

            service.AddTransient<CompoundManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<CompoundManagerViewModel>();
                return new CompoundManagerPage { DataContext = vm };
            });

            service.AddTransient<ReactionManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<ReactionManagerViewModel>();
                return new ReactionManagerPage { DataContext = vm };
            });
            #endregion

            #region English
            service.AddTransient<EnglishMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<EnglishMainPageViewModel>();
                return new EnglishMainPage { DataContext = vm };
            });

            service.AddTransient<DiaryDetailWindow>();
            service.AddTransient<DiaryManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<DiaryManagerViewModel>();
                return new DiaryManagerPage { DataContext = vm };
            });
            service.AddTransient<DiaryWindow>(sp =>
            {
                var vm = sp.GetRequiredService<DiaryViewModel>();
                return new DiaryWindow { DataContext = vm };
            });

            service.AddTransient<DictionaryWindow>(sp =>
            {
                var vm = sp.GetRequiredService<DictionaryViewModel>();
                return new DictionaryWindow { DataContext = vm };
            });
            service.AddTransient<Func<DictionaryWindow>>(sp =>
            {
                return () => sp.GetRequiredService<DictionaryWindow>();
            });

            service.AddTransient<FlashcardManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<FlashcardManagerViewModel>();
                return new FlashcardManagerPage { DataContext = vm };
            });
            service.AddTransient<FlashcardStudyWindow>();
            service.AddTransient<AddFlashcardWindow>();
            service.AddTransient<UpdateFlashcardWindow>();
            service.AddTransient<UpdateFlashcardSetWindow>();

            service.AddTransient<LectureMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<LectureMainPageViewModel>();
                return new LectureMainPage { DataContext = vm };
            });

            service.AddTransient<PracticeManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<PracticeManagerViewModel>();
                return new PracticeManagerPage { DataContext = vm };
            });
            #endregion

            #region Maths
            service.AddTransient<MathsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<MathsMainPageViewModel>();
                return new MathsMainPage { DataContext = vm };
            });
            #endregion

            #region Physics
            service.AddTransient<PhysicsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<PhysicsMainPageViewModel>();
                return new PhysicsMainPage { DataContext = vm };
            });
            #endregion

            #region Teacher
            service.AddTransient<AddCompoundWindow>(sp =>
            {
                var vm = sp.GetRequiredService<CompoundViewModel>();
                return new AddCompoundWindow { DataContext = vm };
            }); 
            
            service.AddTransient<AddReactionWindow>(sp =>
            {
                var vm = sp.GetRequiredService<ReactionViewModel>();
                return new AddReactionWindow { DataContext = vm };
            });
            #endregion

            #region Toolkits
            service.AddTransient<ToolkitsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<ToolkitsViewModel>();
                return new ToolkitsMainPage { DataContext = vm };
            });
            #endregion


            // Xây dựng ServiceProvider
            _serviceProvider = service.BuildServiceProvider();

            // Lấy MainWindowViewModel từ ServiceProvider
            var mainVM = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindow.DataContext = mainVM;

            // Hiển thị MainWindow
            mainWindow.Show();
        }
    }

}
