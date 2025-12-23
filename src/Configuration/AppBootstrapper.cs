using LaboratoryApp.Domain.Interfaces.Providers.Chemistry;
using LaboratoryApp.Domain.Interfaces.Providers.English;
using LaboratoryApp.Domain.Interfaces.Providers.Operations;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.Caches.English;
using Microsoft.Extensions.DependencyInjection;

namespace LaboratoryApp.src.Configuration
{
    public static class AppBootstrapper
    {
        public static async Task InitializeAppDataAsync(IServiceProvider serviceProvider)
        {
            await Task.Run(() =>
            {
                // Gọi providers
                var exerciseSetAccessProvider = serviceProvider.GetRequiredService<IExerciseAccessProvider>();

                var periodicProvider = serviceProvider.GetRequiredService<IPeriodicProvider>();
                var compoundProvider = serviceProvider.GetRequiredService<ICompoundProvider>();
                var reactionProvider = serviceProvider.GetRequiredService<IReactionProvider>();

                var diaryProvider = serviceProvider.GetRequiredService<IDiaryProvider>();
                var dictionaryProvider = serviceProvider.GetRequiredService<IDictionaryProvider>();

                // Gọi caches
                var authorizationCache = serviceProvider.GetRequiredService<IAuthorizationCache>();
                var chemistryCache = serviceProvider.GetRequiredService<IChemistryDataCache>();
                var englishCache = serviceProvider.GetRequiredService<IEnglishDataCache>();

                // Lấy dữ liệu
                authorizationCache.LoadAllData(exerciseSetAccessProvider);
                chemistryCache.LoadAllData(periodicProvider, compoundProvider, reactionProvider);
                englishCache.LoadAllData(diaryProvider, dictionaryProvider);

            });
        }
    }
}
