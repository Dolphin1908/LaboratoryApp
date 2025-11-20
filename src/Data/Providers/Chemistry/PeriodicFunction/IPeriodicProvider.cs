using LaboratoryApp.Domain.Models.Chemistry.Common;

namespace LaboratoryApp.src.Data.Providers.Chemistry.PeriodicFunction
{
    public interface IPeriodicProvider
    {
        Task<List<Element>> GetAllElementsAsync();
    }
}
