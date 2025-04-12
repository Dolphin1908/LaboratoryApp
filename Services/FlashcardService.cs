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
            flashcardSet.Id = GenerateNewSetId();
            _flashcardSets.Add(flashcardSet);
            SaveData();
        }

        /// <summary>
        /// Update a flashcard set
        /// </summary>
        /// <param name="updatedSet">New information of set</param>
        public void UpdateFlashcardSet(FlashcardSet updatedSet)
        {
            var index = _flashcardSets.FindIndex(s => s.Id == updatedSet.Id);
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
            var index = _flashcardSets.FindIndex(s => s.Id == deletedSet.Id);
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
            var set = _flashcardSets.FirstOrDefault(s => s.Id == setId);
            if (set != null)
            {
                flashcard.Id = set.Flashcards.Count == 0 ? 1 : set.Flashcards.Max(f => f.Id) + 1; // Generate new ID for the flashcard
                set.LastUpdatedDate = DateTime.Now;
                set.Flashcards.Add(flashcard);
                SaveData();
            }
        }

        public void UpdateFlashcard(long setId, FlashcardModel flashcard)
        {
            var set = _flashcardSets.FirstOrDefault(s => s.Id == setId);
            if (set != null)
            {
                var index = set.Flashcards.IndexOf(flashcard);
                if (index != -1)
                {
                    set.Flashcards[index] = flashcard;
                    set.LastUpdatedDate = DateTime.Now;
                    SaveData();
                }
            }
        }

        public void DeleteFlashcardFromSet(long setId, FlashcardModel flashcard)
        {
            var set = _flashcardSets.FirstOrDefault(s => s.Id == setId);
            if (set != null)
            {
                set.LastUpdatedDate = DateTime.Now;
                set.Flashcards.Remove(flashcard);
                SaveData();
            }
        }

        /// <summary>
        /// Generate new ID for the new flashcard set.
        /// </summary>
        /// <returns></returns>
        private long GenerateNewSetId()
        {
            return _flashcardSets.Count == 0 ? 1 : _flashcardSets.Max(s => s.Id) + 1;
        }
    }
}
