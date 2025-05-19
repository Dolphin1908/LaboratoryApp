using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.English.FlashcardFunction
{
    public class FlashcardSet : INotifyPropertyChanged
    {
        public long Id { get; set; }
        private string _name;
        private string _description;
        private DateTime _createdDate;
        private DateTime _lastUpdatedDate;

        private ObservableCollection<Flashcard> _flashcards;

        [JsonIgnore]
        public int Total => Flashcards?.Count ?? 0;

        [JsonIgnore]
        public int DueCount => Flashcards.Count(f => f.NextReview <= DateTime.Today && !f.IsLearned);


        private void Flashcards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(DueCount));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));


        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                _createdDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime LastUpdatedDate
        {
            get => _lastUpdatedDate;
            set
            {
                _lastUpdatedDate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Flashcard> Flashcards
        {
            get => _flashcards;
            set
            {
                if (_flashcards != null)
                {
                    _flashcards.CollectionChanged -= Flashcards_CollectionChanged;
                }

                _flashcards = value;

                if (_flashcards != null)
                {
                    _flashcards.CollectionChanged += Flashcards_CollectionChanged;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(DueCount));
            }
        }

    }
}
