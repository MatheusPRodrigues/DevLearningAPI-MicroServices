using DevLearning.Models;
using DevLearning.Models.DTOs.Student;

namespace DevLearning.StudentAPI.Repository.Interfaces
{
    public interface IStudentRepository
    {
        public Task CreateStudent(Student student);
        public Task UpdateStudent(Student student, Guid id);
        public Task<List<StudentResponseDTO>> GetAllStudents();
        public Task<StudentResponseDTO> GetStudentByDocument(string document);
        public Task<StudentResponseDTO> GetStudentByEmail(string email);
        public Task<StudentResponseDTO> GetStudentByEmailAndDocument(string email, string document);
        public Task<StudentResponseDTO> GetStudentById(Guid id);
        public Task<int> GetCountStudentCourse(string courseId);
        public Task InsertStudentCourse(Guid studentId, string courseId, StudentRequestInsertCourseDTO studentCourse);
        public Task UpdateStudentCourse(Guid studentId, string courseId, StudentCourseRequestUpdateDTO studentCourse);
    }
}
