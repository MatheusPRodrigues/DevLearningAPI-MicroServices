using DevLearning.Models.DTOs.Course;

namespace DevLearning.Models.DTOs.Student
{
    public class StudentResponseDTO
    {
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Document { get; set; }
        public string? Phone { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime CreateDate { get; set; }
        public List<CourseStudentDTO>? Courses { get; set; } = new();
    }
}
