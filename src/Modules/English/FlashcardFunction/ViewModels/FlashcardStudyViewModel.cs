using LaboratoryApp.src.Core.Models.English.FlashcardFunction;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Data.Providers.English.FlashcardFunction;
using LaboratoryApp.src.Services.English.FlashcardFunction;
using LaboratoryApp.src.Shared.Converters;
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

namespace LaboratoryApp.src.Modules.English.FlashcardFunction.ViewModels
{
    public class FlashcardStudyViewModel : BaseViewModel
    {
        private readonly IFlashcardService _flashcardService;

        private readonly List<Flashcard> _studySessionCards;
        private readonly FlashcardSet _flashcardSet;

        private bool _isFrontVisible = true;
        private Flashcard _currentFlashcard;
        private int _currentCardIndex = 1;

        // Các thuộc tính
        private ScaleTransform _flipTransform;

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
        public Flashcard CurrentFlashcard
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
        public int TotalCardCount => _studySessionCards.Count;
        #endregion

        public FlashcardStudyViewModel(FlashcardSet flashcardSet, IFlashcardService flashcardService)
        {
            _flashcardSet = flashcardSet;
            _flashcardService = flashcardService;

            _flipTransform = new ScaleTransform();

            _studySessionCards = _flashcardSet.Flashcards.Where(f => f.NextReview <= DateTime.Now)
                                                         .OrderBy(i => Guid.NewGuid())
                                                         .ToList();

            if (!_studySessionCards.Any())
            {
                _studySessionCards = _flashcardSet.Flashcards.OrderBy(i => Guid.NewGuid())
                                                             .ToList();
            }

            _currentFlashcard = _studySessionCards.First();

            // Khởi tạo command
            MarkAsLearnedCommand = new RelayCommand<object>((p) => true, (p) => ProcessCard(p, true));
            MarkAsNotLearnedCommand = new RelayCommand<object>((p) => true, (p) => ProcessCard(p, false));
            FlipCardCommand = new RelayCommand<object>((p) => true, (p) => FlipCard());
        }

        private void ProcessCard(object window, bool isCorrect)
        {
            _flashcardService.RecordStudyResult(_flashcardSet.Id, CurrentFlashcard, isCorrect);

            MoveToNextCard(window);
        }

        private void MoveToNextCard(object parameter)
        {
            if (_currentCardIndex < _studySessionCards.Count)
            {
                _currentCardIndex++;
                CurrentFlashcard = _studySessionCards[_currentCardIndex - 1];
                ResetCardState();
                OnPropertyChanged(nameof(CurrentCardIndex));
            }
            else
            {
                MessageBox.Show("Bạn đã hoàn thành phiên ôn tập!", "Thông báo", MessageBoxButton.OK);
                if (parameter is Window win)
                {
                    win.Close();
                }
                return;
            }    
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
    }
}
