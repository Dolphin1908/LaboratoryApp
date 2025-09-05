using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Services.English;
using LaboratoryApp.src.Modules.English.DiaryFunction.Views;
using System.Windows;

namespace LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels
{
    public class DiaryDetailViewModel : BaseViewModel
    {
        private readonly IEnglishService _englishService;
        private readonly IUserProvider _userProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly EnglishDataCache _englishDataCache;

        private readonly Func<DiaryContent, DiaryViewModel> _diaryEditvmFactory;

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

        public DiaryDetailViewModel(IUserProvider userProvider,
                                    IServiceProvider serviceProvider,
                                    IEnglishService englishService,
                                    EnglishDataCache englishDataCache,
                                    DiaryContent diaryContent,
                                    Func<DiaryContent, DiaryViewModel> diaryEditVmFactory)
        {
            _userProvider = userProvider;
            _serviceProvider = serviceProvider;
            _englishService = englishService;
            _englishDataCache = englishDataCache;
            _diaryEditvmFactory = diaryEditVmFactory;

            _diaryContent = diaryContent;
            _boundDocument = FlowDocumentSerializer.DeserializeFromBytes(diaryContent.ContentBytes) ?? new FlowDocument();
            _isAuthor = AuthenticationCache.CurrentUser != null && AuthenticationCache.CurrentUser.Id == diaryContent.UserId;
            _author = _userProvider.GetUserById(diaryContent.UserId) ?? new User();

            EditCommand = new RelayCommand<object>((p)=> true, (p) =>
            {
                // Gán ViewModel cho DiaryViewModel
                var vm = _diaryEditvmFactory(_diaryContent);

                vm.IsEdit = true;
                vm.Title = _diaryContent.Title;
                vm.SelectedMode = _diaryContent.Mode;
                vm.BoundDocument = FlowDocumentSerializer.DeserializeFromBytes(_diaryContent.ContentBytes) ?? new FlowDocument();

                var window = new AddDiaryWindow
                {
                    DataContext = vm
                };

                var result = window.ShowDialog();

                if (p is DiaryDetailWindow thisWin && result == true)
                {
                    thisWin.Close();
                }
            });

            DeleteCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (MessageBox.Show($"Bạn có muốn xóa {DiaryContent.Title}?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var index = _englishDataCache.AllDiaries.IndexOf(diaryContent);
                    if (index >= 0)
                    {
                        _englishDataCache.AllDiaries.RemoveAt(index);
                    }
                    _englishService.DeleteDiary(DiaryContent.Id);
                }    
            });
        }
    }
}
