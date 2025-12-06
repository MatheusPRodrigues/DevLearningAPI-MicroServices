using DevLearning.API.Models.DTOs.Course;
using DevLearning.Models.DTOs.Course;
using MongoDB.Bson;

namespace DevLearning.CourseAPI.Services.Interfaces
{
    public interface ICourseService
    {
        
            Task CreateCourseAsync(CourseRequestDTO course);
            Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category);
            Task<List<CourseResponseDTO>> GetCoursesByCategoryAsync(ObjectId categoryId);
            Task<List<CourseResponseDTO>> GetCoursesByAuthorAsync(ObjectId authorId);
            Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title);
            Task<CourseResponseDTO> GetOneCourseByIdAsync(ObjectId id);
            Task UpdateActiveCourseByTitleAsync(string title, CourseActiveDTO update);
            Task UpdateCourseByTitleAsync(string title, CourseUpdateDTO update);
    }
}
