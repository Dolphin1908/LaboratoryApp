using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Data.Providers.English;
using LaboratoryApp.src.Data.Providers.English.DiaryFunction;
using LaboratoryApp.src.Modules.English.DiaryFunction.Views;
using LaboratoryApp.src.Services.English;
using LaboratoryApp.src.Services.English.DiaryFunction;
using LaboratoryApp.src.Services.Helper.AI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels
{
    public class DiaryDetailViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAIService _aiService;
        private readonly IDiaryService _diaryService;
        private readonly IUserProvider _userProvider;

        private readonly Func<IServiceProvider, IAIService, IDiaryService, DiaryContent, DiaryViewModel> _diaryEditvmFactory;

        private DiaryContent _diaryContent;
        private FlowDocument _boundDocument;
        private User _author;
        private bool _isAuthor;

        #region Properties
        public DiaryContent DiaryContent
        {
            get => _diaryContent;
            set
            {
                _diaryContent = value;
                OnPropertyChanged(nameof(DiaryContent));
            }
        }
        public FlowDocument BoundDocument
        {
            get => _boundDocument;
            set
            {
                _boundDocument = value;
                OnPropertyChanged(nameof(BoundDocument));
            }
        }
        public User Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }
        public bool IsAuthor
        {
            get => _isAuthor;
            set
            {
                _isAuthor = value;
                OnPropertyChanged(nameof(IsAuthor));
            }
        }
        #endregion

        #region Commands
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion

        public DiaryDetailViewModel(IServiceProvider serviceProvider,
                                    IAIService aiService,
                                    IDiaryService diaryService,
                                    IUserProvider userProvider,
                                    DiaryContent diaryContent,
                                    Func<IServiceProvider, IAIService, IDiaryService, DiaryContent, DiaryViewModel> diaryEditVmFactory)
        {
            _serviceProvider = serviceProvider;
            _aiService = aiService;
            _diaryService = diaryService;
            _userProvider = userProvider;
            _diaryEditvmFactory = diaryEditVmFactory;

            _diaryContent = diaryContent; // Tạo bản sao để tránh thay đổi trực tiếp đối tượng gốc
            _diaryContent.CreatedAt = _diaryContent.CreatedAt.ToLocalTime(); // Chuyển đổi sang giờ địa phương
            _diaryContent.UpdatedAt = _diaryContent.UpdatedAt.ToLocalTime(); // Chuyển đổi sang giờ địa phương
            _boundDocument = FlowDocumentSerializer.DeserializeFromBytes(diaryContent.ContentBytes) ?? new FlowDocument(); // Chuyển đổi byte[] sang FlowDocument
            _isAuthor = AuthenticationCache.CurrentUser?.Id == diaryContent.UserId; // Kiểm tra xem người dùng hiện tại có phải là tác giả của nhật ký không
            _author = _userProvider.GetUserById(diaryContent.UserId) ?? new User(); // Lấy thông tin tác giả từ UserProvider

            EditCommand = new RelayCommand<object>((p)=> true, (p) =>
            {
                // Gán ViewModel cho DiaryViewModel
                var vm = _diaryEditvmFactory(_serviceProvider, _aiService, _diaryService, _diaryContent);

                var window = _serviceProvider.GetRequiredService<DiaryWindow>();
                window.DataContext = vm;

                var result = window.ShowDialog();

                if (p is DiaryDetailWindow thisWin && result == true)
                {
                    thisWin.Close();
                }
            });

            DeleteCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                if (MessageBox.Show($"Bạn có muốn xóa {DiaryContent.Title}?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _diaryService.DeleteDiaryAsync(DiaryContent);

                        if (p is DiaryDetailWindow thisWin)
                        {
                            thisWin.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Xóa nhật ký thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }
    }
}