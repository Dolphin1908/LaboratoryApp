using System;
using System.Net.Http;
using System.Text;
using System.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction.DTOs;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using System.Windows.Documents;
using LaboratoryApp.src.Core.Models.English.DiaryFunction.DTOs;

namespace LaboratoryApp.src.Services.AI
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _decrypted;
        private readonly string _endpoint;

        public AIService()
        {
            _httpClient = new HttpClient();

            var encryptedApiKey = ConfigurationManager.AppSettings["GeminiApiKey"];
            _decrypted = !string.IsNullOrEmpty(encryptedApiKey)
                ? SecureConfigHelper.Decrypt(encryptedApiKey)
                : string.Empty;

            _endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_decrypted}";
        }

        public async Task<WordResultDTO> SearchWordWithAIAsync(string word)
        {
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

            var jsonBody = JsonConvert.SerializeObject(requestBody); // Chuyển đổi đối tượng thành chuỗi JSON
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"); // Chuyển đổi đối tượng thành chuỗi JSON và tạo nội dung yêu cầu

            var response = await _httpClient.PostAsync(_endpoint, content); // Gửi yêu cầu POST đến API
            if (!response.IsSuccessStatusCode) return null; // Kiểm tra mã trạng thái phản hồi

            var responseContent = await response.Content.ReadAsStringAsync(); // Đọc nội dung phản hồi

            var jsonObject = JObject.Parse(responseContent); // Chuyển đổi chuỗi JSON thành JObject
            var jsonString = jsonObject["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString(); // Lấy giá trị của trường "text" trong JSON

            if (string.IsNullOrEmpty(jsonString)) return null; // Kiểm tra nếu jsonString là null hoặc rỗng

            // Xử lý nếu bị bọc trong ```json hoặc ```
            if (jsonString.Trim().StartsWith("```"))
            {
                jsonString = jsonString.Replace("```json", "")
                                       .Replace("```", "")
                                       .Trim();
            }

            try
            {
                var wordResult = JsonConvert.DeserializeObject<WordResultDTO>(jsonString); // Chuyển đổi chuỗi JSON thành đối tượng WordResultDTO
                return wordResult;
            }
            catch (JsonException ex)
            {
                return null;
            }
        }

        public async Task<DiaryResultDTO> EditDiaryWithAIAsync(string title, string body)
        {
            var prompt = @$"
You are a diary assistant.  
The user will provide a diary entry with a title and a body.  

Your job:
- Correct grammar and spelling.
- Keep the style natural and personal.
- Do not change the meaning or emotions.

Return ONLY a single JSON object (no explanation, no code block, no extra text), with this exact structure:

{{
  ""title"": ""Corrected title here"",
  ""content"": ""Corrected body here""
}}

Requirements:
- Output must be raw JSON only.
- No markdown, no explanation, no extra formatting.

User entry:
{{
""title"": ""{title}"",
""content"": ""{body}"",
}}
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

            var jsonBody = JsonConvert.SerializeObject(requestBody); // Chuyển đổi đối tượng thành chuỗi JSON
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"); // Chuyển đổi đối tượng thành chuỗi JSON và tạo nội dung yêu cầu

            var response = await _httpClient.PostAsync(_endpoint, content); // Gửi yêu cầu POST đến API
            if (!response.IsSuccessStatusCode) return null; // Kiểm tra mã trạng thái phản hồi

            var responseContent = await response.Content.ReadAsStringAsync(); // Đọc nội dung phản hồi

            var jsonObject = JObject.Parse(responseContent); // Chuyển đổi chuỗi JSON thành JObject
            var jsonString = jsonObject["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString(); // Lấy giá trị của trường "text" trong JSON

            if (string.IsNullOrEmpty(jsonString)) return null; // Kiểm tra nếu jsonString là null hoặc rỗng

            // Xử lý nếu bị bọc trong ```json hoặc ```
            if (jsonString.Trim().StartsWith("```"))
            {
                jsonString = jsonString.Replace("```json", "")
                                       .Replace("```", "")
                                       .Trim();
            }

            try
            {
                var result = JsonConvert.DeserializeObject<DiaryResultDTO>(jsonString); // Chuyển đổi chuỗi JSON thành đối tượng DiaryContent
                return result;
            }
            catch (JsonException ex)
            {
                return null;
            }
        }
    }
}