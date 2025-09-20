using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.ExerciseFunction.Views;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;

namespace LaboratoryApp.src.Modules.English.ExerciseFunction.ViewModels
{
    public class ExerciseSetManagerViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;

        private bool _isTeacher;

        #region Commands
        public ICommand AddNewSetCommand { get; set; }
        #endregion

        #region Properties
        public bool IsTeacher
        {
            get => _isTeacher;
            set
            {
                _isTeacher = value;
                OnPropertyChanged(nameof(IsTeacher));
            }
        }
        #endregion

        public ExerciseSetManagerViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            AuthenticationCache.CurrentUserChanged += OnUserChanged;

            AddNewSetCommand = new RelayCommand<object>(p => true, (p) =>
            {
                // Thêm mới bài tập
                var window = _serviceProvider.GetRequiredService<AddExerciseSetWindow>();
                if(window.DataContext is ExerciseSetViewModel vm && vm is IAsyncInitializable init)
                {
                    // Khởi tạo dữ liệu bất đồng bộ
                    _ = init.InitializeAsync();
                }
                window.ShowDialog();
            });
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Khởi tạo dữ liệu bất đồng bộ ở đây
                _isTeacher = AuthenticationCache.RoleId == 2;
            }, cancellationToken);
        }

        private void OnUserChanged(UserDTO? user)
        {
            IsTeacher = AuthenticationCache.RoleId == 2;
        }
    }
}
