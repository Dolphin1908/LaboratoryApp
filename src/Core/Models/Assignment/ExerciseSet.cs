using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Assignment.Enums;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;

namespace LaboratoryApp.src.Core.Models.Assignment
{
    public class ExerciseSet
    {
        #region Infomation
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long OwnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        #endregion

        #region Settings
        public string Code { get; set; } = string.Empty; // Unique code for sharing
        public string? Password { get; set; } // Optional password for access
        public bool IsPublic { get; set; } = false; // Public or Private
        #endregion

        #region Content
        public List<long> ExerciseIds { get; set; }
        #endregion

        #region NotMapped
        [NotMapped]
        public UserDTO OwnerInfo { get; set; } = new UserDTO(); // Owner information

        [NotMapped]
        public int Count => ExerciseIds.Count; // Total number of exercises
        #endregion
    }
}