using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.DiaryFunction.Views;
using LaboratoryApp.src.Services.English;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;

namespace LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels
{
    public class DiaryManagerViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnglishService _englishService;
        private readonly IUserProvider _userProvider;
        private readonly EnglishDataCache _englishDataCache;

        private ObservableCollection<DiaryContent> _publicDiaries;
        private ObservableCollection<DiaryContent> _privateDiaries;

        private readonly Func<IUserProvider, DiaryContent, DiaryDetailViewModel> _diaryDetailvmFactory;

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
                                     EnglishDataCache englishDataCache,
                                     IEnglishService englishService,
                                     IUserProvider userProvider,
                                     Func<IUserProvider, DiaryContent, DiaryDetailViewModel> diaryDetailvmFactory)
        {
            _serviceProvider = serviceProvider;
            _englishDataCache = englishDataCache;
            _englishService = englishService;
            _userProvider = userProvider;

            _diaryDetailvmFactory = diaryDetailvmFactory;

            #region Commands
            AddDiaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (AuthenticationCache.CurrentUser == null)
                {
                    // Show a message box or notification to inform the user to log in
                    MessageBox.Show("Vui lòng đăng nhập để có thể viết nhật ký mới", "Yêu cầu đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var window = _serviceProvider.GetRequiredService<AddDiaryWindow>();
                window.ShowDialog();

                PublicDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.Mode == "public").ToList());
                PrivateDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.Mode == "private").ToList());
            });

            OpenDiaryDetailCommand = new RelayCommand<object>((p) => p is DiaryContent, (p) =>
            {
                var diary = p as DiaryContent;
                var window = _serviceProvider.GetRequiredService<DiaryDetailWindow>();
                window.DataContext = _diaryDetailvmFactory(_userProvider, diary);
                window.ShowDialog();
            });
            #endregion
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Load any additional data or perform setup tasks here
                _englishDataCache.LoadAllData(_englishService);
                PublicDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.Mode == "public").ToList());
                PrivateDiaries = new ObservableCollection<DiaryContent>(_englishDataCache.AllDiaries.Where(d => d.Mode == "private").ToList());
            }, cancellationToken);
        }
    }
}
