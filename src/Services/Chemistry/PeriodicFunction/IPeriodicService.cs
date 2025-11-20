using LaboratoryApp.Domain.Models.Chemistry.Common;

namespace LaboratoryApp.src.Services.Chemistry.PeriodicFunction
{
    public interface IPeriodicService
    {
        List<Element> LoadAllElements();
    }
}
