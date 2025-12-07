using DevLearning.Models;
using DevLearning.Models.DTOs.Student;
using DevLearning.StudentAPI.Repository;
using DevLearning.StudentAPI.Repository.Interfaces;
using DevLearning.StudentAPI.Services.Interfaces;

namespace DevLearning.StudentAPI.Services
{
    public class StudentService : IStudentService
    {
        private IStudentRepository _studentRepository;
        private HttpClient _httpClient;

        public StudentService(
            IStudentRepository studentRepository,
            HttpClient httpClient
        )
        {
            _studentRepository = studentRepository;
            _httpClient = httpClient;
        }

        public async Task CreateStudent(StudentRequestDTO student)
        {
            try
            {
                var studentStorage = await _studentRepository.GetStudentByEmailAndDocument(student.Email, student.Document);
                if (studentStorage is not null)
                    throw new Exception("Estudante com email ou documento já cadastrado.");
                var newStudent = new Student(student.Name, student.Email, student.Document, student.Phone, student.Birthdate);
                await _studentRepository.CreateStudent(newStudent);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<StudentResponseDTO>> GetAllStudents()
        {
            try
            {
                return await _studentRepository.GetAllStudents();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetCountStudentCourse(string courseId)
        {
            try
            {
                return await _studentRepository.GetCountStudentCourse(courseId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StudentResponseDTO> GetStudentByDocument(string document)
        {
            try
            {
                return await _studentRepository.GetStudentByDocument(document);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StudentResponseDTO> GetStudentByEmail(string email)
        {
            try
            {
                return await _studentRepository.GetStudentByEmail(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StudentResponseDTO> GetStudentById(string id)
        {
            try
            {
                return await _studentRepository.GetStudentById(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertStudentCourse(Guid studentId, string courseId, StudentRequestInsertCourseDTO studentCourse)
        {
            try
            {
                if (await _studentRepository.GetStudentById(studentId) is null)
                    throw new Exception("Estudante não encontrado");
                //var course = await _courseRepository.GetOneCourseByIdAsync(courseId);
                //if (course is null)
                //    throw new Exception("Curso não encontrado");
                //if (course.Active == false)
                //    throw new Exception("Curso inativo, não pode ocorrer mátricula");
                //if (await _studentRepository.GetStudentCourse(studentId, courseId) is not null)
                //    throw new Exception("Estudante já está matriculado nesse curso");
                await _studentRepository.InsertStudentCourse(studentId, courseId, studentCourse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateStudent(StudentRequestUpdateDTO student, string id)
        {
            try
            {
                var studentStorage = await _studentRepository.GetStudentById(Guid.Parse(id));
                if (studentStorage is null)
                    throw new Exception("Estudante não encontrado");
                if (student.Document is not null && await _studentRepository.GetStudentByDocument(student.Document) is not null)
                    throw new Exception("O documento informado já está cadastrado.");
                if (student.Email is not null && await _studentRepository.GetStudentByEmail(student.Email) is not null)
                    throw new Exception("O email informado já está cadastrado.");

                var newStudent = new Student(
                    !String.IsNullOrWhiteSpace(student.Name) ? student.Name : studentStorage.Name,
                    !String.IsNullOrWhiteSpace(student.Email) ? student.Email : studentStorage.Email,
                    !String.IsNullOrWhiteSpace(student.Phone) ? student.Phone : studentStorage.Phone,
                    !String.IsNullOrWhiteSpace(student.Document) ? student.Document : studentStorage.Document,
                    student.Birthdate is not null ? (DateTime)student.Birthdate : studentStorage.Birthdate
                    );
                await _studentRepository.UpdateStudent(newStudent, Guid.Parse(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateStudentCourse(Guid studentId, string courseId, StudentCourseRequestUpdateDTO studentCourse)
        {
            try
            {
                var student = await _studentRepository.GetStudentById(studentId);
                if (student is null)
                    throw new Exception("Estudante não encontrado");
                if (!student.Courses.Any(c => c.CourseId == courseId))
                    throw new Exception("Curso não encontrado");
                await _studentRepository.UpdateStudentCourse(studentId, courseId, studentCourse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
