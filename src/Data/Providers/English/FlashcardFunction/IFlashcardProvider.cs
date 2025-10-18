using LaboratoryApp.src.Core.Models.English.FlashcardFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.English.FlashcardFunction
{
    public interface IFlashcardProvider
    {
        List<FlashcardSet> Load();
        void Save(IEnumerable<FlashcardSet> flashcardSets);

        //List<FlashcardSet> GetAllFlashcardSets();
        //void AddFlashcardSet(List<FlashcardSet> flashcardSets, FlashcardSet flashcardSet);
        //void UpdateFlashcardSet(List<FlashcardSet> flashcardSets, FlashcardSet updatedSet);
        //void DeleteFlashcardSet(List<FlashcardSet> flashcardSets, FlashcardSet deletedSet);
        //void AddFlashcardToSet(List<FlashcardSet> flashcardSets, long setId, Flashcard flashcard);
        //void UpdateFlashcard(List<FlashcardSet> flashcardSets, long setId, Flashcard flashcard);
        //void DeleteFlashcardFromSet(List<FlashcardSet> flashcardSets, long setId, Flashcard flashcard);
    }
}
