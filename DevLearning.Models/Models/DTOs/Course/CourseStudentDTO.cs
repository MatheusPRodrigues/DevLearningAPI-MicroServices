using DevLearning.Models.Enums.Course;
using DevLearning.Models.Enums.StudentCourse;

namespace DevLearning.Models.DTOs.Course
{
    public class CourseStudentDTO
    {
        public string CourseId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public CourseLevel Level { get; set; }
        public byte Progress { get; set; }
        public string LevelLabel => Level.ToString();
        public int DurationInMinutes { get; set; }
        public FavoriteType Favorite { get; set; }
    }
}
