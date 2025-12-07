using DevLearning.Models.Enums.Course;

namespace DevLearning.Models.DTOs.Course
{
    public class CourseResponseDTO
    {
        public string CourseId { get; init; }
        public string Tag { get; init; }
        public string Title { get; init; }
        public string Summary { get; init; }
        public string Url { get; init; }
        public CourseLevel Level { get; init; }
        public string LevelLabel => Level.ToString();
        public int DurationInMinutes { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime LastUpdateDate { get; init; }
        public bool Active { get; init; }
        public bool Free { get; init; }
        public bool Featured { get; init; }
        public string AuthorId { get; set; }
        public string CategoryId { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string Tags { get; init; }
    }
}
