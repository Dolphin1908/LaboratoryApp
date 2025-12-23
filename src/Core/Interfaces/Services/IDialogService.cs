using System.Windows;

namespace LaboratoryApp.src.Core.Interfaces.Services
{
    public interface IDialogService
    {
        MessageBoxResult ShowMessage(string message, string title = "Thông báo", MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.Information);
        bool? ShowDialogCenterOwner(Window window);
        void ShowCenterOwner(Window window);

        void ShowAddCompoundCommand();
        void ShowAddReactionCommand();
    }
}
