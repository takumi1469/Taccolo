using System.Text.Json.Serialization;

namespace Bachelor_Thesis_Takumi_Saito
{
    public class Meaning
    {
        [JsonPropertyName("alternatives")]
        public List<string>? Alternatives { get; set; } = new List<string>();
        [JsonPropertyName("translatedText")]
        public string translatedText { get; set; }
    }
}
