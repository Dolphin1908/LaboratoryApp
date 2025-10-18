using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.English;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Data.Providers.English.DiaryFunction;
using LaboratoryApp.src.Services.Authentication;
using LaboratoryApp.src.Services.Helper.Counter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LaboratoryApp.src.Services.English.DiaryFunction
{
    public class DiaryService : IDiaryService
    {
        private readonly IDiaryProvider _diaryProvider;
        private readonly ICounterService _counterService;
        private readonly IEnglishDataCache _englishDataCache;

        public DiaryService(IDiaryProvider diaryProvider,
                            ICounterService counterService,
                            IEnglishDataCache englishDataCache)
        {
            _diaryProvider = diaryProvider;
            _counterService = counterService;
            _englishDataCache = englishDataCache;
        }

        /// <summary>
        /// Create a new diary entry and save it to the database and cache.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="isPublic"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public async Task CreateDiaryAsync(string title, bool isPublic, FlowDocument document)
        {
            byte[] data = FlowDocumentSerializer.SerializeToBytes(document);

            var entry = new DiaryContent
            {
                Id = _counterService.GetNextId(CollectionName.Diaries),
                Title = title ?? "Untitled",
                IsPublic = isPublic,
                UserId = AuthenticationCache.CurrentUser?.Id ?? 0,
                ContentBytes = data,
                ContentFormat = "XamlPackage",
            };

            await Task.Run(() =>
            {
                _diaryProvider.AddDiary(entry);
                _englishDataCache.AllDiaries.Add(entry);
            });
        }

        /// <summary>
        /// Get all public diary entries from the cache.
        /// </summary>
        /// <returns></returns>
        public async Task<List<DiaryContent>> GetPublicDiariesAsync()
        {
            return await Task.Run(() => _englishDataCache.AllDiaries.Where(d => d.IsPublic == true).ToList());
        }

        /// <summary>
        /// Get all private diary entries for the current user from the cache.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<DiaryContent>> GetPrivateDiariesForCurrentUserAsync(long userId)
        {
            return await Task.Run(() => _englishDataCache.AllDiaries.Where(d => d.UserId == userId).ToList());
        }

        /// <summary>
        /// Update an existing diary entry and save changes to the database and cache.
        /// </summary>
        /// <param name="originalDiary"></param>
        /// <param name="newTitle"></param>
        /// <param name="newIsPublic"></param>
        /// <param name="newDocument"></param>
        /// <returns></returns>
        public async Task UpdateDiaryAsync(DiaryContent originalDiary, string newTitle, bool newIsPublic, FlowDocument newDocument)
        {
            byte[] data = FlowDocumentSerializer.SerializeToBytes(newDocument);

            originalDiary.Title = newTitle ?? "Untitled";
            originalDiary.IsPublic = newIsPublic;
            originalDiary.ContentBytes = data;
            originalDiary.UpdatedAt = DateTime.UtcNow;

            await Task.Run(() =>
            {
                _diaryProvider.UpdateDiary(originalDiary);

                var index = _englishDataCache.AllDiaries.FindIndex(d => d.Id == originalDiary.Id);
                if (index >= 0)
                    _englishDataCache.AllDiaries[index] = originalDiary;
            });
        }

        /// <summary>
        /// Delete a diary entry from the database and cache.
        /// </summary>
        /// <param name="diary"></param>
        /// <returns></returns>
        public async Task DeleteDiaryAsync(DiaryContent diary)
        {
            await Task.Run(() =>
            {
                _diaryProvider.DeleteDiary(diary.Id);
                var index = _englishDataCache.AllDiaries.FindIndex(d => d.Id == diary.Id);
                if (index >= 0)
                    _englishDataCache.AllDiaries.RemoveAt(index);
            });
        }
    }
}
