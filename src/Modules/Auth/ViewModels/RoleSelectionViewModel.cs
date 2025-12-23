using LaboratoryApp.Domain.DTOs.Users;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Auth.Views;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Auth.ViewModels
{
    public class RoleSelectionViewModel : BaseViewModel
    {
        private UserOrganizationProfileDTO? _selectedRole;
        private List<UserOrganizationProfileDTO> _roles;

        #region Commands
        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmRoleCommand { get; set; }
        #endregion

        #region Properties
        public UserOrganizationProfileDTO? SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        public List<UserOrganizationProfileDTO> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }
        #endregion

        public RoleSelectionViewModel()
        {
            CancelCommand = new RelayCommand<object>(p => true, p =>
            {
                SelectedRole = null;
                CloseWindow(p);
            });

            ConfirmRoleCommand = new RelayCommand<object>(p => CanConfirmRole(), p =>
            {
                if (AuthenticationCache.CurrentAuthentication != null && SelectedRole != null)
                {
                    AuthenticationCache.CurrentAuthentication.CurrentOrganizationProfile = SelectedRole;
                }

                CloseWindow(p);
            });
        }

        private void CloseWindow(object p)
        {
            if (p is RoleSelectionWindow window)
            {
                window.Close();
            }
        }

        private bool CanConfirmRole()
        {
            return SelectedRole != null;
        }
    }
}
