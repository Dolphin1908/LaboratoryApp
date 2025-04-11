using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English
{
    public class FlashcardModel : INotifyPropertyChanged
    {
        public long id { get; set; }

        private string _word;
        public string Word
        {
            get => _word;
            set
            {
                if (_word != value)
                {
                    _word = value;
                    OnPropertyChanged(nameof(Word));
                }
            }
        }

        private string _pos;
        public string Pos
        {
            get => _pos;
            set
            {
                if (_pos != value)
                {
                    _pos = value;
                    OnPropertyChanged(nameof(Pos));
                }
            }
        }

        private string _meaning;
        public string Meaning
        {
            get => _meaning;
            set
            {
                if (_meaning != value)
                {
                    _meaning = value;
                    OnPropertyChanged(nameof(Meaning));
                }
            }
        }

        private string _example;
        public string Example
        {
            get => _example;
            set
            {
                if (_example != value)
                {
                    _example = value;
                    OnPropertyChanged(nameof(Example));
                }
            }
        }

        private string _note;
        public string Note
        {
            get => _note;
            set
            {
                if (_note != value)
                {
                    _note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
