using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Taccolo
{
    public class AzureTranslator
    {
        private static string key;
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";

        // location, also known as region.
        // required if you're using a multi-service or regional (not global) resource. It can be found in the Azure portal on the Keys and Endpoint page.
        private static readonly string location = "westeurope";

        public AzureTranslator(IConfiguration configuration)
        {
            key = configuration["AzureTranslator:ApiKey"];
        }
        public async Task<string> AzTranslate(string sourceLanguage, string targetLanguage, string inputText)
        {
            // Input and output languages are defined as parameters.
            string route = $"/translate?api-version=3.0&from={sourceLanguage}&to={targetLanguage}";
            string textToTranslate = inputText;
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                // location required if you're using a multi-service or regional (not global) resource.
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string jsonResult = await response.Content.ReadAsStringAsync();//in JSON format
                
                using JsonDocument doc = JsonDocument.Parse(jsonResult);
                string translatedText = doc.RootElement[0].GetProperty("translations")[0].GetProperty("text").GetString();
                return translatedText;
            }
        }
    }
}
