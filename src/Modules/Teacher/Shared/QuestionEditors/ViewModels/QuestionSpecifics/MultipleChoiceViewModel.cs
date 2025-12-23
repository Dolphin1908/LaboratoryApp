using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.ViewModels.QuestionSpecifics
{
    public class MultipleChoiceViewModel : BaseSpecificQuestionViewModel
    {
        private ObservableCollection<AnswerOption> _answerOptions;

        #region Commands
        public ICommand AddAnswerOptionCommand { get; set; }
        public ICommand RemoveAnswerOptionCommand { get; set; }
        public ICommand OptionCheckedCommand { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<AnswerOption> AnswerOptions
        {
            get => _answerOptions;
            set
            {
                _answerOptions = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public MultipleChoiceViewModel(Question model) : base(model)
        {
            AnswerOptions = new ObservableCollection<AnswerOption>(Model.AnswerOptions);

            if (AnswerOptions.Count == 0)
            {
                AddAnswerOption("Phương án 1");
                AddAnswerOption("Phương án 2");
            }

            #region Commands
            AddAnswerOptionCommand = new RelayCommand<object>(p => true, p => ExecuteAddAnswerOption());
            RemoveAnswerOptionCommand = new RelayCommand<AnswerOption>(p => true, p => ExecuteRemoveAnswerOption(p));
            OptionCheckedCommand = new RelayCommand<AnswerOption>(p => true, p => ExecuteOptionChecked(p));
            #endregion
        }

        private void AddAnswerOption(string content)
        {
            var newOption = new AnswerOption
            {
                QuestionId = Model.Id,
                Content = content,
                IsCorrect = false,
                OrderIndex = AnswerOptions.Count
            };
            AnswerOptions.Add(newOption);
            Model.AnswerOptions.Add(newOption);
        }

        private void ExecuteAddAnswerOption()
        {
            AddAnswerOption($"Phương án {AnswerOptions.Count + 1}");
        }

        private void ExecuteRemoveAnswerOption(AnswerOption option)
        {
            if (option == null) return;

            AnswerOptions.Remove(option);
            Model.AnswerOptions.Remove(option);

            for (int i = 0; i < AnswerOptions.Count; i++)
            {
                AnswerOptions[i].OrderIndex = i;
            }
        }

        private void ExecuteOptionChecked(AnswerOption selectedOption)
        {
            if (Model.Type == QuestionType.MultipleChoice)
            {
                foreach (var opt in AnswerOptions)
                {
                    if (opt != selectedOption)
                    {
                        opt.IsCorrect = false;
                    }
                }
            }
        }
    }
}
