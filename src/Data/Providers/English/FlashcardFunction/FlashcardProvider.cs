using LaboratoryApp.src.Core.Models.English.FlashcardFunction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.English.FlashcardFunction
{
    public class FlashcardProvider : IFlashcardProvider
    {
        private readonly string _appDataFolder;
        private readonly string _jsonPath;

        public FlashcardProvider()
        {
            _appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LaboratoryApp");
            _jsonPath = Path.Combine(_appDataFolder, "flashcards.json");
        }

        public List<FlashcardSet> Load()
        {
            EnsureDataFileExist();
            var json = File.ReadAllText(_jsonPath);
            return JsonConvert.DeserializeObject<List<FlashcardSet>>(json) ?? new List<FlashcardSet>();
        }

        public void Save(IEnumerable<FlashcardSet> flashcardSets)
        {
            var json = JsonConvert.SerializeObject(flashcardSets, Formatting.Indented);
            File.WriteAllText(_jsonPath, json);
        }

        private void EnsureDataFileExist()
        {
            if (!Directory.Exists(_appDataFolder))
            {
                Directory.CreateDirectory(_appDataFolder);
            }

            // Nếu chưa tồn tại, copy từ file gốc trong thư mục cài đặt
            if (!File.Exists(_jsonPath))
            {
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                string sourcePath = Path.Combine(installPath, "Database", "flashcards.json");

                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, _jsonPath);
                }
                else
                {
                    // Nếu file gốc cũng không tồn tại, tạo file rỗng
                    File.WriteAllText(_jsonPath, "[]");
                }
            }
        }

        /// <summary>
        /// Commented out old methods for reference.
        /// </summary>
        //public List<FlashcardSet> GetAllFlashcardSets()
        //{
        //    string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LaboratoryApp");

        //    if (!Directory.Exists(appDataFolder))
        //    {
        //        Directory.CreateDirectory(appDataFolder);
        //    }

        //    // Nếu chưa tồn tại, copy từ file gốc trong thư mục cài đặt
        //    if (!File.Exists(_jsonPath))
        //    {
        //        string installPath = AppDomain.CurrentDomain.BaseDirectory;
        //        string sourcePath = Path.Combine(installPath, "Database", "flashcards.json");

        //        if (File.Exists(sourcePath))
        //        {
        //            File.Copy(sourcePath, _jsonPath);
        //        }
        //        else
        //        {
        //            // Nếu file gốc cũng không tồn tại, tạo file rỗng
        //            File.WriteAllText(_jsonPath, "[]");
        //        }
        //    }

        //    var json = File.ReadAllText(_jsonPath); // Đọc nội dung file JSON

        //    return JsonConvert.DeserializeObject<List<FlashcardSet>>(json) ?? new List<FlashcardSet>();
        //}

        ///// <summary>
        ///// Save the flashcard sets to the JSON file.
        ///// </summary>
        //private void SaveData(List<FlashcardSet> flashcardSets)
        //{
        //    var json = JsonConvert.SerializeObject(flashcardSets, Formatting.Indented);
        //    File.WriteAllText(_jsonPath, json);
        //}

        ///// <summary>
        ///// Add new flashcard set
        ///// </summary>
        ///// <param name="flashcardSet">Information of new set</param>
        //public void AddFlashcardSet(List<FlashcardSet> flashcardSets, FlashcardSet flashcardSet)
        //{
        //    flashcardSet.Id = GenerateNewSetId(flashcardSets);
        //    flashcardSets.Add(flashcardSet);
        //    SaveData(flashcardSets);
        //}

        ///// <summary>
        ///// Update a flashcard set
        ///// </summary>
        ///// <param name="updatedSet">New information of set</param>
        //public void UpdateFlashcardSet(List<FlashcardSet> flashcardSets, FlashcardSet updatedSet)
        //{
        //    var index = flashcardSets.FindIndex(s => s.Id == updatedSet.Id);
        //    if (index != -1)
        //    {
        //        flashcardSets[index] = updatedSet;
        //        SaveData(flashcardSets);
        //    }
        //}

        ///// <summary>
        ///// Delete a flashcard set by its ID.
        ///// </summary>
        ///// <param name="setId">ID of flashcard set</param>
        //public void DeleteFlashcardSet(List<FlashcardSet> flashcardSets, FlashcardSet deletedSet)
        //{
        //    var index = flashcardSets.FindIndex(s => s.Id == deletedSet.Id);
        //    if (index != -1)
        //    {
        //        flashcardSets.RemoveAt(index);
        //        SaveData(flashcardSets);
        //    }
        //}

        ///// <summary>
        ///// Add a new flashcard to a specific set.
        ///// </summary>
        ///// <param name="setId">ID of flashcard set</param>
        ///// <param name="flashcard">Data of new flashcard</param>
        //public void AddFlashcardToSet(List<FlashcardSet> flashcardSets, long setId, Flashcard flashcard)
        //{
        //    var set = flashcardSets.FirstOrDefault(s => s.Id == setId);
        //    if (set != null)
        //    {
        //        flashcard.Id = set.Flashcards.Count == 0 ? 1 : set.Flashcards.Max(f => f.Id) + 1; // Generate new ID for the flashcard
        //        set.LastUpdatedDate = DateTime.Now;
        //        set.Flashcards.Add(flashcard);
        //        SaveData(flashcardSets);
        //    }
        //}

        ///// <summary>
        ///// Update a flashcard in a specific set.
        ///// </summary>
        ///// <param name="flashcardSets"></param>
        ///// <param name="setId"></param>
        ///// <param name="flashcard"></param>
        //public void UpdateFlashcard(List<FlashcardSet> flashcardSets, long setId, Flashcard flashcard)
        //{
        //    var set = flashcardSets.FirstOrDefault(s => s.Id == setId);
        //    if (set != null)
        //    {
        //        var index = set.Flashcards.IndexOf(flashcard);
        //        if (index != -1)
        //        {
        //            set.Flashcards[index] = flashcard;
        //            set.LastUpdatedDate = DateTime.Now;
        //            SaveData(flashcardSets);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Delete a flashcard from a specific set.
        ///// </summary>
        ///// <param name="flashcardSets"></param>
        ///// <param name="setId"></param>
        ///// <param name="flashcard"></param>
        //public void DeleteFlashcardFromSet(List<FlashcardSet> flashcardSets, long setId, Flashcard flashcard)
        //{
        //    var set = flashcardSets.FirstOrDefault(s => s.Id == setId);
        //    if (set != null)
        //    {
        //        set.LastUpdatedDate = DateTime.Now;
        //        set.Flashcards.Remove(flashcard);
        //        SaveData(flashcardSets);
        //    }
        //}

        ///// <summary>
        ///// Generate new ID for the new flashcard set.
        ///// </summary>
        ///// <returns></returns>
        //private long GenerateNewSetId(List<FlashcardSet> flashcardSets)
        //{
        //    return flashcardSets.Count == 0 ? 1 : flashcardSets.Max(s => s.Id) + 1;
        //}
    }
}
