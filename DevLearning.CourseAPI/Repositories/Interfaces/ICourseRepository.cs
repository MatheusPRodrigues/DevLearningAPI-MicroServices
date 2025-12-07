using DevLearning.API.Models.DTOs.Course;
using DevLearning.Models;
using DevLearning.Models.DTOs.Course;
using MongoDB.Bson;

namespace DevLearning.CourseAPI.Repositories.Interfaces
{
    public interface ICourseRepository
    {
            Task CreateCourseAsync(Course course);
            Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category);
            Task<List<CourseResponseDTO>> GetCoursesByCategoryAsync(ObjectId categoryId);  
            Task<List<CourseResponseDTO>> GetCoursesByAuthorAsync(ObjectId authorId);      
            Task<CourseResponseDTO> GetOneCourseByIdAsync(ObjectId id);
            Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title);
            Task UpdateActiveCourseByTitleAsync(string title, bool active, DateTime lastUpdateDate);
            Task UpdateCourseByTitleAsync(string title, CourseUpdateDTO update);
    }
}
