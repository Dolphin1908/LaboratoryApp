# LaboratoryApp
_This application focuses on courses such as Math, Physics, Chemistry, and English. In the future, it will offer many more courses with numerous features to support all learners and students._

## Features
- **Math**:
    - [ ]  **Lectures & Theory:** Summary of formulas, theorems, and lectures by topic.
    - [ ]  **CAS Calculator:** Calculate equations, derivatives, integrals, and matrices.
    - [ ]  **Exercises & Solutions:** Self-practice exercises, multiple choice tests with detailed explanations.
    - [ ]  **Math Graphs:** Tools for drawing function graphs and spatial geometry.
    - [ ]  **Quick Test:** Random questions by difficulty level (easy, medium, difficult).
    - [ ]  **Image Problem Solver:** Enter formulas or take pictures to get solutions.
- **Physics**:
    - [ ]  **Theory & Formulas:** Essential formulas and lectures organized by topic.
    - [ ]  **Experimental Simulation:** Simulate phenomena like pendulums, electric current, and optics.
    - [ ]  **Exercises & Tests:** Self-practice exercises with solutions.
    - [ ]  **Physics Calculator:** Quick calculations for physical formulas (Ohm's law, energy equations, etc.).
    - [ ]  **Graph Simulation:** Visualize motion, waves, and alternating current.
    - [ ]  **Quick Test:** Timed test system.
- **Chemistry**:
    - [x]  **Interactive Periodic Table:** Detailed information about each element.
    - [x]  **Chemical Formulas & Reactions:** Look up and balance chemical equations.
    - [ ]  **Exercises & Tests:** Practice theoretical concepts and solve chemistry problems.
    - [ ]  **Experimental Simulation:** Virtual experiments for acid-base reactions, electrolysis, and more.
    - [ ]  **Chemical Calculator:** Calculate molar mass, solution concentration, and reaction rate.
    - [ ]  **Quick Test:** Chapter-based test system.
- **English**:
    - [x]  **English-Vietnamese Dictionary:** Meanings, pronunciations, and usage examples.
    - [x]  **Vocabulary Learning:** Flashcards, matching exercises, and topic-based review.
    - [x]  **Diary Writing:** Practice daily entries with grammar and vocabulary feedback.
    - [ ]  **Grammar Guide:** Clear explanations with practice exercises.
    - [ ]  **Listening Practice:** Level-based activities with keyword recognition.
    - [ ]  **Pronunciation Practice:** Voice recognition with scoring.
    - [ ]  **Comprehensive Exercises:** Test reading, writing, and fill-in-the-blank skills.
    - [ ]  **Conversation Practice:** Common phrases and interactive dialogues.
- **Common**:
    - [x]  **Calculator:** General calculation tool
- **Teacher**:
    - [x]  **Add New Compounds:** Teacher functionality to expand the chemistry database.
    - [x]  **Add New Reactions:** Teacher functionality to add chemical reactions.

## How to run
1. Clone the repository:
```git clone https://github.com/Dolphin1908/LaboratoryApp.git```
2. Go to the path LaboratoryApp/src/Core/Helpers
3. Add new file named "SecureConfigHelper.cs" and copy the code below:
```csharp
	public static class SecureConfigHelper
    {
        private readonly static string Passphrase = "ToDo";
        private readonly static byte[] Salt = Encoding.UTF8.GetBytes("ToDo"); // 16 bytes

        private static void GenerateKeyAndIV(out byte[] key, out byte[] iv)
        {
            using var rfc = new Rfc2898DeriveBytes(Passphrase, Salt, 100_000, HashAlgorithmName.SHA256);
            key = rfc.GetBytes(16); // AES-128
            iv = rfc.GetBytes(16);  // IV luôn 16 bytes
        }

        public static string Encrypt(string plainText)
        {
            GenerateKeyAndIV(out var key, out var iv);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
                sw.Flush(); // flush StreamWriter
                cs.FlushFinalBlock(); // đảm bảo tất cả đã ghi
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string encryptedBase64)
        {
            GenerateKeyAndIV(out var key, out var iv);
            byte[] cipherBytes = Convert.FromBase64String(encryptedBase64);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
```
4. Replace ToDo with your passphrase and salt in the code above.
5. Go to the path LaboratoryApp/App.config and replace the API key with the encrypted API key.