using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using LaboratoryApp.Services;
using LaboratoryApp.Models.English;

namespace LaboratoryApp.ViewModels.English.Flashcard.SubWin
{
    public class FlashcardStudyViewModel : BaseViewModel
    {
        // Các thuộc tính
        private bool _isFrontVisible = true;
        private ScaleTransform _flipTransform;
        private FlashcardSet _flashcardSet;
        private int _currentCardIndex = 1;
        private List<FlashcardModel> _flashcards;
        private FlashcardModel _currentFlashcard;
        private FlashcardService _flashcardService;


        // Property cho FlipTransform để binding trong XAML
        public ScaleTransform FlipTransform => _flipTransform;

        #region Commands
        public ICommand MarkAsLearnedCommand { get; set; }
        public ICommand MarkAsNotLearnedCommand { get; set; }
        public ICommand FlipCardCommand { get; set; }
        #endregion

        #region Properties
        public bool IsFrontVisible
        {
            get => _isFrontVisible;
            set
            {
                _isFrontVisible = value;
                OnPropertyChanged(nameof(IsFrontVisible));
            }
        }

        public List<FlashcardModel> Flashcards
        {
            get => _flashcards;
            set
            {
                _flashcards = value;
                OnPropertyChanged(nameof(Flashcards));
            }
        }

        public FlashcardModel CurrentFlashcard
        {
            get => _currentFlashcard;
            set
            {
                _currentFlashcard = value;
                OnPropertyChanged(nameof(CurrentFlashcard));
            }
        }

        public int CurrentCardIndex
        {
            get => _currentCardIndex;
            set
            {
                _currentCardIndex = value;
                OnPropertyChanged(nameof(CurrentCardIndex));
            }
        }
        #endregion

        public FlashcardStudyViewModel(FlashcardSet flashcardSet, FlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
            _flashcardSet = flashcardSet;
            Flashcards = _flashcardSet.Flashcards.OrderBy(i => Guid.NewGuid()).ToList();

            CurrentFlashcard = Flashcards[CurrentCardIndex - 1];

            // Khởi tạo command
            MarkAsLearnedCommand = new RelayCommand<object>((p) => true, (p) => MarkAsLearned(p));
            MarkAsNotLearnedCommand = new RelayCommand<object>((p) => true, (p) => MarkAsNotLearned(p));
            FlipCardCommand = new RelayCommand<object>((p)=>true,(p)=>FlipCard());

            // Khởi tạo ScaleTransform để lật thẻ
            _flipTransform = new ScaleTransform();
        }

        private void MarkAsLearned(object window)
        {
            // Cập nhật thông tin thẻ
            CurrentFlashcard.ReviewCount++;
            CurrentFlashcard.LastReviewed = DateTime.Now;
            CurrentFlashcard.NextReview = CalculateNextReview(true);
            CurrentFlashcard.IsLearned = true;
            CurrentFlashcard.CorrectStreak++;
            // Lưu lại thông tin
            Flashcards[CurrentCardIndex - 1] = CurrentFlashcard;

            MoveToNextCard(window);
        }

        private void MarkAsNotLearned(object window)
        {
            // Cập nhật thông tin thẻ
            CurrentFlashcard.ReviewCount++;
            CurrentFlashcard.LastReviewed = DateTime.Now;
            CurrentFlashcard.NextReview = CalculateNextReview(false);
            CurrentFlashcard.IsLearned = false;
            CurrentFlashcard.CorrectStreak = 0;

            // Lưu lại thông tin
            Flashcards[CurrentCardIndex - 1] = CurrentFlashcard;

            MoveToNextCard(window);
        }

        private void MoveToNextCard(object window)
        {
            if (CurrentCardIndex < Flashcards.Count)
            {
                CurrentCardIndex++;
            }
            else
            {
                foreach (var flashcard in Flashcards)
                {
                    var temp = _flashcardSet.Flashcards.FirstOrDefault(f => f.Id == flashcard.Id);
                    var index = _flashcardSet.Flashcards.IndexOf(temp);
                    if (index != -1)
                    {
                        _flashcardSet.Flashcards[index] = flashcard;
                    }
                }

                _flashcardService.UpdateFlashcardSet(_flashcardSet);
                
                if(window is Window win)
                {
                    win.Close();
                }
                return;
            }    

            CurrentFlashcard = Flashcards[CurrentCardIndex - 1];

            ResetCardState();
        }

        private void SaveFlashcardSet()
        {
            _flashcardService.UpdateFlashcardSet(_flashcardSet);
        }

        private void ResetCardState()
        {
            IsFrontVisible = true;
            _flipTransform.ScaleX = 1;
        }

        private void FlipCard()
        {
            var flipOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseIn }
            };

            flipOut.Completed += (s, e) =>
            {
                // Đổi mặt khi thẻ "biến mất"
                IsFrontVisible = !IsFrontVisible;

                var flipIn = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut }
                };

                _flipTransform.BeginAnimation(ScaleTransform.ScaleXProperty, flipIn);
            };

            _flipTransform.BeginAnimation(ScaleTransform.ScaleXProperty, flipOut);
        }

        private DateTime CalculateNextReview(bool isCorrect)
        {
            if (isCorrect)
            {
                return DateTime.Now.AddDays(CurrentFlashcard.CorrectStreak switch
                {
                    0 => 1,
                    1 => 3,
                    _ => (int)(CurrentFlashcard.CorrectStreak * 2.5)
                });
            }
            return DateTime.Now.AddDays(1);
        }
    }
}
