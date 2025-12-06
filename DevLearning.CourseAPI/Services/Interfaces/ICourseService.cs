using DevLearning.API.Models.DTOs.Course;
using DevLearning.Models.DTOs.Course;

namespace DevLearning.CourseAPI.Services.Interfaces
{
    public interface ICourseService
    {
        public interface ICourseService
        {
            Task CreateCourseAsync(CourseRequestDTO course);
            Task<List<CourseResponseDTO>> GetAllCoursesAsync();
            Task<List<CourseResponseDTO>> GetCoursesByCategoryAsync(Guid categoryId);  
            Task<List<CourseResponseDTO>> GetCoursesByAuthorAsync(Guid authorId);     
            Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title);
            Task<CourseResponseDTO> GetOneCourseByIdAsync(string id);
            Task UpdateCourseByTitleAsync(string title, CourseUpdateDTO update);
        }
    }
}
