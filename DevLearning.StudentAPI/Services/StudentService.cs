using DevLearning.Models;
using DevLearning.Models.DTOs.Course;
using DevLearning.Models.DTOs.Student;
using DevLearning.StudentAPI.Repository;
using DevLearning.StudentAPI.Repository.Interfaces;
using DevLearning.StudentAPI.Services.Interfaces;
using System.Reflection.Metadata;
using System.Xml.Linq;

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
                var students = await _studentRepository.GetAllStudents();
                if (students is not null)
                {
                    foreach (var s in students)
                    {
                        if (s.Courses is not null || s.Courses.Count > 0)
                            foreach (var c in s.Courses)
                            {
                                var course = await _httpClient.GetFromJsonAsync<CourseResponseDTO>(c.CourseId);
                                c.Title = String.IsNullOrWhiteSpace(course.Title) ? "Sem título" : course.Title;
                                c.Summary = course.Summary;
                                c.Url = course.Url;
                                c.Level = course.Level;
                                c.DurationInMinutes = course.DurationInMinutes;
                            }
                    }
                }

                return students;
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
                var student = await _studentRepository.GetStudentByDocument(document);
                if (student is not null)
                {
                    if (student.Courses is not null || student.Courses.Count > 0)
                        foreach (var c in student.Courses)
                        {
                            var course = await _httpClient.GetFromJsonAsync<CourseResponseDTO>(c.CourseId);
                            c.Title = String.IsNullOrWhiteSpace(course.Title) ? "Sem título" : course.Title;
                            c.Summary = course.Summary;
                            c.Url = course.Url;
                            c.Level = course.Level;
                            c.DurationInMinutes = course.DurationInMinutes;
                        }
                }

                return student;
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
                var student = await _studentRepository.GetStudentByEmail(email);
                if (student is not null)
                {
                    if (student.Courses is not null || student.Courses.Count > 0)
                        foreach (var c in student.Courses)
                        {
                            var course = await _httpClient.GetFromJsonAsync<CourseResponseDTO>(c.CourseId);
                            c.Title = String.IsNullOrWhiteSpace(course.Title) ? "Sem título" : course.Title;
                            c.Summary = course.Summary;
                            c.Url = course.Url;
                            c.Level = course.Level;
                            c.DurationInMinutes = course.DurationInMinutes;
                        }
                }

                return student;
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
                var guidId = Guid.Parse(id);
                if (guidId == Guid.Empty)
                    throw new Exception("Id de Estudante é inváldio!");

                var student = await _studentRepository.GetStudentById(guidId);
                if (student is not null)
                {
                    if (student.Courses is not null || student.Courses.Count > 0)
                        foreach (var c in student.Courses)
                        {
                            var course = await _httpClient.GetFromJsonAsync<CourseResponseDTO>(c.CourseId);
                            c.Title = String.IsNullOrWhiteSpace(course.Title) ? "Sem título" : course.Title;
                            c.Summary = course.Summary;
                            c.Url = course.Url;
                            c.Level = course.Level;
                            c.DurationInMinutes = course.DurationInMinutes;
                        }
                }

                return student;
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
                var student = await _studentRepository.GetStudentById(studentId);
                if (student is null)
                    throw new Exception("Estudante não encontrado");

                Console.WriteLine(_httpClient.BaseAddress);

                var response = await _httpClient.GetFromJsonAsync<CourseResponseDTO>($"{courseId}");
                if (response is null)
                    throw new Exception("Curso não encontrado");
                if (response.Active == false)
                    throw new Exception("Curso inativo, não pode ocorrer mátricula");
                if (student.Courses.Any(c => c.CourseId == courseId))
                    throw new Exception("Estudante já está matriculado nesse curso");

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
