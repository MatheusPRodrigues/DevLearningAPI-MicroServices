using DevLearning.Models.Enums.Course;
using DevLearning.Models.Enums.StudentCourse;

namespace DevLearning.Models.DTOs.Course
{
    public class CourseStudentDTO
    {
        public string CourseId { get; init; }
        public string Title { get; init; }
        public string Summary { get; init; }
        public string Url { get; init; }
        public CourseLevel Level { get; init; }
        public byte Progress { get; set; }
        public string LevelLabel => Level.ToString();
        public int DurationInMinutes { get; init; }
        public FavoriteType Favorite { get; set; }
    }
}
