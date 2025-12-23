using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.Enums.Users;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.Dashboard.Views;
using LaboratoryApp.src.Modules.Tools.English.Dashboard.Views;
using LaboratoryApp.src.Modules.Tools.Maths.Dashboard.Views;
using LaboratoryApp.src.Modules.Tools.Physics.Dashboard.Views;
using LaboratoryApp.src.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace LaboratoryApp.src.UI.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly INavigateService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        private bool _isStudent;

        public bool IsStudent
        {
            get => _isStudent;
            set
            {
                _isStudent = value;
                OnPropertyChanged(nameof(IsStudent));
            }
        }

        #region Commands
        public ICommand NavigateToMathMainPage { get; set; } // Math
        public ICommand NavigateToPhysicsMainPage { get; set; } // Physics
        public ICommand NavigateToChemistryMainPage { get; set; } // Chemistry
        public ICommand NavigateToEnglishMainPage { get; set; } // English
        public ICommand NavigateToAssignmentMainPage { get; set; } // Assignment
        #endregion

        public DashboardViewModel(INavigateService navigationService, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            AuthenticationCache.CurrentAuthenticationChanged += OnUserChanged;

            // Navigate to the math page
            NavigateToMathMainPage = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<MathsMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the physics page
            NavigateToPhysicsMainPage = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<PhysicsMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the chemistry page
            NavigateToChemistryMainPage = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<ChemistryMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the english page
            NavigateToEnglishMainPage = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<EnglishMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the assignment page
            //NavigateToAssignmentMainPage = new RelayCommand<object>((p) => IsStudent, (p) =>
            //{
            //    var page = _serviceProvider.GetRequiredService<ExerciseSetManagerPage>();
            //    if (page.DataContext is ExerciseSetManagerViewModel vm && vm is IAsyncInitializable init)
            //    {
            //        _ = init.InitializeAsync();
            //    }
            //    _navigationService.NavigateTo(page);
            //});
        }

        private void OnUserChanged(AuthenticationResponseDTO user)
        {
            IsStudent = user?.CurrentOrganizationProfile?.Role.HasFlag(UserRole.Student) ?? false;
        }
    }
}
