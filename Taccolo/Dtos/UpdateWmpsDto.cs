namespace Taccolo.Dtos
{
    public class UpdateWmpsDto
    {
        public List<WordMeaningPairDto>? WordMeaningPairs { get; set; }
        public List<Guid>? WmpsToDelete { get; set; }
    }
}
