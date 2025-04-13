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

namespace LaboratoryApp.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiKey = AppConfigHelper.GetKey("GeminiApiKey");

        public AIService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WordResultDTO> SearchWordWithAIAsync(string word)
        {
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            var prompt = @$"
Given the English word: \""{word}\"".

Please return ONLY a single JSON object (no explanation, no code block, no extra text), with this exact structure:

{{
""word"": ""{word}"",
""pronunciation"": "" / IPA / "",
""pos"": [
{{
""pos"": ""Part of speech of the word in Vietnamese"",
""definitions"": [
{{
""definition"": ""Definition of the pos of the word in Vietnamese."",
""examples"": [
{{
""example"": ""Example sentence using the word."",
""translation"": ""Translation of the sentence in Vietnamese.""
}}
]
}}
]
}}
// allow multiple POS if the word has many
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
