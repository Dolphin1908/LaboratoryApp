using LaboratoryApp.Domain.Interfaces.Providers.Users;
using LaboratoryApp.Domain.Interfaces.Services.English;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.English.DiaryFunction;
using LaboratoryApp.Domain.Models.Users;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Tools.English.DiaryFunction.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Tools.English.DiaryFunction.ViewModels
{
    public class DiaryDetailViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAIService _aiService;
        private readonly IDialogService _dialogService;
        private readonly IDiaryService _diaryService;
        private readonly IFormatConversionService _formatConversionService;
        private readonly IUserProvider _userProvider;

        private readonly Func<IServiceProvider, IAIService, IDialogService, IDiaryService, IFormatConversionService, DiaryContent, DiaryViewModel> _diaryEditvmFactory;

        private string _xamlDocument;
        private DiaryContent _diaryContent;
        private User? _author;
        private bool _isAuthor;

        #region Properties
        public string XamlDocument
        {
            get => _xamlDocument;
            set
            {
                _xamlDocument = value;
                OnPropertyChanged(nameof(XamlDocument));
            }
        }
        public DiaryContent DiaryContent
        {
            get => _diaryContent;
            set
            {
                _diaryContent = value;
                OnPropertyChanged(nameof(DiaryContent));
            }
        }
        public User? Author
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
                                    IDialogService dialogService,
                                    IDiaryService diaryService,
                                    IFormatConversionService formatConversionService,
                                    IUserProvider userProvider,
                                    DiaryContent diaryContent,
                                    Func<IServiceProvider, IAIService, IDialogService, IDiaryService, IFormatConversionService, DiaryContent, DiaryViewModel> diaryEditVmFactory)
        {
            _serviceProvider = serviceProvider;
            _aiService = aiService;
            _dialogService = dialogService;
            _diaryService = diaryService;
            _formatConversionService = formatConversionService;
            _userProvider = userProvider;
            _diaryEditvmFactory = diaryEditVmFactory;

            _diaryContent = diaryContent; // Tạo bản sao để tránh thay đổi trực tiếp đối tượng gốc
            _diaryContent.CreatedAt = _diaryContent.CreatedAt.ToLocalTime(); // Chuyển đổi sang giờ địa phương
            _diaryContent.UpdatedAt = _diaryContent.UpdatedAt.HasValue ? _diaryContent.UpdatedAt.Value.ToLocalTime() : (DateTime?)null; // Chuyển đổi sang giờ địa phương
            _xamlDocument = _formatConversionService.DeserializeStringFromByteArray(diaryContent.ContentBytes); // Chuyển đổi byte[] sang XAML string
            _isAuthor = AuthenticationCache.CurrentAuthentication?.User.Id == diaryContent.UserId; // Kiểm tra xem người dùng hiện tại có phải là tác giả của nhật ký không
            InitializeAuthorAsync(diaryContent.UserId); // Khởi tạo tác giả

            EditCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Gán ViewModel cho DiaryViewModel
                var vm = _diaryEditvmFactory(_serviceProvider, _aiService, _dialogService, _diaryService, _formatConversionService, _diaryContent);

                var window = _serviceProvider.GetRequiredService<DiaryWindow>();
                window.DataContext = vm;

                var result = _dialogService.ShowDialogCenterOwner(window);

                if (p is DiaryDetailWindow thisWin && result == true)
                {
                    thisWin.Close();
                }
            });

            DeleteCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                if (_dialogService.ShowMessage($"Bạn có muốn xóa {DiaryContent.Title}?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
                        _dialogService.ShowMessage($"Xóa nhật ký thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }

        // Thêm phương thức async void để khởi tạo Author
        private async void InitializeAuthorAsync(long userId)
        {
            var user = await _userProvider.GetUserByIdAsync(userId);
            Author = user ?? new User();
        }
    }
}