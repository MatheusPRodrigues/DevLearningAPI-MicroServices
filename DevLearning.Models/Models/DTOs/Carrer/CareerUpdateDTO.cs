namespace DevLearning.Models.DTOs.Carrer
{
    public class CareerUpdateDTO
    {
        public string? Title { get; init; }
        public string? Summary { get; init; }
        public int? DurationInMinutes { get; init; }
        public bool? Active { get; init; }
        public bool? Featured { get; init; }
        public string? Tags { get; init; }
    }
}
