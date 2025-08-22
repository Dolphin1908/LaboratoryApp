using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Shared.Interface
{
    public interface IAsyncInitializable
    {
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}
