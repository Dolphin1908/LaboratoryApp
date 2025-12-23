using LaboratoryApp.Domain.Interfaces.English;
using LaboratoryApp.Domain.Interfaces.Providers.Auth;
using LaboratoryApp.Domain.Interfaces.Providers.Chemistry;
using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Interfaces.Providers.English;
using LaboratoryApp.Domain.Interfaces.Providers.Infrastructure;
using LaboratoryApp.Domain.Interfaces.Providers.Operations;
using LaboratoryApp.Domain.Interfaces.Providers.Users;
using LaboratoryApp.Domain.Interfaces.Services.Auth;
using LaboratoryApp.Domain.Interfaces.Services.Chemistry;
using LaboratoryApp.Domain.Interfaces.Services.Common;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Interfaces.Services.English;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Interfaces.Services.Operations;
using LaboratoryApp.Domain.Interfaces.Services.Users;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.Caches.English;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Data.Providers.Auth;
using LaboratoryApp.src.Data.Providers.Chemistry;
using LaboratoryApp.src.Data.Providers.Common;
using LaboratoryApp.src.Data.Providers.Content;
using LaboratoryApp.src.Data.Providers.English;
using LaboratoryApp.src.Data.Providers.Infrastructure;
using LaboratoryApp.src.Data.Providers.Operations;
using LaboratoryApp.src.Data.Providers.Users;
using LaboratoryApp.src.Services.Auth;
using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Services.Common;
using LaboratoryApp.src.Services.Content;
using LaboratoryApp.src.Services.English;
using LaboratoryApp.src.Services.Infrastructure;
using LaboratoryApp.src.Services.Operations;
using LaboratoryApp.src.Services.UI;
using LaboratoryApp.src.Services.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace LaboratoryApp.src.Configuration
{
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Khởi tạo và đăng ký tất cả các dịch vụ của ứng dụng
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // Register application services here
            services.AddCaches();
            services.AddDatabase();
            services.AddPresentation();
            services.AddProviders();
            services.AddServices();

            return services;
        }

        /// <summary>
        /// Khởi tạo và đăng ký các dịch vụ liên quan đến cơ sở dữ liệu
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            // Register database-related services here
            var mongoConnString = SecureConfigHelper.Decrypt(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
            var chemDbPath = ConfigurationManager.AppSettings["ChemistryDbPath"]!; // ! để báo cho trình biên dịch biết rằng giá trị này không bao giờ là null
            var englishDbPath = ConfigurationManager.AppSettings["EnglishDbPath"]!; // ! để báo cho trình biên dịch biết rằng giá trị này không bao giờ là null

            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(mongoConnString, DatabaseName.AssignmentMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(mongoConnString, DatabaseName.AuthenticationMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(mongoConnString, DatabaseName.AuthorizationMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(mongoConnString, DatabaseName.ChemistryMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(mongoConnString, DatabaseName.EnglishMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(mongoConnString, DatabaseName.HelperMongoDB));
            services.AddSingleton<IMongoDBProvider>(sp => new MongoDBProvider(mongoConnString, DatabaseName.InfrastructureMongoDB));
            services.AddSingleton<ISQLiteDataProvider>(sp => new SQLiteDataProvider(chemDbPath));
            services.AddSingleton<ISQLiteDataProvider>(sp => new SQLiteDataProvider(englishDbPath));

            return services;
        }

        /// <summary>
        /// Khởi tạo và đăng ký các dịch vụ liên quan đến cache
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddCaches(this IServiceCollection services)
        {
            // Register cache-related services here
            services.AddSingleton<IAuthorizationCache, AuthorizationCache>();
            services.AddSingleton<IChemistryDataCache, ChemistryDataCache>();
            services.AddSingleton<IEnglishDataCache, EnglishDataCache>();

            return services;
        }

        /// <summary>
        /// Khởi tạo và đăng ký các Providers
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddProviders(this IServiceCollection services)
        {
            // Register provider-related services here
            // Admin

            // Auth
            services.AddSingleton<IRefreshTokenProvider, RefreshTokenProvider>();

            // Chemistry
            services.AddSingleton<ICompoundProvider, CompoundProvider>();
            services.AddSingleton<IPeriodicProvider, PeriodicProvider>();
            services.AddSingleton<IReactionProvider, ReactionProvider>();

            // Content
            services.AddSingleton<IAnswerOptionProvider, AnswerOptionProvider>();
            services.AddSingleton<IExerciseProvider, ExerciseProvider>();
            services.AddSingleton<IQuestionBlockProvider, QuestionBlockProvider>();
            services.AddSingleton<IQuestionProvider, QuestionProvider>();

            // English
            services.AddSingleton<IDiaryProvider, DiaryProvider>();
            services.AddSingleton<IDictionaryProvider, DictionaryProvider>();
            services.AddSingleton<IFlashcardProvider, FlashcardProvider>();

            // Infrastructure
            services.AddSingleton<IAssetProvider, AssetProvider>();

            // Maths

            // Operations
            services.AddSingleton<IExerciseAccessProvider, ExerciseAccessProvider>();

            // Physics

            // Users
            services.AddSingleton<IOrganizationProvider, OrganizationProvider>();
            services.AddSingleton<IUserOrganizationProvider, UserOrganizationProvider>();
            services.AddSingleton<IUserProvider, UserProvider>();

            return services;
        }

        /// <summary>
        /// Khởi tạo và đăng ký các Services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register service-related services here
            // Auth
            services.AddSingleton<IAuthenticationService, AuthenticationService>();

            // Chemistry
            services.AddSingleton<ICompoundService, CompoundService>();
            services.AddSingleton<IPeriodicService, PeriodicService>();
            services.AddSingleton<IReactionService, ReactionService>();

            // Common
            services.AddSingleton<ICounterService, CounterService>();

            // Content
            services.AddSingleton<IAnswerOptionService, AnswerOptionService>();
            services.AddSingleton<IExerciseService, ExerciseService>();
            services.AddSingleton<IQuestionBlockService, QuestionBlockService>();
            services.AddSingleton<IQuestionService, QuestionService>();

            // English
            services.AddSingleton<IDiaryService, DiaryService>();
            services.AddSingleton<IDictionaryService, DictionaryService>();
            services.AddSingleton<IFlashcardService, FlashcardService>();

            // Infrastructure
            services.AddSingleton<IAIService, AIService>();
            services.AddSingleton<IAssetService, AssetService>();
            services.AddSingleton<IFormatConversionService, FormatConversionService>();
            services.AddSingleton<ISpeechService, SpeechService>();

            // Operations
            services.AddSingleton<IExerciseAccessService, ExerciseAccessService>();

            // UI
            services.AddTransient<NavigateService>();
            services.AddTransient<Func<INavigateService>>(sp => () => sp.GetRequiredService<NavigateService>());
            services.AddSingleton<INavigateService, NavigateService>();
            services.AddSingleton<IDialogService, DialogService>();

            // Users
            services.AddSingleton<IOrganizationService, OrganizationService>();
            services.AddSingleton<IUserOrganizationService, UserOrganizationService>();

            return services;
        }
    }
}