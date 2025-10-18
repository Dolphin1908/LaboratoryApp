using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Assignment.Enums
{
    /// <summary>
    /// Định nghĩa các loại câu hỏi
    /// </summary>
    public enum QuestionType
    {
        [Display(Name = "Trắc nghiệm")]        
        MultipleChoice,   // bao gồm cả True/False, Matching

        [Display(Name = "Điền vào chỗ trống")] 
        FillInBlank,      // bao gồm Dictation, ShortAnswer

        [Display(Name = "Tự luận")]            
        Writing,          // bao gồm Sentence, Paragraph, Essay

        [Display(Name = "Bài đọc")]            
        Reading,          // bài đọc kèm nhiều câu hỏi

        [Display(Name = "Bài nghe")]           
        Listening         // bài nghe kèm nhiều câu hỏi
    }

    /// <summary>
    /// Định nghĩa các loại bài tập
    /// </summary>
    public enum ExerciseType
    {
        [Display(Name = "Luyện tập")] 
        Practice, // Luyện tập

        [Display(Name = "Bài tập về nhà")] 
        Homework, // Bài tập về nhà

        [Display(Name = "Kiểm tra ngắn")]
        Quiz,     // Kiểm tra ngắn

        [Display(Name = "Kiểm tra")]
        Test,     // Kiểm tra

        [Display(Name = "Thi")] 
        Exam,     // Thi

        [Display(Name = "Ôn tập")] 
        Review    // Ôn tập
    }
}
