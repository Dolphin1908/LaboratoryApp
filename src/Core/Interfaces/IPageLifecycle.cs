namespace LaboratoryApp.src.Core.Interfaces
{
    public interface IPageLifecycle
    {
        Task OnCleanupAsync(); // Gọi khi trang được dọn dẹp
        Task OnSaveAsync(); // Gọi khi trang được lưu
    }
}
