using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.Interfaces.Providers.Users;
using LaboratoryApp.Domain.Interfaces.Services.English;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.English;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Tools.English.DiaryFunction.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Tools.English.DiaryFunction.ViewModels
{
    public class DiaryManagerViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAIService _aiService;
        private readonly IDialogService _dialogService;
        private readonly IDiaryService _diaryService;
        private readonly IFormatConversionService _formatConversionService;
        private readonly IUserProvider _userProvider;
        private readonly IEnglishDataCache _englishDataCache;

        // Initialize _publicDiaries and _privateDiaries fields to avoid CS8618
        private ObservableCollection<DiaryContent> _publicDiaries = new ObservableCollection<DiaryContent>();
        private ObservableCollection<DiaryContent> _privateDiaries = new ObservableCollection<DiaryContent>();

        private readonly Func<IServiceProvider, IAIService, IDialogService, IDiaryService, IFormatConversionService, IUserProvider, DiaryContent, DiaryDetailViewModel> _diaryDetailvmFactory;

        #region Properties
        public ObservableCollection<DiaryContent> PublicDiaries
        {
            get => _publicDiaries;
            set
            {
                _publicDiaries = value;
                OnPropertyChanged(nameof(PublicDiaries));
            }
        }
        public ObservableCollection<DiaryContent> PrivateDiaries
        {
            get => _privateDiaries;
            set
            {
                _privateDiaries = value;
                OnPropertyChanged(nameof(PrivateDiaries));
            }
        }
        #endregion

        #region Commands
        public ICommand AddDiaryCommand { get; set; }
        public ICommand OpenDiaryDetailCommand { get; set; }
        #endregion

        public DiaryManagerViewModel(IServiceProvider serviceProvider,
                                     IAIService aiService,
                                     IDialogService dialogService,
                                     IDiaryService diaryService,
                                     IFormatConversionService formatConversionService,
                                     IUserProvider userProvider,
                                     IEnglishDataCache englishDataCache,
                                     Func<IServiceProvider, IAIService, IDialogService, IDiaryService, IFormatConversionService, IUserProvider, DiaryContent, DiaryDetailViewModel> diaryDetailvmFactory)
        {
            _serviceProvider = serviceProvider;
            _aiService = aiService;
            _dialogService = dialogService;
            _diaryService = diaryService;
            _formatConversionService = formatConversionService;
            _userProvider = userProvider;
            _englishDataCache = englishDataCache;

            _diaryDetailvmFactory = diaryDetailvmFactory;

            AuthenticationCache.CurrentAuthenticationChanged += OnUserChanged;

            #region Commands
            AddDiaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (AuthenticationCache.CurrentAuthentication == null)
                {
                    // Show a message box or notification to inform the user to log in
                    _dialogService.ShowMessage("Vui lòng đăng nhập để có thể viết nhật ký mới", "Yêu cầu đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var window = _serviceProvider.GetRequiredService<DiaryWindow>();
                _dialogService.ShowDialogCenterOwner(window);

                PublicDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.IsPublic == true).ToList());
                PrivateDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.UserId == (AuthenticationCache.CurrentAuthentication?.User.Id ?? 0)).ToList());
            });

            OpenDiaryDetailCommand = new RelayCommand<object>((p) => p is DiaryContent, (p) =>
            {
                var diary = p as DiaryContent;
                var window = _serviceProvider.GetRequiredService<DiaryDetailWindow>();

                window.DataContext = _diaryDetailvmFactory(_serviceProvider, _aiService, _dialogService, _diaryService, _formatConversionService, _userProvider, diary!);
                _dialogService.ShowDialogCenterOwner(window);

                PublicDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.IsPublic == true).ToList());
                PrivateDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.UserId == (AuthenticationCache.CurrentAuthentication?.User.Id ?? 0)).ToList());
            });
            #endregion
        }

        /// <summary>
        /// Load danh sách nhật ký
        /// </summary>
        /// <returns></returns>
        private async Task LoadDiariesAsync()
        {
            PublicDiaries.Clear();
            PrivateDiaries.Clear();

            // ViewModel giờ chỉ cần yêu cầu dữ liệu, không cần biết logic filter
            var publicItems = await _diaryService.GetPublicDiariesAsync();
            foreach (var item in publicItems) PublicDiaries.Add(item);

            if (AuthenticationCache.IsAuthenticated)
            {
                var privateItems = await _diaryService.GetPrivateDiariesForCurrentUserAsync(AuthenticationCache.CurrentAuthentication!.User.Id);
                foreach (var item in privateItems) PrivateDiaries.Add(item);
            }
        }

        /// <summary>
        /// Khởi tạo ViewModel bất đồng bộ
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await LoadDiariesAsync();
        }

        /// <summary>
        /// Xử lý khi người dùng đăng nhập hoặc đăng xuất
        /// </summary>
        /// <param name="user"></param>
        private async void OnUserChanged(AuthenticationResponseDTO? user)
        {
            await LoadDiariesAsync();
        }

        /// <summary>
        /// Hủy đăng ký sự kiện khi ViewModel bị hủy
        /// </summary>
        public void Dispose()
        {
            AuthenticationCache.CurrentAuthenticationChanged -= OnUserChanged;
        }
    }
}