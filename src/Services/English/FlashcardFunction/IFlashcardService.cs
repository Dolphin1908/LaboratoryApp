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
        // Lấy dữ liệu
        IEnumerable<FlashcardSet> GetAllSets();

        // Thao tác với Bộ thẻ (Set)
        FlashcardSet CreateNewSet(string name, string description);
        void UpdateSet(FlashcardSet updatedSet);
        void DeleteSet(long setId);

        // Thao tác với Thẻ (Card)
        void AddCardToSet(long setId, Flashcard newCard);
        void UpdateCardInSet(long setId, Flashcard updatedCard);
        void DeleteCardFromSet(long setId, long cardId);

        // Logic ôn tập
        void RecordStudyResult(long setId, Flashcard card, bool wasCorrect);
    }
}
