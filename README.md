# LaboratoryApp
_This application focus on some courses such as: Math, Physics, Chemistry, English,... In the future, it will have so much courses with a lot of features to support all learner or students._

## Features
- **Math**:
	- [ ] Lectures & Theory: Summary of formulas, theorems, lectures by topic.
	- [ ] CAS calculator: Support for calculating equations, derivatives, integrals, matrices, etc.
	- [ ] Exercises & Exercise solutions: Self-practice exercises, multiple choice tests, detailed answer explanations.
	- [ ] Math graphs: Tools for drawing function graphs, spatial geometry.
	- [ ] Quick test: Random test questions by level (easy, medium, difficult).
	- [ ] Solve math problems with images: Enter formulas or take pictures of problems to get solutions.

- **Physics**:
	- [ ] Theory & formulas: List of important formulas, lectures by topic.
	- [ ] Experimental simulation: Simulate physical phenomena such as pendulum, electric current, optics.
	- [ ] Exercises & tests: System of self-practice exercises, with solutions.
	- [ ] Physical calculator: Quickly calculate physical formulas (Ohm's law, kinetic energy, potential energy, ...).
	- [ ] Graph simulation: Draw graphs of motion, waves, alternating current.
	- [ ] Quick test: Test system with timer.

- **Chemistry**:
	- [x] Interactive periodic table: Displays detailed information about each element.
	- [ ] Chemical formulas & reactions: Look up chemical reaction equations, balance equations.
	- [ ] Exercises & tests: Practice theoretical exercises, chemistry problems.
	- [ ] Experimental simulation: Practice virtual experiments such as acid-base reactions, electrolysis.
	- [ ] Chemical calculator: Calculate molar mass, solution concentration, reaction rate.
	- [ ] Quick test: Test system by chapter.

- **English**:
	- [x] English-Vietnamese dictionary: Look up meanings, transcriptions, usage examples.
	- [x] Learn vocabulary: Flashcards, word matching exercises, review vocabulary by topic.
	- [ ] English grammar: Detailed grammar explanations, practice exercises.
	- [ ] Listening practice: Listening by level, keyword recognition.
	- [ ] Pronunciation Practice: Voice Recognition, Pronunciation Score.
	- [ ] Exercises & Quizzes: Test reading comprehension, writing, and fill-in-the-blank skills.
	- [ ] English Communication: Learn common communication sentences, practice conversations.

- **Common**:
	- [x] Calculator: Used for calculations

## How to run
1. Clone the repository:
```git clone https://github.com/Dolphin1908/LaboratoryApp.git```
2. Go to the path LaboratoryApp/Support/Helpers
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