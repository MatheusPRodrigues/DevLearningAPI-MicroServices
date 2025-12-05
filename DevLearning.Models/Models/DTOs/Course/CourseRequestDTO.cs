using DevLearning.Models.Enums.Course;

namespace DevLearning.API.Models.DTOs.Course
{
    public class CourseRequestDTO
    {
        public string Tag { get; init; }
        public string Title { get; init; }
        public string Summary { get; init; }
        public string Url { get; init; }
        public CourseLevel Level { get; init; }
        public int DurationInMinutes { get; init; }
        public Guid AuthorId { get; init; }
        public Guid CategoryId { get; init; }
        public string Tags { get; init; }
    }
}
