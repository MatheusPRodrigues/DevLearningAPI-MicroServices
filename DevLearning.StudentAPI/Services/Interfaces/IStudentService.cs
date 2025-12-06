using DevLearning.Models.DTOs.Student;

namespace DevLearning.StudentAPI.Services.Interfaces
{
    public interface IStudentService
    {
        public Task CreateStudent(StudentRequestDTO student);
        public Task InsertStudentCourse(Guid studentId, string courseId, StudentRequestInsertCourseDTO studentCourse);
        public Task<List<StudentResponseDTO>> GetAllStudents();
        public Task<StudentResponseDTO> GetStudentById(string id);
        public Task<StudentResponseDTO> GetStudentByDocument(string document);
        public Task<StudentResponseDTO> GetStudentByEmail(string email);
        public Task UpdateStudentCourse(Guid studentId, string courseId, StudentCourseRequestUpdateDTO studentCourse);
        public Task UpdateStudent(StudentRequestUpdateDTO student, string id);
    }
}
