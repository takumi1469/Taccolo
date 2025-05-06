namespace Taccolo.Dtos
{
    public class SearchQueryDto
    {
        public string? Keywords { get; set; }
        public string? SourceLanguage { get; set; }
        public string? TargetLanguage { get; set; }

        public string? matchAndOr {  get; set; }

    }
}
