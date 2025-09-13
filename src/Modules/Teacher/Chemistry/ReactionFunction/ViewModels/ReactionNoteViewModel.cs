using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;

namespace LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionNoteViewModel : BaseViewModel
    {
        private ReactionNoteType _reactionNoteType;
        private ObservableCollection<NoteLineViewModel> _reactionNotes;

        #region Properties
        public ReactionNoteType ReactionNoteType
        {
            get => _reactionNoteType;
            set
            {
                _reactionNoteType = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<NoteLineViewModel> ReactionNotes
        {
            get => _reactionNotes;
            set
            {
                _reactionNotes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<EnumDisplay<ReactionNoteType>> NoteTypeOptions { get; set; }
        #endregion

        #region Commands
        public ICommand AddNoteCommand { get; set; }
        public ICommand RemoveNoteCommand { get; set; }
        #endregion

        public ReactionNoteViewModel()
        {
            NoteTypeOptions = new ObservableCollection<EnumDisplay<ReactionNoteType>>(
                Enum.GetValues(typeof(ReactionNoteType))
                    .Cast<ReactionNoteType>()
                    .Select(e => new EnumDisplay<ReactionNoteType>(e))
            );

            ReactionNotes = new ObservableCollection<NoteLineViewModel>();

            AddNoteCommand = new RelayCommand<object>(p => true, p =>
            {
                ReactionNotes.Add(new NoteLineViewModel());
            });
            RemoveNoteCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is NoteLineViewModel note)
                    ReactionNotes.Remove(note);
            });
        }
    }
}
