using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.English.DictionaryFunction
{
    public interface IDictionaryProvider
    {
        public List<Word> GetAllWords();
        public List<Pos> GetAllPos();
        public List<Example> GetAllExamples();
        public List<Definition> GetAllDefinitions();
    }
}
