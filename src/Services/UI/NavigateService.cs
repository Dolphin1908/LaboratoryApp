using LaboratoryApp.src.Core.Interfaces.Services;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace LaboratoryApp.src.Services.UI
{
    public class NavigateService : INavigateService
    {
        private Frame? _mainFrame;

        public NavigateService()
        {
            // Do nothing
        }

        public void Initialize(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        /// <summary>
        /// Chuyển hướng đến một trang mới.
        /// </summary>
        /// <param name="page"></param>
        public void NavigateTo(Page page)
        {
            _mainFrame?.Navigate(page);
        }

        /// <summary>
        /// Chuyển hướng trở lại trang trước đó nếu có thể.
        /// </summary>
        public void NavigateBack()
        {
            if (_mainFrame != null && _mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
            }
        }

        /// <summary>
        /// Chuyển hướng đến một trang mới và xóa lịch sử điều hướng trước đó.
        /// </summary>
        /// <param name="page"></param>
        public void NavigateToAndClearHistory(Page page)
        {
            if (_mainFrame == null) return;

            NavigatedEventHandler? handler = null; // Đăng ký sự kiện navigated tạm thời
            handler = (s, e) =>
            {
                _mainFrame.Navigated -= handler; // Hủy đăng ký sự kiện sau khi thực hiện xong
                while (_mainFrame.CanGoBack)
                {
                    _mainFrame.RemoveBackEntry(); // Xóa tất cả các mục trong lịch sử điều hướng
                }
            };

            _mainFrame.Navigated += handler; // Đăng ký sự kiện
            _mainFrame.Navigate(page); // Thực hiện điều hướng đến trang mới, sẽ kích hoạt sự kiện đã đăng ký
        }
    }
}
