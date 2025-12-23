using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.DashboardFunction.Views;
using LaboratoryApp.src.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.DashboardFunction.ViewModels
{
    public class TeacherToolsMainPageViewModel : BaseViewModel
    {
        private readonly INavigateService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        #region Commands
        public ICommand NavigateToAssessmentMainPageCommand { get; set; }
        #endregion

        public TeacherToolsMainPageViewModel(INavigateService navigationService, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            AuthenticationCache.CurrentAuthenticationChanged += OnUserChanged;

            NavigateToAssessmentMainPageCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(_serviceProvider.GetRequiredService<AssessmentMainPage>());
            });
        }

        private void OnUserChanged(AuthenticationResponseDTO? user)
        {
            // Handle user change logic here
        }
    }
}
