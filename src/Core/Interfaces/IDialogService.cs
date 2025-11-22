using System.Windows;

namespace LaboratoryApp.src.Core.Interfaces
{
    public interface IDialogService
    {
        MessageBoxResult ShowMessage(string message, string title = "Thông báo", MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.Information);

        void ShowAddCompoundCommand();
        void ShowAddReactionCommand();
    }
}
