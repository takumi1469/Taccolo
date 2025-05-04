namespace Taccolo
{
    public class FlashcardQuestion
    {
        public string Word { get; set; }
        public string CorrectChoice { get; set; }
        public List<string> WrongChoices { get; set; } = new List<string>();  

        public FlashcardQuestion() { }
        public FlashcardQuestion(string word, string correctChoice, List<string> wrongChoices) 
        {
            Word = word;
            CorrectChoice = correctChoice;
            WrongChoices = wrongChoices;
        }
    }
}
