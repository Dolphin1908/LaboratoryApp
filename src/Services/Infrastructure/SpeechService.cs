using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using System.Speech.Synthesis;

namespace LaboratoryApp.src.Services.Infrastructure
{
    public class SpeechService : ISpeechService
    {
        public SpeechSynthesizer? synthesizer;

        public void Setup()
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

        public void Speak(string text)
        {
            if (synthesizer != null && !string.IsNullOrEmpty(text))
            {
                synthesizer.SpeakAsync(text);
            }
        }

        public void Stop()
        {
            if (synthesizer != null)
            {
                synthesizer.SpeakAsyncCancelAll();
            }
        }
    }
}
