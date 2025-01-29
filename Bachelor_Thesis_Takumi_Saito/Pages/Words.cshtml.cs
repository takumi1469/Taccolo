using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibreTranslate.Net;
using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bachelor_Thesis_Takumi_Saito.Pages
{
    public class WordsModel : PageModel
    {
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string InputText { get; set; }

        [BindProperty]
        public string SourceChoice { get; set; }

        [BindProperty]
        public string TargetChoice { get; set; }
        public (LanguageCode, string) SourceLanguage { get; set; }
        public (LanguageCode, string) TargetLanguage { get; set; }

        public LibreTranslate.Net.LibreTranslate MyLibreTranslate = new LibreTranslate.Net.LibreTranslate("http://127.0.0.1:5000");
        public string Result { get; set; }
        public List<WordMeaningPair> IndividualWords = new List<WordMeaningPair>();
        //public List<string> IndividualWords = new List<string>();
        public class Meaning
        {
            [JsonPropertyName("alternatives")]
            public List<string>? Alternatives { get; set; } = new List<string>();
            [JsonPropertyName("translatedText")]
            public string translatedText { get; set; }
        }

        public class WordMeaningPair
        {
            public string word { get; set; }
            public Meaning MeaningPaired { get; set; }

            public WordMeaningPair(string _word, Meaning _meaningPaired)
            {
                this.word = _word;
                this.MeaningPaired = _meaningPaired;
            }
        }

        public readonly UserManager<ApplicationUser> _userManager;

        public readonly AppDbContext _context;

        public WordsModel(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context; //"context" comes from DI
            _userManager = userManager; //"userManager" comes from DI
        }

        public async Task<string> LookupLibreTranslate(string word)
        {
            string meaning = await MyLibreTranslate.TranslateAsync(new Translate()
            {
                ApiKey = "",
                Source = SourceLanguage.Item1,
                Target = TargetLanguage.Item1,
                Text = word
            });
            return meaning;
        }
        public async Task OnGetAsync()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User); //Gets user information from DI
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                //translate whole text by Azure Translator
                if (!string.IsNullOrEmpty(InputText))
                {
                    SourceLanguage = DetermineLanguage(SourceChoice);
                    TargetLanguage = DetermineLanguage(TargetChoice);
                    string toTranslate = await AzureTranslator.AzTranslate(SourceLanguage.Item2, TargetLanguage.Item2, InputText);
                    Result = toTranslate;
                }
                else
                    Result = "Input is empty";

                //look up individual words by LibreTranslate
                string cleanedInput = Regex.Replace(InputText, @"[^\w\s']", "");
                string[] wordsArray = cleanedInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> allWords = new List<string>(wordsArray);
                foreach (string word in allWords)
                {
                    Meaning NewMeaning = JsonSerializer.Deserialize<Meaning>(await LookupLibreTranslate(word));
                    WordMeaningPair NewWordMeaningPair = new WordMeaningPair(word, NewMeaning);
                    IndividualWords.Add(NewWordMeaningPair);
                    //IndividualWords.Add(await LookupLibreTranslate(word));
                }
            }

            string UserId = (await _userManager.GetUserAsync(User))?.Id;

            LearningSet NewSet = new LearningSet(Title, InputText, Result, SourceChoice, TargetChoice, UserId);
            InputText = NewSet.Input;
            Result = NewSet.Translation;

            _context.LearningSets.Add(NewSet);
            await _context.SaveChangesAsync();

            return Page();
        }

        public (LanguageCode, string) DetermineLanguage(string choice)
        {
            (LanguageCode, string) CodePair = choice switch
            {
                "English" => (LanguageCode.English, "en"),
                "Albanian" => (LanguageCode.Albanian, "sq"),
                "Arabic" => (LanguageCode.Arabic, "ar"),
                "Azerbaijani" => (LanguageCode.Azerbaijani, "az"),
                "Basque" => (LanguageCode.Basque, "eu"),
                //"Bengali" => (LanguageCode.Bengali,),
                "Bulgarian" => (LanguageCode.Bulgarian, "bg"),
                "Catalan" => (LanguageCode.Catalan, "ca"),
                "Chinese" => (LanguageCode.Chinese, "zh-Hant"),
                "Czech" => (LanguageCode.Czech, "cs"),
                "Danish" => (LanguageCode.Danish, "da"),
                "Dutch" => (LanguageCode.Dutch, "nl"),
                //"Esperanto" => (LanguageCode.Esperanto,""),
                "Estonian" => (LanguageCode.Estonian, "et"),
                "Finnish" => (LanguageCode.Finnish, "fi"),
                "French" => (LanguageCode.French, "fr"),
                "Galician" => (LanguageCode.Galician, "gl"),
                "German" => (LanguageCode.German, "de"),
                "Greek" => (LanguageCode.Greek, "el"),
                "Hebrew" => (LanguageCode.Hebrew, "he"),
                "Hindi" => (LanguageCode.Hindi, "hi"),
                "Hungarian" => (LanguageCode.Hungarian, "hu"),
                "Indonesian" => (LanguageCode.Indonesian, "id"),
                "Irish" => (LanguageCode.Irish, "ga"),
                "Italian" => (LanguageCode.Italian, "it"),
                "Japanese" => (LanguageCode.Japanese, "ja"),
                "Korean" => (LanguageCode.Korean, "ko"),
                "Latvian" => (LanguageCode.Latvian, "lv"),
                "Lithuanian" => (LanguageCode.Lithuanian, "lt"),
                "Malay" => (LanguageCode.Malay, "ms"),
                "Norwegian" => (LanguageCode.Norwegian, "nb"),
                "Persian" => (LanguageCode.Persian, "fa"),
                "Polish" => (LanguageCode.Polish, "pl"),
                "Portuguese" => (LanguageCode.Portuguese, "pt"),
                "Romanian" => (LanguageCode.Romanian, "ro"),
                "Russian" => (LanguageCode.Russian, "ru"),
                "Slovak" => (LanguageCode.Slovak, "sk"),
                "Slovenian" => (LanguageCode.Slovenian, "sl"),
                "Spanish" => (LanguageCode.Spanish, "es"),
                "Swedish" => (LanguageCode.Swedish, "sv"),
                "Tagalog" => (LanguageCode.Tagalog, "fil"),
                "Thai" => (LanguageCode.Thai, "th"),
                "Turkish" => (LanguageCode.Turkish, "tr"),
                "Ukranian" => (LanguageCode.Ukranian, "uk"),
                "Urdu" => (LanguageCode.Urdu, "ur")
            };
            return CodePair;
        }
    }
}
