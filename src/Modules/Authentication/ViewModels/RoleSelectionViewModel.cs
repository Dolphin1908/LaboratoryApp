using LaboratoryApp.Domain.DTOs.Users;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Authentication.Views;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Authentication.ViewModels
{
    public class RoleSelectionViewModel : BaseViewModel
    {
        private UserOrganizationProfileDTO? _selectedRole;
        private List<UserOrganizationProfileDTO> _roles;

        #region Commands
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
            ConfirmRoleCommand = new RelayCommand<object>(p => CanConfirmRole(), p =>
            {
                if (p is RoleSelectionWindow window)
                {
                    window.Close();
                }
            });
        }

        private bool CanConfirmRole()
        {
            return SelectedRole != null;
        }
    }
}
