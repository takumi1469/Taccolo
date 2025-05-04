using System.Diagnostics;

namespace Taccolo
{
    public class Tokenizer
    {
        public static string TokenizeJapanese(string input)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "python", 
                Arguments = $"tokenizer.py \"{input}\"",
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
    }
}
