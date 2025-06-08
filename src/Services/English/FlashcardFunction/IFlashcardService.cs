using LaboratoryApp.src.Core.Models.English.FlashcardFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.English.FlashcardFunction
{
    public interface IFlashcardService
    {
        List<FlashcardSet> GetAllFlashcardSets();
        void AddFlashcardSet(FlashcardSet flashcardSet);
        void UpdateFlashcardSet(FlashcardSet updatedSet);
        void DeleteFlashcardSet(FlashcardSet deletedSet);

        void AddFlashcardToSet(long setId, Flashcard flashcard);
        void UpdateFlashcard(long setId, Flashcard flashcard);
        void DeleteFlashcardFromSet(long setId, Flashcard flashcard);
    }
}
