using LaboratoryApp.Domain.Models.Content;

using LaboratoryApp.Domain.Models.Chemistry.ReactionFunction;

using LaboratoryApp.Domain.Models.English.DiaryFunction;
using LaboratoryApp.Domain.Models.English.FlashcardFunction;

using LaboratoryApp.src.Constants;

using LaboratoryApp.src.Core.Caches.Assignment;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.Caches.English;

using LaboratoryApp.src.Core.Helpers;

using LaboratoryApp.src.Data.Providers.Assignment.ExerciseFunction;
using LaboratoryApp.src.Data.Providers.Assignment.ExerciseSetFunction;
using LaboratoryApp.src.Data.Providers.Assignment.QuestionFunction;

using LaboratoryApp.src.Data.Providers.Authentication;
using LaboratoryApp.src.Data.Providers.Authentication.Interface;

using LaboratoryApp.src.Data.Providers.Authorization;

using LaboratoryApp.src.Data.Providers.Chemistry.CompoundFunction;
using LaboratoryApp.src.Data.Providers.Chemistry.PeriodicFunction;
using LaboratoryApp.src.Data.Providers.Chemistry.ReactionFunction;

using LaboratoryApp.src.Data.Providers.Common;

using LaboratoryApp.src.Data.Providers.English.DiaryFunction;
using LaboratoryApp.src.Data.Providers.English.DictionaryFunction;
using LaboratoryApp.src.Data.Providers.English.FlashcardFunction;

using LaboratoryApp.src.Modules.Assignment.ExerciseFunction.ViewModels;
using LaboratoryApp.src.Modules.Assignment.ExerciseFunction.Views;

using LaboratoryApp.src.Modules.Assignment.ExerciseSetFunction.ViewModels;
using LaboratoryApp.src.Modules.Assignment.ExerciseSetFunction.Views;

using LaboratoryApp.src.Modules.Assignment.QuestionFunction.ViewModels;
using LaboratoryApp.src.Modules.Assignment.QuestionFunction.Views;

using LaboratoryApp.src.Modules.Authentication.ViewModels;
using LaboratoryApp.src.Modules.Authentication.Views;

using LaboratoryApp.src.Modules.Chemistry.Common.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.Common.Views;

using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.Views;

using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;

using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.Views;

using LaboratoryApp.src.Modules.English.Common.ViewModels;
using LaboratoryApp.src.Modules.English.Common.Views;

using LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels;
using LaboratoryApp.src.Modules.English.DiaryFunction.Views;

using LaboratoryApp.src.Modules.English.DictionaryFunction.ViewModels;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;

using LaboratoryApp.src.Modules.English.FlashcardFunction.ViewModels;
using LaboratoryApp.src.Modules.English.FlashcardFunction.Views;

using LaboratoryApp.src.Modules.English.LectureFunction.ViewModels;
using LaboratoryApp.src.Modules.English.LectureFunction.Views;

using LaboratoryApp.src.Modules.Maths.Common.ViewModels;
using LaboratoryApp.src.Modules.Maths.Common.Views;

using LaboratoryApp.src.Modules.Physics.Common.ViewModels;
using LaboratoryApp.src.Modules.Physics.Common.Views;

using LaboratoryApp.src.Modules.Teacher.Assignment.Common.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Assignment.Common.Views;

using LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.Views;

using LaboratoryApp.src.Modules.Teacher.Assignment.QuestionFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Assignment.QuestionFunction.Views;

using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.Views;

using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.Views;

using LaboratoryApp.src.Modules.Teacher.Common.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Common.Views;

using LaboratoryApp.src.Modules.Toolkits.Common.ViewModels;
using LaboratoryApp.src.Modules.Toolkits.Common.Views;

using LaboratoryApp.src.Services.Assignment.ExerciseFunction;
using LaboratoryApp.src.Services.Assignment.ExerciseSetFunction;
using LaboratoryApp.src.Services.Assignment.QuestionFunction;

