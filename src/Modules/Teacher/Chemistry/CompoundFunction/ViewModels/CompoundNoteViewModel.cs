using LaboratoryApp.Domain.Enums.Chemistry.CompoundFunction;
using LaboratoryApp.Domain.Helpers;
using LaboratoryApp.src.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels
{
    public class CompoundNoteViewModel : BaseViewModel
    {
        private CompoundNoteType _compoundNoteType;
        private ObservableCollection<NoteLineViewModel> _compoundNotes;

        #region Properties
        public CompoundNoteType CompoundNoteType
        {
            get => _compoundNoteType;
            set
            {
                _compoundNoteType = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<NoteLineViewModel> CompoundNotes
        {
            get => _compoundNotes;
            set
            {
                _compoundNotes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<EnumDisplay<CompoundNoteType>> NoteTypeOptions { get; set; }
        #endregion

        #region Commands
        public ICommand AddNoteCommand { get; set; }
        public ICommand RemoveNoteCommand { get; set; }
        #endregion

        public CompoundNoteViewModel()
        {
            NoteTypeOptions = new ObservableCollection<EnumDisplay<CompoundNoteType>>(
                Enum.GetValues(typeof(CompoundNoteType))
                    .Cast<CompoundNoteType>()
                    .Select(e => new EnumDisplay<CompoundNoteType>(e))
            );

            CompoundNotes = new ObservableCollection<NoteLineViewModel>();

            AddNoteCommand = new RelayCommand<object>(p => true, p =>
            {
                CompoundNotes.Add(new NoteLineViewModel());
            });
            RemoveNoteCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is NoteLineViewModel note)
                    CompoundNotes.Remove(note);
            });
        }
    }
}
