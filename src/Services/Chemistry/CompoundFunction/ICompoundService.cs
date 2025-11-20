using LaboratoryApp.Domain.Models.Chemistry.CompoundFunction;

namespace LaboratoryApp.src.Services.Chemistry.CompoundFunction
{
    public interface ICompoundService
    {
        IEnumerable<Compound> GetSuggestions(string searchText, int limit = 10);
        void SaveCompound(Compound compound);
    }
}