using DevLearning.API.Models.DTOs.Course;
using DevLearning.Models;

namespace DevLearning.CourseAPI.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        public interface ICourseRepository
        {
            Task CreateCourseAsync(Course course);

            Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category);

            Task<CourseResponseDTO> GetOneCourseByIdAsync(Guid id);
            Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title);

            Task UpdateCourseByTitleAsync(string title, bool free, bool featured, DateTime lastUpdateDate);

            Task UpdateActiveCourseByTitleAsync(string title, bool active, DateTime lastUpdateDate);
        }
    }
}
