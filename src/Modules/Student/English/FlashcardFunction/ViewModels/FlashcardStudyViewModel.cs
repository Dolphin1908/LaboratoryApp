using LaboratoryApp.Domain.Interfaces.Services.English;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.English.FlashcardFunction;
using LaboratoryApp.src.Core.Interfaces;
using LaboratoryApp.src.Core.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LaboratoryApp.src.Modules.Student.English.FlashcardFunction.ViewModels
{
    public class FlashcardStudyViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ISpeechService _speechService;
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

        public FlashcardStudyViewModel(IDialogService dialogService,
                                       ISpeechService speechService,
                                       FlashcardSet flashcardSet,
                                       IFlashcardService flashcardService)
        {
            _dialogService = dialogService;
            _speechService = speechService;
            _flashcardSet = flashcardSet;
            _flashcardService = flashcardService;

            _speechService.Setup();

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
                _dialogService.ShowMessage("Bạn đã hoàn thành phiên ôn tập!", "Thông báo", MessageBoxButton.OK);
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
            _speechService.Speak(CurrentFlashcard.Word);

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
