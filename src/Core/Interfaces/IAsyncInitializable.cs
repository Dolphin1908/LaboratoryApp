namespace LaboratoryApp.src.Core.Interfaces
{
    public interface IAsyncInitializable
    {
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}
