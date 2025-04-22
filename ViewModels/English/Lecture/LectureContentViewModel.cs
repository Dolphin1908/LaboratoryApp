using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LaboratoryApp.Models.English.Lecture;

namespace LaboratoryApp.ViewModels.English.Lecture
{
    public class LectureContentViewModel : BaseViewModel
    {
        private string _htmlPath;

        #region Commands
        public ICommand ChooseLectureCommand { get; set; }
        #endregion

        #region Properties
        public string HtmlPath
        {
            get => _htmlPath;
            set
            {
                _htmlPath = value;
                OnPropertyChanged(nameof(HtmlPath));
            }
        }
        #endregion

        public LectureContentViewModel()
        {
            HtmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Components", "HTML", "topic_1.html");

            ChooseLectureCommand = new RelayCommand<long>((p) => true, (p) => ChooseLecture(p));
        }

        private void ChooseLecture(long topicId)
        {
            var filename = $"topic_{topicId}.html";

            HtmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Components", "HTML", filename);
        }
    }
}
