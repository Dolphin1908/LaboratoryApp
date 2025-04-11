using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English
{
    public class FlashcardSet : INotifyPropertyChanged
    {
        public long id { get; set; }

        private string _name;
        public string name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(name));
                }
            }
        }

        private string _description;
        public string description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(description));
                }
            }
        }

        private long _count;
        // Số lượng flashcard trong bộ thẻ
        public long count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChanged(nameof(count));
                }
            }
        } 
        public DateTime createdDate { get; set; }
        private DateTime _lastUpdatedDate;
        public DateTime lastUpdatedDate
        {
            get => _lastUpdatedDate;
            set
            {
                if (_lastUpdatedDate != null)
                {
                    _lastUpdatedDate = value;
                    OnPropertyChanged(nameof(lastUpdatedDate));
                }    
            }
        }

        public ObservableCollection<FlashcardModel> flashcards { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
