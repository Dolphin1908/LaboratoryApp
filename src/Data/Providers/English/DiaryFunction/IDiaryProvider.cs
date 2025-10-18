using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.English.DiaryFunction
{
    public interface IDiaryProvider
    {
        public void AddDiary(DiaryContent diary);
        public List<DiaryContent> GetAllDiaries();
        public void UpdateDiary(DiaryContent diary);
        public void DeleteDiary(long id);
    }
}
