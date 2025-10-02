using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.Assignment.Exercise.ViewModels
{
    public class ExerciseManagerViewModel : BaseViewModel, IAsyncInitializable
    {
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Khởi tạo dữ liệu bất đồng bộ ở đây
            }, cancellationToken);
        }
    }
}
