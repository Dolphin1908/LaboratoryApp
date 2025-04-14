using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using LaboratoryApp.Models.DTOs;
using LaboratoryApp.Support.Helpers;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Numerics;
using System.Transactions;
using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Xml;

using LaboratoryApp.Support.Helpers;
using System.Windows;
using System.Configuration;

namespace LaboratoryApp.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];

        public AIService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WordResultDTO> SearchWordWithAIAsync(string word)
        {
            var decrypted = SecureConfigHelper.Decrypt(_apiKey);

            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={decrypted}";

            var prompt = @$"
Given the English word: \""{word}\"".

Please return ONLY a single JSON object (no explanation, no code block, no extra text), with this exact structure:

{{
""word"": ""{word}"",
""pronunciation"": "" / IPA / "",
""pos"": [
{{
""pos"": ""Part of speech in Vietnamese"",
""definitions"": [
{{
""definition"": ""Vietnamese definition of the word in this part of speech."",
""examples"": [
{{
""example"": ""English example sentence."",
""translation"": ""Vietnamese translation of the sentence.""
}},
...
]
}},
...
]
}},
...
]
}}

Requirements:
- Include **all parts of speech** if the word has multiple.
- For **each part of speech**, list **all meanings** (definitions).
- For **each meaning**, list **multiple example sentences** (if available).
- Output must be **raw JSON only**. No code block, no markdown, no explanation.
- Replace all placeholder text with real data.

Example output (for reference only, do not include in actual response):
{{
  ""word"": ""run"",
  ""pronunciation"": ""/rʌn/"",
  ""pos"": [
    {{
      ""pos"": ""động từ"",
      ""definitions"": [
        {{
          ""definition"": ""chạy"",
          ""examples"": [
            {{
              ""example"": ""I run every morning."",
              ""translation"": ""Tôi chạy mỗi sáng.""
            }},
            {{
              ""example"": ""She runs faster than me."",
              ""translation"": ""Cô ấy chạy nhanh hơn tôi.""
            }}
          ]
        }}
      ]
    }},
    {{
      ""pos"": ""danh từ"",
      ""definitions"": [
        {{
          ""definition"": ""sự chạy"",
          ""examples"": [
            {{
              ""example"": ""He went for a run in the park."",
              ""translation"": ""Anh ấy đi chạy bộ trong công viên.""
            }}
          ]
        }}
      ]
    }}
  ]
}}

Please replace the values accordingly and do not include any explanation or code block formatting (like ```json). Output must be raw JSON only.
";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();

            var jsonObject = JObject.Parse(responseContent);
            var jsonString = jsonObject["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

            if (string.IsNullOrEmpty(jsonString)) return null;

            // Xử lý nếu bị bọc trong ```json hoặc ```
            if (jsonString.Trim().StartsWith("```"))
            {
                jsonString = jsonString.Replace("```json", "")
                                       .Replace("```", "")
                                       .Trim();
            }

            try
            {
                var wordResult = JsonConvert.DeserializeObject<WordResultDTO>(jsonString);
                return wordResult;
            }
            catch (JsonException ex)
            {
                return null;
            }
        }
    }
}
