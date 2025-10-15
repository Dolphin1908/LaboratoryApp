using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.Helper.Speech
{
    public static class SpeechService
    {
        public static SpeechSynthesizer? synthesizer;

        public static void Setup()
        {
            try
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.SelectVoice("Microsoft Zira Desktop"); // Select a voice
                synthesizer.Volume = 100; // Set volume (0-100)
                synthesizer.Rate = 0; // Set rate (-10 to 10)
            }
            catch (Exception ex)
            {
                synthesizer = null;
                throw new Exception("Lỗi khi khởi tạo SpeechSynthesizer: " + ex.Message);
            }
        }
    }
}
