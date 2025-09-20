using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Modules.English.ExerciseFunction.ViewModels
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
