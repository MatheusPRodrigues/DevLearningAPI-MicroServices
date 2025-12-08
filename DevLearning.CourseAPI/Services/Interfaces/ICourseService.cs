using DevLearning.API.Models.DTOs.Course;
using DevLearning.Models.DTOs.Course;
using MongoDB.Bson;

namespace DevLearning.CourseAPI.Services.Interfaces
{
    public interface ICourseService
    {
        
            Task CreateCourseAsync(CourseRequestDTO course);
            Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category);
            Task<List<CourseResponseDTO>> GetCoursesByCategoryAsync(Guid categoryId);
            Task<List<CourseResponseDTO>> GetCoursesByAuthorAsync(Guid authorId);
            Task<CourseResponseDTO> GetOneCourseByTitleAsync(CourseRequestTitleDTO title);
            Task<CourseResponseDTO> GetOneCourseByIdAsync(ObjectId id);
            Task UpdateActiveCourseByTitleAsync(string title, CourseActiveDTO update);
            Task UpdateCourseByTitleAsync(string title, CourseUpdateDTO update);
    }
}
