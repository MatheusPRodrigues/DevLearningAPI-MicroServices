using DevLearning.API.Models.DTOs.Course;
using DevLearning.Models;
using MongoDB.Bson;

namespace DevLearning.CourseAPI.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        public interface ICourseRepository
        {
            Task CreateCourseAsync(Course course);
            Task<List<CourseResponseDTO>> GetAllCoursesAsync();
            Task<List<CourseResponseDTO>> GetCoursesByCategoryAsync(ObjectId categoryId);  
            Task<List<CourseResponseDTO>> GetCoursesByAuthorAsync(ObjectId authorId);      
            Task<CourseResponseDTO> GetOneCourseByIdAsync(ObjectId id);
            Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title);
            Task UpdateCourseByTitleAsync(string title, bool free, bool featured, DateTime lastUpdateDate);
            Task UpdateActiveCourseByTitleAsync(string title, bool active, DateTime lastUpdateDate);
        }
    }
}
