using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English
{
    public class FlashcardModel
    {
        public long id { get; set; }
        public string Word { get; set; } // Từ vựng
        public string Meaning { get; set; } // Nghĩa
        public string Example { get; set; } // Câu ví dụ (nếu có)
        public string Note { get; set; } // Ghi chú cá nhân
    }
}
