using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.English.DictionaryFunction;

namespace LaboratoryApp.src.Services.English
{
    public interface IEnglishService
    {
        public List<Word> GetAllWords();
        public List<Pos> GetAllPos();
        public List<Example> GetAllExamples();
        public List<Definition> GetAllDefinitions();
    }
}
