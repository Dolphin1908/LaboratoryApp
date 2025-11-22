using LaboratoryApp.src.Modules.Auth.ViewModels;
using LaboratoryApp.src.Modules.Auth.Views;
using LaboratoryApp.src.UI.ViewModels;
using LaboratoryApp.src.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LaboratoryApp.src.Configuration
{
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Khởi tạo và đăng ký các dịch vụ liên quan đến giao diện người dùng
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            // Register presentation-related services here
            // Core UI
            services.AddCoreUI();

            // Modules
            services.AddAdminModule();
            services.AddAuthModule();
            services.AddSharedModule();
            services.AddStudentModule();
            services.AddTeacherModule();
            services.AddToolsModule();

            return services;
        }

        /// <summary>
        /// Khởi tạo và đăng ký các module liên quan đến UI gốc
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddCoreUI(this IServiceCollection services)
        {
            // Register admin-related modules here
            #region ViewModels
            services.AddTransient<ControlBarViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<MainWindowViewModel>();
            #endregion

            #region Views
            services.AddTransient<MainWindow>();
            services.AddTransient<Dashboard>(sp =>
            {
                var vm = sp.GetRequiredService<DashboardViewModel>();
                return new Dashboard { DataContext = vm };
            });
            #endregion

            #region Factories

            #endregion

            return services;
        }

        /// <summary>
        /// Khởi tạo và đăng ký các module liên quan đến quản trị
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddAdminModule(this IServiceCollection services)
        {
            // Register admin-related modules here
            #region ViewModels

            #endregion

            #region Views

            #endregion

            #region Factories

            #endregion

            return services;
        }

        /// <summary>
        /// Khởi tạo và đăng ký các module liên quan đến xác thực
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddAuthModule(this IServiceCollection services)
        {
            // Register authentication-related modules here
            #region ViewModels
            services.AddTransient<AuthenticationViewModel>();
            services.AddTransient<RoleSelectionViewModel>();
            #endregion

            #region Views
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

            #region Factories

            #endregion

            return services;
        }

        /// <summary>
        /// Khởi tạo và đăng ký các module dùng chung
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddSharedModule(this IServiceCollection services)
        {
            // Register shared-related modules here
            #region ViewModels

            #endregion

            #region Views

            #endregion

            #region Factories

            #endregion

            return services;
        }
    }
}
