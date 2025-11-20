using LaboratoryApp.Domain.Models.Chemistry.CompoundFunction;

namespace LaboratoryApp.src.Data.Providers.Chemistry.CompoundFunction
{
    public interface ICompoundProvider
    {
        Task<List<Compound>> GetAllCompoundsAsync();
        Task AddCompoundAsync(Compound compound);
        Task UpdateCompoundAsync(Compound compound);
        Task DeleteCompoundAsync(Compound compound);
    }
}
