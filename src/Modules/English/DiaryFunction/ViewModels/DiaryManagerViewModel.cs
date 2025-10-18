using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.English;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Data.Providers.English;
using LaboratoryApp.src.Data.Providers.English.DiaryFunction;
using LaboratoryApp.src.Modules.English.DiaryFunction.Views;
using LaboratoryApp.src.Services.English.DiaryFunction;
using LaboratoryApp.src.Services.Helper.AI;
using LaboratoryApp.src.Shared.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels
{
    public class DiaryManagerViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAIService _aiService;
        private readonly IDiaryService _diaryService;
        private readonly IUserProvider _userProvider;
        private readonly IEnglishDataCache _englishDataCache;

        // Initialize _publicDiaries and _privateDiaries fields to avoid CS8618
        private ObservableCollection<DiaryContent> _publicDiaries = new ObservableCollection<DiaryContent>();
        private ObservableCollection<DiaryContent> _privateDiaries = new ObservableCollection<DiaryContent>();

        private readonly Func<IServiceProvider, IAIService, IDiaryService, IUserProvider, DiaryContent, DiaryDetailViewModel> _diaryDetailvmFactory;

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
                                     IDiaryService diaryService,
                                     IUserProvider userProvider,
                                     IEnglishDataCache englishDataCache,
                                     Func<IServiceProvider, IAIService, IDiaryService, IUserProvider, DiaryContent, DiaryDetailViewModel> diaryDetailvmFactory)
        {
            _serviceProvider = serviceProvider;
            _aiService = aiService;
            _diaryService = diaryService;
            _userProvider = userProvider;
            _englishDataCache = englishDataCache;

            _diaryDetailvmFactory = diaryDetailvmFactory;

            AuthenticationCache.CurrentUserChanged += OnUserChanged;

            #region Commands
            AddDiaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (AuthenticationCache.CurrentUser == null)
                {
                    // Show a message box or notification to inform the user to log in
                    MessageBox.Show("Vui lòng đăng nhập để có thể viết nhật ký mới", "Yêu cầu đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var window = _serviceProvider.GetRequiredService<DiaryWindow>();
                window.ShowDialog();

                PublicDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.IsPublic == true).ToList());
                PrivateDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.UserId == (AuthenticationCache.CurrentUser?.Id ?? 0)).ToList());
            });

            OpenDiaryDetailCommand = new RelayCommand<object>((p) => p is DiaryContent, (p) =>
            {
                var diary = p as DiaryContent;
                var window = _serviceProvider.GetRequiredService<DiaryDetailWindow>();

                window.DataContext = _diaryDetailvmFactory(_serviceProvider, _aiService, _diaryService, _userProvider, diary!);
                window.ShowDialog();

                PublicDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.IsPublic == true).ToList());
                PrivateDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.UserId == (AuthenticationCache.CurrentUser?.Id ?? 0)).ToList());
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
                var privateItems = await _diaryService.GetPrivateDiariesForCurrentUserAsync(AuthenticationCache.CurrentUser!.Id);
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
        private async void OnUserChanged(UserDTO? user)
        {
            await LoadDiariesAsync();
        }

        /// <summary>
        /// Hủy đăng ký sự kiện khi ViewModel bị hủy
        /// </summary>
        public void Dispose()
        {
            AuthenticationCache.CurrentUserChanged -= OnUserChanged;
        }
    }
}