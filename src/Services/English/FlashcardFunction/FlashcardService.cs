using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Newtonsoft.Json;

using LaboratoryApp.src.Data.Providers.English.FlashcardFunction;
using LaboratoryApp.src.Core.Models.English.FlashcardFunction;

namespace LaboratoryApp.src.Services.English.FlashcardFunction
{
    public class FlashcardService : IFlashcardService
    {
        private readonly IFlashcardProvider _flashcardProvider;

        private List<FlashcardSet> _sets;

        public IEnumerable<FlashcardSet> GetAllSets() => _sets;

        public FlashcardService(IFlashcardProvider flashcardProvider)
        {
            _flashcardProvider = flashcardProvider;
            _sets = _flashcardProvider.Load();
        }

        public FlashcardSet CreateNewSet(string name, string description)
        {
            var newSet = new FlashcardSet
            {
                Id = _sets.Count == 0 ? 1 : _sets.Max(s => s.Id) + 1,
                Name = name,
                Description = description,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                Flashcards = new ObservableCollection<Flashcard>()
            };

            _sets.Add(newSet);
            _flashcardProvider.Save(_sets);

            return newSet;
        }

        public void UpdateSet(FlashcardSet updateSet)
        {
            var setToUpdate = _sets.FirstOrDefault(s => s.Id == updateSet.Id);
            if (setToUpdate != null)
            {
                setToUpdate.Name = updateSet.Name;
                setToUpdate.Description = updateSet.Description;
                setToUpdate.LastUpdatedDate = DateTime.Now;

                _flashcardProvider.Save(_sets);
            }
        }

        public void DeleteSet(long setId)
        {
            var setToDelete = _sets.FirstOrDefault(s=>s.Id == setId);
            if (setToDelete != null)
            {
                _sets.Remove(setToDelete);
                _flashcardProvider.Save(_sets);
            }
        }

        public void AddCardToSet(long setId, Flashcard newCard)
        {
            var set = _sets.FirstOrDefault(s => s.Id == setId);
            if(set!= null)
            {
                newCard.Id = set.Flashcards.Any() ? set.Flashcards.Max(f => f.Id) + 1 : 1;
                newCard.NextReview = DateTime.Now;
                newCard.LastReviewed = DateTime.Now;
                set.Flashcards.Add(newCard);
                set.LastUpdatedDate = DateTime.Now;

                _flashcardProvider.Save(_sets);
            }
        }

        public void UpdateCardInSet (long setId, Flashcard updateCard)
        {
            var set = _sets.FirstOrDefault(s => s.Id == setId);
            if (set!= null)
            {
                var flashcardToUpdate = set.Flashcards.FirstOrDefault(f => f.Id == updateCard.Id);
                if (flashcardToUpdate != null)
                {
                    flashcardToUpdate.Word = updateCard.Word;
                    flashcardToUpdate.Pos = updateCard.Pos;
                    flashcardToUpdate.Meaning = updateCard.Meaning;
                    flashcardToUpdate.Example = updateCard.Example;
                    flashcardToUpdate.Note = updateCard.Note;
                    set.LastUpdatedDate = DateTime.Now;

                    _flashcardProvider.Save(_sets);
                }
            }
        }

        public void DeleteCardFromSet(long setId, long cardId)
        {
            var set = _sets.FirstOrDefault(s=> s.Id == setId);
            if (set!= null)
            {
                var flashcardToDelete = set.Flashcards.FirstOrDefault(f => f.Id == cardId);
                if (flashcardToDelete != null)
                {
                    set.Flashcards.Remove(flashcardToDelete);
                    _flashcardProvider.Save(_sets);
                }
            }
        }

        public void RecordStudyResult(long setId, Flashcard card, bool wasCorrect)
        {
            var set = _sets.FirstOrDefault(s => s.Id == setId);
            var cardToUpdate = set?.Flashcards.FirstOrDefault(f => f.Id == card.Id);
            if (cardToUpdate == null) return;

            cardToUpdate.ReviewCount++;
            cardToUpdate.LastReviewed = DateTime.Now;
            cardToUpdate.IsLearned = wasCorrect;

            if(wasCorrect)
            {
                cardToUpdate.CorrectStreak++;
                cardToUpdate.NextReview = DateTime.Now.AddDays(cardToUpdate.CorrectStreak switch
                {
                    <= 1 => 1,
                    2 => 3,
                    _ => (int)(cardToUpdate.CorrectStreak * 2.1)
                });
            }
            else
            {
                cardToUpdate.CorrectStreak = 0;
                cardToUpdate.NextReview = DateTime.Now.AddMinutes(10);
            }

            _flashcardProvider.Save(_sets);
        }
    }
}