using LaboratoryApp.src.Services.Authentication.Common;
using LaboratoryApp.src.Services.Authentication.OrganizationFunction;
using LaboratoryApp.src.Services.Authentication.UserOrganizationFunction;
using LaboratoryApp.src.Services.Chemistry.CompoundFunction;
using LaboratoryApp.src.Services.Chemistry.PeriodicFunction;
using LaboratoryApp.src.Services.Chemistry.ReactionFunction;

using LaboratoryApp.src.Services.English.DiaryFunction;
using LaboratoryApp.src.Services.English.DictionaryFunction;
using LaboratoryApp.src.Services.English.FlashcardFunction;

using LaboratoryApp.src.Services.Helper.AI;
using LaboratoryApp.src.Services.Helper.Counter;
using LaboratoryApp.src.Services.Helper.Speech;

using LaboratoryApp.src.Shared;
using LaboratoryApp.src.Shared.Interface;

using LaboratoryApp.src.UI.ViewModels;
using LaboratoryApp.src.UI.Views;

using Microsoft.Extensions.DependencyInjection;

using System.Configuration;
using System.Windows;

namespace LaboratoryApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;
        private readonly string MongoConnString = SecureConfigHelper.Decrypt(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
        private readonly string _chemDbPath = ConfigurationManager.AppSettings["ChemistryDbPath"]!; // ! để báo cho trình biên dịch biết rằng giá trị này không bao giờ là null
        private readonly string _englishDbPath = ConfigurationManager.AppSettings["EnglishDbPath"]!; // ! để báo cho trình biên dịch biết rằng giá trị này không bao giờ là null

        /// <summary>
        /// Construction
        /// </summary>
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Đăng ký các thiết lập cơ sở dữ liệu
            #region DatabaseSettings
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(MongoConnString, DatabaseName.AssignmentMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(MongoConnString, DatabaseName.AuthenticationMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(MongoConnString, DatabaseName.AuthorizationMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(MongoConnString, DatabaseName.ChemistryMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(MongoConnString, DatabaseName.EnglishMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(MongoConnString, DatabaseName.HelperMongoDB));
            services.AddSingleton<ISQLiteDataProvider>(sp => new SQLiteDataProvider(_chemDbPath));
            services.AddSingleton<ISQLiteDataProvider>(sp => new SQLiteDataProvider(_englishDbPath));
            #endregion

            // Đăng ký các caches
            #region Caches
            services.AddSingleton<IAssignmentCache, AssignmentCache>();
            services.AddSingleton<IAuthorizationCache, AuthorizationCache>();
            services.AddSingleton<IChemistryDataCache, ChemistryDataCache>();
            services.AddSingleton<IEnglishDataCache, EnglishDataCache>();
            #endregion

            // Đăng ký các providers
            #region Providers
            // Assignment
            services.AddSingleton<IExerciseProvider, ExerciseProvider>();
            services.AddSingleton<IExerciseSetProvider, ExerciseSetProvider>();
            services.AddSingleton<IQuestionProvider, QuestionProvider>();

            // Authentication
            services.AddSingleton<IOrganizationProvider, OrganizationProvider>();
            services.AddSingleton<IUserOrganizationProvider, UserOrganizationProvider>();
            services.AddSingleton<IUserProvider, UserProvider>();
            services.AddSingleton<IRefreshTokenProvider, RefreshTokenProvider>();

            // Authorization
            services.AddSingleton<IExerciseSetAccessProvider, ExerciseSetAccessProvider>();

            // Chemistry
            services.AddSingleton<ICompoundProvider, CompoundProvider>();
            services.AddSingleton<IPeriodicProvider, PeriodicProvider>();
            services.AddSingleton<IReactionProvider, ReactionProvider>();

            // English
            services.AddSingleton<IDiaryProvider, DiaryProvider>();
            services.AddSingleton<IDictionaryProvider, DictionaryProvider>();
            services.AddSingleton<IFlashcardProvider, FlashcardProvider>();
            #endregion

            // Đăng ký các dịch vụ cần thiết
            #region Services
            services.AddSingleton<INavigationService, NavigateService>();

            services.AddSingleton<IExerciseService, ExerciseService>();
            services.AddSingleton<IExerciseSetService, ExerciseSetService>();
            services.AddSingleton<IQuestionService, QuestionService>();

            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IOrganizationService, OrganizationService>();
            services.AddSingleton<IUserOrganizationService, UserOrganizationService>();

            services.AddSingleton<ICompoundService, CompoundService>();
            services.AddSingleton<IPeriodicService, PeriodicService>();
            services.AddSingleton<IReactionService, ReactionService>();

            services.AddSingleton<IDiaryService, DiaryService>();
            services.AddSingleton<IDictionaryService, DictionaryService>();
            services.AddSingleton<IFlashcardService, FlashcardService>();

            services.AddSingleton<IAIService, AIService>();
            services.AddSingleton<ICounterService, CounterService>();
            SpeechService.Setup();
            #endregion

            // Đăng ký các ViewModels theo modules
            #region UI
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<ControlBarViewModel>();
            #endregion

            #region Authentication
            services.AddTransient<AuthenticationViewModel>();
            services.AddTransient<RoleSelectionViewModel>();
            #endregion

            #region Assignment
            services.AddTransient<ExerciseSetManagerViewModel>();
            services.AddTransient<InsertExerciseSetViewModel>();

            services.AddTransient<Func<INavigationService, IServiceProvider, IAssignmentCache, IAuthorizationCache, IExerciseService, ExerciseSet, ExerciseManagerViewModel>>(sp =>
            (navigationService, serviceProvider, assignmentCache, authorizationCache, exerciseService, selectedSet) =>
            {
                var addExerciseVmFactory = sp.GetRequiredService<Func<IExerciseService, ExerciseSet, ExerciseViewModel>>();
                var exerciseDetailVmFactory = sp.GetRequiredService<Func<IServiceProvider, IAuthorizationCache, INavigationService, long, Exercise, QuestionManagerViewModel>>();
                return new ExerciseManagerViewModel(navigationService, serviceProvider, assignmentCache, authorizationCache, exerciseService, selectedSet, addExerciseVmFactory, exerciseDetailVmFactory);
            });

            services.AddTransient<Func<IServiceProvider, IAuthorizationCache, INavigationService, long, Exercise, QuestionManagerViewModel>>(sp =>
            (serviceProvider, authorizationCache, navigationService, setId, selectedExercise) =>
            {
                return new QuestionManagerViewModel(serviceProvider, authorizationCache, navigationService, setId, selectedExercise);
            });
            #endregion

            #region Chemistry
            services.AddTransient<ChemistryMainPageViewModel>();

            services.AddTransient<CompoundManagerViewModel>();

            services.AddTransient<ElementInfoViewModel>();
            services.AddTransient<PeriodicTableViewModel>();

            services.AddTransient<ReactionManagerViewModel>();
            services.AddTransient<Func<IChemistryDataCache, Reaction, ReactionSelectionResultViewModel>>(sp =>
            (cache, selectedReaction) => ActivatorUtilities.CreateInstance<ReactionSelectionResultViewModel>(sp, cache, selectedReaction));
            #endregion

            #region English
            services.AddTransient<EnglishMainPageViewModel>();

            services.AddTransient<DiaryViewModel>();
            services.AddTransient<Func<IServiceProvider, IAIService, IDiaryService, DiaryContent, DiaryViewModel>>(sp =>
            (service, aiService, diaryService, diary) =>
            {
                return new DiaryViewModel(service, aiService, diaryService, diary);
            });
            services.AddTransient<DiaryManagerViewModel>();
            services.AddTransient<Func<IServiceProvider, IAIService, IDiaryService, IUserProvider, DiaryContent, DiaryDetailViewModel>>(sp =>
            (service, aiService, diaryService, userService, diary) =>
            {
                var diaryVmFactory = sp.GetRequiredService<Func<IServiceProvider, IAIService, IDiaryService, DiaryContent, DiaryViewModel>>();
                return new DiaryDetailViewModel(service, aiService, diaryService, userService, diary, diaryVmFactory);
            });

            services.AddTransient<DictionaryViewModel>();

            // Đăng ký các hàm tạo cho FlashcardViewModel và FlashcardStudyViewModel
            services.AddTransient<FlashcardManagerViewModel>();
            services.AddTransient<FlashcardViewModel>();
            services.AddTransient<FlashcardSetViewModel>();
            services.AddTransient<FlashcardStudyViewModel>();
            services.AddTransient<Func<IFlashcardService, FlashcardSet, FlashcardSetViewModel>>(sp => (service, set) => new FlashcardSetViewModel(service, set));
            services.AddTransient<Func<IServiceProvider, IFlashcardService, long, Flashcard, FlashcardViewModel>>(sp => (sp, service, setId, card) => new FlashcardViewModel(sp, service, setId, card));
            services.AddTransient<Func<FlashcardSet, IFlashcardService, FlashcardStudyViewModel>>(sp => (set, service) => new FlashcardStudyViewModel(set, service));

            services.AddTransient<LectureMainPageViewModel>();
            services.AddTransient<LectureContentViewModel>();
            #endregion

            #region Maths
            services.AddTransient<MathsMainPageViewModel>();
            #endregion

            #region Physics
            services.AddTransient<PhysicsMainPageViewModel>();
            #endregion

            #region Teacher
            services.AddTransient<TeacherMainPageViewModel>();

            services.AddTransient<ExerciseSetViewModel>();
            services.AddTransient<Func<IExerciseService, ExerciseSet, ExerciseViewModel>>(sp =>
            (service, currSet) =>
            {
                return new ExerciseViewModel(service, currSet);
            });

            services.AddTransient<QuestionViewModel>();

            services.AddTransient<CompoundComponentViewModel>();
            services.AddTransient<CompoundNoteViewModel>();
            services.AddTransient<CompoundViewModel>();

            services.AddTransient<ReactionComponentViewModel>();
            services.AddTransient<ReactionNoteViewModel>();
            services.AddTransient<ReactionViewModel>();
            #endregion

            #region Toolkits
            services.AddTransient<ToolkitsViewModel>();
            #endregion

            // Đăng ký các Views và ViewModels theo modules
            #region UI
            services.AddTransient<MainWindow>();

            services.AddTransient<Dashboard>(sp =>
            {
                var vm = sp.GetRequiredService<DashboardViewModel>();
                return new Dashboard { DataContext = vm };
            });

            services.AddTransient<ToolkitsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<ToolkitsViewModel>();
                return new ToolkitsMainPage { DataContext = vm };
            });
            #endregion

            #region Authentication
            services.AddTransient<AuthenticationWindow>(sp =>
            {
                var vm = sp.GetRequiredService<AuthenticationViewModel>();
                return new AuthenticationWindow { DataContext = vm };
            });

            services.AddTransient<RoleSelectionWindow>(sp =>
            {
                var vm = sp.GetRequiredService<RoleSelectionViewModel>();
                return new RoleSelectionWindow { DataContext = vm };
            });
            #endregion

            #region Assignment
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

            services.AddTransient<ExerciseManagerPage>();

            services.AddTransient<QuestionManagerPage>();
            #endregion

            #region Chemistry
            services.AddTransient<ChemistryMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<ChemistryMainPageViewModel>();
                return new ChemistryMainPage { DataContext = vm };
            });

            services.AddTransient<PeriodicTableWindow>(sp =>
            {
                var vm = sp.GetRequiredService<PeriodicTableViewModel>();
                return new PeriodicTableWindow { DataContext = vm };
            });

            services.AddTransient<CompoundManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<CompoundManagerViewModel>();
                return new CompoundManagerPage { DataContext = vm };
            });

            services.AddTransient<ReactionManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<ReactionManagerViewModel>();
                return new ReactionManagerPage { DataContext = vm };
            });
            services.AddTransient<ReactionSelectionResultWindow>();
            #endregion

            #region English
            services.AddTransient<EnglishMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<EnglishMainPageViewModel>();
                return new EnglishMainPage { DataContext = vm };
            });

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

            services.AddTransient<DictionaryWindow>(sp =>
            {
                var vm = sp.GetRequiredService<DictionaryViewModel>();
                return new DictionaryWindow { DataContext = vm };
            });
            services.AddTransient<Func<DictionaryWindow>>(sp =>
            {
                return () => sp.GetRequiredService<DictionaryWindow>();
            });

            services.AddTransient<FlashcardManagerPage>(sp =>
            {
                var vm = sp.GetRequiredService<FlashcardManagerViewModel>();
                return new FlashcardManagerPage { DataContext = vm };
            });
            services.AddTransient<FlashcardStudyWindow>();
            services.AddTransient<FlashcardWindow>();
            services.AddTransient<UpdateFlashcardSetWindow>();

            services.AddTransient<LectureMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<LectureMainPageViewModel>();
                return new LectureMainPage { DataContext = vm };
            });
            #endregion

            #region Maths
            services.AddTransient<MathsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<MathsMainPageViewModel>();
                return new MathsMainPage { DataContext = vm };
            });
            #endregion

            #region Physics
            services.AddTransient<PhysicsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<PhysicsMainPageViewModel>();
                return new PhysicsMainPage { DataContext = vm };
            });
            #endregion

            #region Teacher
            services.AddTransient<TeacherMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<TeacherMainPageViewModel>();
                return new TeacherMainPage { DataContext = vm };
            });

            services.AddTransient<AddExerciseSetWindow>(sp =>
            {
                var vm = sp.GetRequiredService<ExerciseSetViewModel>();
                return new AddExerciseSetWindow { DataContext = vm };
            });
            services.AddTransient<AddExerciseWindow>();
            services.AddTransient<AddQuestionWindow>(sp =>
            {
                var vm = sp.GetRequiredService<QuestionViewModel>();
                return new AddQuestionWindow { DataContext = vm };
            });

            services.AddTransient<AddCompoundWindow>(sp =>
            {
                var vm = sp.GetRequiredService<CompoundViewModel>();
                return new AddCompoundWindow { DataContext = vm };
            });

            services.AddTransient<AddReactionWindow>(sp =>
            {
                var vm = sp.GetRequiredService<ReactionViewModel>();
                return new AddReactionWindow { DataContext = vm };
            });
            #endregion

            #region Toolkits
            services.AddTransient<ToolkitsMainPage>(sp =>
            {
                var vm = sp.GetRequiredService<ToolkitsViewModel>();
                return new ToolkitsMainPage { DataContext = vm };
            });
            #endregion

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Gọi providers
            var exerciseProvider = _serviceProvider.GetRequiredService<IExerciseProvider>();
            var exerciseSetProvider = _serviceProvider.GetRequiredService<IExerciseSetProvider>();
            var questionProvider = _serviceProvider.GetRequiredService<IQuestionProvider>();

            var exerciseSetAccessProvider = _serviceProvider.GetRequiredService<IExerciseSetAccessProvider>();

            var periodicProvider = _serviceProvider.GetRequiredService<IPeriodicProvider>();
            var compoundProvider = _serviceProvider.GetRequiredService<ICompoundProvider>();
            var reactionProvider = _serviceProvider.GetRequiredService<IReactionProvider>();

            var diaryProvider = _serviceProvider.GetRequiredService<IDiaryProvider>();
            var dictionaryProvider = _serviceProvider.GetRequiredService<IDictionaryProvider>();

            // Gọi caches
            var assignmentCache = _serviceProvider.GetRequiredService<IAssignmentCache>();
            var authorizationCache = _serviceProvider.GetRequiredService<IAuthorizationCache>();
            var chemistryCache = _serviceProvider.GetRequiredService<IChemistryDataCache>();
            var englishCache = _serviceProvider.GetRequiredService<IEnglishDataCache>();

            // Lấy dữ liệu
            assignmentCache.LoadAllData(exerciseProvider, exerciseSetProvider, questionProvider);
            authorizationCache.LoadAllData(exerciseSetAccessProvider);
            chemistryCache.LoadAllData(periodicProvider, compoundProvider, reactionProvider);
            englishCache.LoadAllData(diaryProvider, dictionaryProvider);

            // Lấy MainWindowViewModel từ ServiceProvider
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            var mainWindowVM = _serviceProvider.GetRequiredService<MainWindowViewModel>();

            mainWindow.DataContext = mainWindowVM;
            navigationService.Initialize(mainWindow.MainFrame);

            mainWindowVM.Initialize();

            // Hiển thị MainWindow
            mainWindow.Show();
            base.OnStartup(e);
        }
    }

}
