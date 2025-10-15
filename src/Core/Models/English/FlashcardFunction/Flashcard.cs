using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.English.FlashcardFunction
{
    public class Flashcard : INotifyPropertyChanged
    {
        // Data
        public long Id { get; set; }
        private string? _word;
        private string? _pos;
        private string? _meaning;
        private string? _example;
        private string? _note;

        // Spaced‑Repetition / Trạng thái học
        private int _reviewCount; // số lần đã học
        private int _correctStreak; // số lần đúng liên tiếp
        private DateTime _lastReviewed; // ngày học gần nhất
        private DateTime _nextReview; // ngày học tiếp theo (học lại)

        // **Mới**: đã hoàn thành (học thuộc)
        private bool _isLearned;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));


        public string Word
        {
            get => _word ?? string.Empty;
            set
            {
                _word = value;
                OnPropertyChanged(nameof(Word));
            }
        }
        public string Pos
        {
            get => _pos ?? string.Empty;
            set
            {
                _pos = value;
                OnPropertyChanged(nameof(Pos));
            }
        }
        public string Meaning
        {
            get => _meaning ?? string.Empty;
            set
            {
                _meaning = value;
                OnPropertyChanged(nameof(Meaning));
            }
        }
        public string Example
        {
            get => _example ?? string.Empty;
            set
            {
                _example = value;
                OnPropertyChanged(nameof(Example));
            }
        }
        public string Note
        {
            get => _note ?? string.Empty;
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
            }
        }
        public int ReviewCount
        {
            get => _reviewCount;
            set
            {
                _reviewCount = value;
                OnPropertyChanged(nameof(ReviewCount));
            }
        }
        public int CorrectStreak
        {
            get => _correctStreak;
            set
            {
                _correctStreak = value;
                OnPropertyChanged(nameof(CorrectStreak));
            }
        }
        public DateTime LastReviewed
        {
            get => _lastReviewed;
            set
            {
                _lastReviewed = value;
                OnPropertyChanged(nameof(LastReviewed));
            }
        }
        public DateTime NextReview
        {
            get => _nextReview;
            set
            {
                _nextReview = value;
                OnPropertyChanged(nameof(NextReview));
            }
        }
        public bool IsLearned
        {
            get => _isLearned;
            set
            {
                _isLearned = value;
                OnPropertyChanged(nameof(IsLearned));
            }
        }
    }
}
