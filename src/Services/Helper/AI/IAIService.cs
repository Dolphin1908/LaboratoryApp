using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DiaryFunction.DTOs;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction.DTOs;

namespace LaboratoryApp.src.Services.Helper.AI
{
    public interface IAIService
    {
        Task<WordResultDTO?> SearchWordWithAIAsync(string word);
        Task<DiaryResultDTO?> EditDiaryWithAIAsync(string title, string body);
    }
}
