using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibreTranslate.Net;
using Taccolo.Pages.Data;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Configuration;
using Taccolo.Pages.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GenerativeAI;
using GenerativeAI.Types;

namespace Taccolo.Pages
{
    public class MakeNewLsModel : PageModel
    {
        [BindProperty]
        public bool ShowOutputField { get; set; } = false;

        public LearningSet? TempLearningSet { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string InputText { get; set; }

        [BindProperty]
        public string SourceChoice { get; set; }

        [BindProperty]
        public string TargetChoice { get; set; }
        public string WarningMessage {  get; set; } = string.Empty;
        public (LanguageCode, string) SourceLanguage { get; set; }
        public (LanguageCode, string) TargetLanguage { get; set; }

        private static string LibreTranslateUrl;

        public LibreTranslate.Net.LibreTranslate MyLibreTranslate = new LibreTranslate.Net.LibreTranslate("http://127.0.0.1:5000");

        //public LibreTranslate.Net.LibreTranslate MyLibreTranslate = new LibreTranslate.Net.LibreTranslate("LibreTranslateUrl");

        public string Result { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MakeNewLsModel> _logger;

        public MakeNewLsModel(AppDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<MakeNewLsModel> logger)
        {
            _context = context; //"context" comes from DI
            _userManager = userManager; //"userManager" comes from DI
            _configuration = configuration;
            _logger = logger;   
        }

        public async Task<string> LookupLibreTranslate(LibreTranslate.Net.LibreTranslate myLibreTranslate, string word)
        {
            string meaning = await myLibreTranslate.TranslateAsync(new Translate()
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

        public async Task<IActionResult> OnPostProcessAsync()
        {
            _logger.LogError("*******OnPostProcess is called*******");

            string dateCreation = DateTime.Now.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture);
            DateTime dateCreationForOrder = DateTime.UtcNow;
            if (SourceChoice == "not-chosen" || TargetChoice == "not-chosen")
            {
                WarningMessage = "Please select both Source Language and Target Language";
                return Page();
            }

            ShowOutputField = true;

            if (!string.IsNullOrEmpty(InputText))
            {
                string UserId = (await _userManager.GetUserAsync(User))?.Id;

                //translate whole text by Azure Translator
                SourceLanguage = DetermineLanguage(SourceChoice);
                TargetLanguage = DetermineLanguage(TargetChoice);
                AzureTranslator azureTranslator = new AzureTranslator(_configuration);
                string toTranslate = await azureTranslator.AzTranslate(SourceLanguage.Item2, TargetLanguage.Item2, InputText);
                Result = toTranslate;

                // TempLearningSet = new LearningSet(Title, InputText, Result, SourceChoice, TargetChoice, date, UserId);
                TempLearningSet = new LearningSet
                    (title: Title,
                    input: InputText,
                    translation: Result,
                    sourceLanguage: SourceChoice,
                    targetLanguage: TargetChoice,
                    date: dateCreation,
                    userId: UserId,
                    dateForOrder: dateCreationForOrder) ;

                // Tokenize Japanese
                if(SourceChoice == "Japanese")
                {
                    Tokenizer tokenizer = new Tokenizer(_configuration);
                    InputText = await tokenizer.TokenizeJapanese(InputText);
                }

                // look up individual words by LibreTranslate
                LibreTranslateUrl = _configuration["LibreTranslate:URL"];
                LibreTranslate.Net.LibreTranslate MyLibreTranslate = new LibreTranslate.Net.LibreTranslate(LibreTranslateUrl);

                _logger.LogInformation("**********LibreTranslate URL is " + LibreTranslateUrl + "***************");
                
                string cleanedInput = Regex.Replace(InputText, @"[^\w\s']", "");
                string[] wordsArray = cleanedInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> allWords = new List<string>(wordsArray);

                int tempOrder = 1;
                string geminiKey = _configuration["GoogleGemini:ApiKey"];
                var googleAI = new GoogleAi(geminiKey);
                var googleModel = googleAI.CreateGenerativeModel("models/gemini-2.0-flash");

                try
                {
                    foreach (string word in allWords)
                    {
                        string wordLower = word.ToLower();
                        Meaning NewMeaning = new Meaning();

                        if (SourceChoice == "Japanese" || SourceChoice == "Urdu" || TargetChoice == "Japanese" || TargetChoice == "Urdu")
                        {
                            var googleResponse = await googleModel.GenerateContentAsync($@"
Give me JSON response about {TargetLanguage} translation of the {SourceLanguage} word {wordLower} in the following format:
{{
    ""alternatives"": [
        ""<alternative_meaning_1>"",
        ""<alternative_meaning_2>"",
        ""<alternative_meaning_3>""
    ],
    ""translatedText"": ""<main_meaning>""
}}.
Give me exactly three alternatives and one main meaning.");
                            string responseJson = googleResponse.Text().Trim().Remove(0, 7).TrimEnd('`');

                            _logger.LogInformation(responseJson);
                            
                            NewMeaning = JsonSerializer.Deserialize<Meaning>(responseJson);

                        }
                        else
                        {
                            NewMeaning = JsonSerializer.Deserialize<Meaning>(await LookupLibreTranslate(MyLibreTranslate, wordLower));
                        }

                        WordMeaningPair NewWordMeaningPair = new WordMeaningPair(TempLearningSet.Id,
                            wordLower, NewMeaning.translatedText, NewMeaning.Alternatives, tempOrder);
                        TempLearningSet.WordMeaningPairs.Add(NewWordMeaningPair);
                        tempOrder++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                // TempData might be causing cookie bloating that makes the site inaccessible
                //TempData["TempLearningSet"] = JsonSerializer.Serialize(TempLearningSet);

                var tempLearningSetString = JsonSerializer.Serialize(TempLearningSet);
                HttpContext.Session.SetString("tempLearningSetString", tempLearningSetString);

                return Page();
            }
            else
                Result = "Input is empty";
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (HttpContext.Session.GetString("tempLearningSetString") is string serializedSet)
            {
                // Deserialize the TempLearningSet from TempData
                TempLearningSet = JsonSerializer.Deserialize<LearningSet>(serializedSet);

                if (TempLearningSet is not null)
                {
                    // Now save TempLearningSet to the database                    
                    _context.LearningSets.Add(TempLearningSet);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("EditViewLs", new { lsid = TempLearningSet.Id });
                }
                else return Page();
            }
            else
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
