namespace DevLearning.Models.DTOs.CareerItem
{
    public class CareerItemRequestDTO
    {
        public Guid CourseId { get; init; }       
        public string Title { get; init; }
        public string Description { get; init; }
        public byte Order { get; init; }
    }
}
