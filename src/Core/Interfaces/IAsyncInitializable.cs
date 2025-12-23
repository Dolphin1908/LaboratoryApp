namespace LaboratoryApp.src.Core.Interfaces.Services
{
    public interface IAsyncInitializable
    {
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}
