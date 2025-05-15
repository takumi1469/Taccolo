using System.Diagnostics;
using System.Text;

namespace Taccolo
{
    public class Tokenizer
    {
        private static string apiUrl;
        private readonly IConfiguration _configuration;

        public Tokenizer(IConfiguration configuration)
        {
            apiUrl = configuration["MeCab:URL"];
        }


        public static string TokenizeJapaneseOriginal(string input)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "python", 
                Arguments = $"Tokenizer\\tokenizer.py \"{input}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory // where script lives
            };

            using var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Python error: {error}");
            }

            return output;
        }

        public async Task<string> TokenizeJapanese(string input)
        {
            using HttpClient client = new HttpClient();

            var requestObject = new { text = input };

            string json = System.Text.Json.JsonSerializer.Serialize(requestObject);

            // turn the json into HttpHontent
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // send POST request to Flask-MeCab API
                HttpResponseMessage response = await client.PostAsync(apiUrl, httpContent);

                response.EnsureSuccessStatusCode();

                // receive response
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine("apiUrl is " + apiUrl);
                return "error";
            }
        }
    }
}
