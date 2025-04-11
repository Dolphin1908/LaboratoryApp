using LaboratoryApp.Models.English;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Services
{
    public class FlashcardService
    {
        private List<FlashcardSet> _flashcardSets; // List of flashcard sets
        private readonly string _jsonPath = ConfigurationManager.AppSettings["FlashcardJsonPath"]; // Path to the JSON file

        public FlashcardService()
        {
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(_jsonPath))
            {
                var json = File.ReadAllText(_jsonPath);
                _flashcardSets = JsonConvert.DeserializeObject<List<FlashcardSet>>(json) ?? new List<FlashcardSet>();
            }
            else
            {
                _flashcardSets = new List<FlashcardSet>();
            }
        }

        /// <summary>
        /// Save the flashcard sets to the JSON file.
        /// </summary>
        private void SaveData()
        {
            var json = JsonConvert.SerializeObject(_flashcardSets, Formatting.Indented);
            File.WriteAllText(_jsonPath, json);
        }

        /// <summary>
        /// Get all flashcard sets from the JSON file.
        /// </summary>
        /// <returns></returns>
        public List<FlashcardSet> GetAllFlashcardSets()
        {
            return _flashcardSets;
        }

        /// <summary>
        /// Add new flashcard set
        /// </summary>
        /// <param name="flashcardSet">Information of new set</param>
        public void AddFlashcardSet(FlashcardSet flashcardSet)
        {
            flashcardSet.id = GenerateNewSetId();
            _flashcardSets.Add(flashcardSet);
            SaveData();
        }

        /// <summary>
        /// Update a flashcard set
        /// </summary>
        /// <param name="updatedSet">New information of set</param>
        public void UpdateFlashcardSet(FlashcardSet updatedSet)
        {
            var index = _flashcardSets.FindIndex(s => s.id == updatedSet.id);
            if (index != -1)
            {
                _flashcardSets[index] = updatedSet;
                SaveData();
            }
        }

        /// <summary>
        /// Delete a flashcard set by its ID.
        /// </summary>
        /// <param name="setId">ID of flashcard set</param>
        public void DeleteFlashcardSet(FlashcardSet deletedSet)
        {
            var index = _flashcardSets.FindIndex(s => s.id == deletedSet.id);
            if (index != null)
            {
                _flashcardSets.RemoveAt(index);
                SaveData();
            }
        }

        /// <summary>
        /// Add a new flashcard to a specific set.
        /// </summary>
        /// <param name="setId">ID of flashcard set</param>
        /// <param name="flashcard">Data of new flashcard</param>
        public void AddFlashcardToSet(long setId, FlashcardModel flashcard)
        {
            var set = _flashcardSets.FirstOrDefault(s => s.id == setId);
            if (set != null)
            {
                set.count += 1; // Increase the count of flashcards in the set
                flashcard.id = set.flashcards.Count == 0 ? 1 : set.flashcards.Max(f => f.id) + 1; // Generate new ID for the flashcard
                set.lastUpdatedDate = DateTime.Now;
                set.flashcards.Add(flashcard);
                SaveData();
            }
        }

        public void UpdateFlashcard(long setId, FlashcardModel flashcard)
        {
            var set = _flashcardSets.FirstOrDefault(s => s.id == setId);
            if (set != null)
            {
                var index = set.flashcards.IndexOf(flashcard);
                if (index != -1)
                {
                    set.flashcards[index] = flashcard;
                    set.lastUpdatedDate = DateTime.Now;
                    SaveData();
                }
            }
        }

        public void DeleteFlashcardFromSet(long setId, FlashcardModel flashcard)
        {
            var set = _flashcardSets.FirstOrDefault(s => s.id == setId);
            if (set != null)
            {
                set.count -= 1; // Decrease the count of flashcards in the set
                set.lastUpdatedDate = DateTime.Now;
                set.flashcards.Remove(flashcard);
                SaveData();
            }
        }

        /// <summary>
        /// Generate new ID for the new flashcard set.
        /// </summary>
        /// <returns></returns>
        private long GenerateNewSetId()
        {
            return _flashcardSets.Count == 0 ? 1 : _flashcardSets.Max(s => s.id) + 1;
        }
    }
}
