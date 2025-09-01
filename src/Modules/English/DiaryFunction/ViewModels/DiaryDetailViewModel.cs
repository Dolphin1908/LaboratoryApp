using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;

namespace LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels
{
    public class DiaryDetailViewModel : BaseViewModel
    {
        private readonly IUserProvider _userProvider;

        private DiaryContent _diaryContent;
        private FlowDocument _boundDocument;
        private User _author;

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
        #endregion

        public DiaryDetailViewModel(IUserProvider userProvider,
                                    DiaryContent diaryContent)
        {
            _userProvider = userProvider;

            _diaryContent = diaryContent;
            _boundDocument = FlowDocumentSerializer.DeserializeFromBytes(diaryContent.ContentBytes) ?? new FlowDocument();
            _author = _userProvider.GetUserById(diaryContent.UserId) ?? new User();
        }
    }
}
