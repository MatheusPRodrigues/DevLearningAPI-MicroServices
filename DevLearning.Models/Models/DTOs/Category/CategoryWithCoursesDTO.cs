namespace DevLearning.Models.DTOs.Category
{
    public class CategoryWithCoursesDTO
    {
        public string CategoryTitle { get; set; }
        public List<string> Courses { get; set; } = new List<string>();
    }
}
