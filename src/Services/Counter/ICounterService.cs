using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.Helper.Counter
{
    public interface ICounterService
    {
        long GetNextId(string collectionName);
    }
}
