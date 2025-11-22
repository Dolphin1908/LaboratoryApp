using LaboratoryApp.src.Core.Interfaces;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LaboratoryApp.src.Services.UI
{
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region Common
        public MessageBoxResult ShowMessage(string message, string title = "Thông báo", MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.Information)
        {
            return MessageBox.Show(message, title, messageBoxButton, messageBoxImage);
        }
        #endregion

        #region Chemistry
        public void ShowAddCompoundCommand()
        {
            var window = _serviceProvider.GetRequiredService<AddCompoundWindow>();
            window.ShowDialog(); // Hiển thị cửa sổ nhập thông tin hợp chất (compound) mới.
        }

        public void ShowAddReactionCommand()
        {
            var window = _serviceProvider.GetRequiredService<AddReactionWindow>();
            window.ShowDialog();
        }
        #endregion
    }
}
