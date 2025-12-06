using Dapper;
using DevLearning.Models;
using DevLearning.Models.DTOs.Course;
using DevLearning.Models.DTOs.Student;
using DevLearning.StudentAPI.Data;
using DevLearning.StudentAPI.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace DevLearning.StudentAPI.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SqlConnection _connection;
        public StudentRepository(ConnectionDB dbConnection)
        {
            _connection = dbConnection.GetConnection();
        }

        public async Task CreateStudent(Student student)
        {
            try
            {
                var sql = @"INSERT INTO Student VALUES (@Id, @Name, @Email, @Document, @Phone, @Birthdate, @CreateDate)";
                await _connection.ExecuteAsync(sql, new 
                { 
                    Id = student.Id,
                    Name = student.Name, 
                    Email = student.Email, 
                    Document = student.Document, 
                    Phone = student.Phone,
                    Birthdate = student.Birthdate, 
                    CreateDate = student.CreateDate 
                });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
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
                var sql = @"SELECT 
                            s.Id AS StudentId, 
                            s.Name AS [Name], 
                            s.Email AS Email, 
                            s.Document AS Document, 
                            s.Phone AS Phone, 
                            s.Birthdate AS BirthDate, 
                            s.CreateDate AS CreateDate,
                            sc.CourseId AS CourseId,
                            sc.Progress as Progress,
                            sc.Favorite AS Favorite, 
                            sc.StartDate AS StartDate, 
                            sc.LastUpdateDate AS LastUpdateDate
                            FROM Student s
                            LEFT JOIN StudentCourse sc ON sc.StudentId = s.Id;";
                var students = await _connection.QueryAsync<StudentResponseDTO, CourseStudentDTO, StudentResponseDTO>(sql, (student, course) =>
                {
                    student.Courses ??= new List<CourseStudentDTO>();
                    if (course != null && course.CourseId != String.Empty)
                        student.Courses.Add(course);

                    return student;
                }, splitOn: "CourseId");

                var result = students.GroupBy(s => s.StudentId).Select(g =>
                {
                    var groupedStudent = g.FirstOrDefault();
                    groupedStudent.Courses = g.SelectMany(s => s.Courses ?? new List<CourseStudentDTO>()).ToList();
                    return groupedStudent;
                });

                return result.ToList();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<int> GetCountStudentCourse(Guid courseId)
        {
            try
            {
                var sql = @"SELECT COUNT(*) FROM StudentCourse WHERE CourseId = @CourseId";
                return _connection.ExecuteScalarAsync<int>(sql, new { CourseId = courseId });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
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
                var sql = @"SELECT 
                            s.Id AS StudentId, 
                            s.Name AS [Name], 
                            s.Email AS Email, 
                            s.Document AS Document, 
                            s.Phone AS Phone, 
                            s.Birthdate AS BirthDate, 
                            s.CreateDate AS CreateDate,
                            sc.CourseId AS CourseId,
                            sc.Progress as Progress,
                            sc.Favorite AS Favorite, 
                            sc.StartDate AS StartDate, 
                            sc.LastUpdateDate AS LastUpdateDate
                            FROM Student s
                            LEFT JOIN StudentCourse sc ON sc.StudentId = s.Id
                            WHERE Document = @Document;";

                //    var sql = @"SELECT 
                //s.Id AS StudentId, 
                //s.Name AS [Name], 
                //s.Email AS Email, 
                //s.Document AS Document, 
                //s.Phone AS Phone, 
                //s.Birthdate AS BirthDate, 
                //s.CreateDate AS CreateDate,
                //c.Id AS CourseId, 
                //c.Title AS Title, 
                //c.Summary AS Summary, 
                //c.Url AS [Url], 
                //c.Level AS [Level], 
                //sc.Progress as Progress,
                //c.DurationInMinutes AS DurationInMinutes,
                //sc.Favorite AS Favorite, 
                //sc.StartDate AS StartDate, 
                //sc.LastUpdateDate AS LastUpdateDate
                //FROM Student s
                //LEFT JOIN StudentCourse sc ON sc.StudentId = s.Id
                //LEFT JOIN Course c ON sc.CourseId = c.Id WHERE Document = @Document";
                var students = await _connection.QueryAsync<StudentResponseDTO, CourseStudentDTO, StudentResponseDTO>(sql, (student, course) =>
                {
                    student.Courses ??= new List<CourseStudentDTO>();
                    if (course != null && course.CourseId != String.Empty)
                        student.Courses.Add(course);

                    return student;
                }, new { Document = document }, splitOn: "CourseId");

                var result = students.GroupBy(s => s.StudentId).Select(g =>
                {
                    var groupedStudent = g.FirstOrDefault();
                    groupedStudent.Courses = g.SelectMany(s => s.Courses ?? new List<CourseStudentDTO>()).ToList();
                    return groupedStudent;
                });

                return result.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
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
                var sql = @"SELECT 
                            s.Id AS StudentId, 
                            s.Name AS [Name], 
                            s.Email AS Email, 
                            s.Document AS Document, 
                            s.Phone AS Phone, 
                            s.Birthdate AS BirthDate, 
                            s.CreateDate AS CreateDate,
                            sc.CourseId AS CourseId,
                            sc.Progress as Progress,
                            sc.Favorite AS Favorite, 
                            sc.StartDate AS StartDate, 
                            sc.LastUpdateDate AS LastUpdateDate
                            FROM Student s
                            LEFT JOIN StudentCourse sc ON sc.StudentId = s.Id
                            WHERE Email = @Email;";

                //    var sql = @"SELECT 
                //s.Id AS StudentId, 
                //s.Name AS [Name], 
                //s.Email AS Email, 
                //s.Document AS Document, 
                //s.Phone AS Phone, 
                //s.Birthdate AS BirthDate, 
                //s.CreateDate AS CreateDate,
                //c.Id AS CourseId, 
                //c.Title AS Title, 
                //c.Summary AS Summary, 
                //c.Url AS [Url], 
                //c.Level AS [Level], 
                //sc.Progress as Progress,
                //c.DurationInMinutes AS DurationInMinutes,
                //sc.Favorite AS Favorite, 
                //sc.StartDate AS StartDate, 
                //sc.LastUpdateDate AS LastUpdateDate
                //FROM Student s
                //LEFT JOIN StudentCourse sc ON sc.StudentId = s.Id
                //LEFT JOIN Course c ON sc.CourseId = c.Id WHERE Email = @Email";
                var students = await _connection.QueryAsync<StudentResponseDTO, CourseStudentDTO, StudentResponseDTO>(sql, (student, course) =>
                {
                    student.Courses ??= new List<CourseStudentDTO>();
                    if (course != null && course.CourseId != String.Empty)
                        student.Courses.Add(course);

                    return student;
                }, new { Email = email }, splitOn: "CourseId");

                var result = students.GroupBy(s => s.StudentId).Select(g =>
                {
                    var groupedStudent = g.FirstOrDefault();
                    groupedStudent.Courses = g.SelectMany(s => s.Courses ?? new List<CourseStudentDTO>()).ToList();
                    return groupedStudent;
                });

                return result.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StudentResponseDTO> GetStudentById(Guid id)
        {
            try
            {
                var sql = @"SELECT 
                            s.Id AS StudentId, 
                            s.Name AS [Name], 
                            s.Email AS Email, 
                            s.Document AS Document, 
                            s.Phone AS Phone, 
                            s.Birthdate AS BirthDate, 
                            s.CreateDate AS CreateDate,
                            sc.CourseId AS CourseId,
                            sc.Progress as Progress,
                            sc.Favorite AS Favorite, 
                            sc.StartDate AS StartDate, 
                            sc.LastUpdateDate AS LastUpdateDate
                            FROM Student s
                            LEFT JOIN StudentCourse sc ON sc.StudentId = s.Id
                            WHERE s.Id = @StudentId;";

                //var sql = @"SELECT 
                //            s.Id AS StudentId, 
                //            s.Name AS [Name], 
                //            s.Email AS Email, 
                //            s.Document AS Document, 
                //            s.Phone AS Phone, 
                //            s.Birthdate AS BirthDate, 
                //            s.CreateDate AS CreateDate,
                //            c.Id AS CourseId, 
                //            c.Title AS Title, 
                //            c.Summary AS Summary, 
                //            c.Url AS [Url], 
                //            c.Level AS [Level], 
                //            sc.Progress as Progress,
                //            c.DurationInMinutes AS DurationInMinutes,
                //            sc.Favorite AS Favorite, 
                //            sc.StartDate AS StartDate, 
                //            sc.LastUpdateDate AS LastUpdateDate
                //            FROM Student s
                //            LEFT JOIN StudentCourse sc ON sc.StudentId = s.Id
                //            LEFT JOIN Course c ON sc.CourseId = c.Id WHERE s.Id = @StudentId";
                var students = await _connection.QueryAsync<StudentResponseDTO, CourseStudentDTO, StudentResponseDTO>(sql, (student, course) =>
                {
                    student.Courses ??= new List<CourseStudentDTO>();
                    if (course != null && course.CourseId != String.Empty)
                        student.Courses.Add(course);

                    return student;
                }, new { StudentId = id }, splitOn: "CourseId");

                var result = students.GroupBy(s => s.StudentId).Select(g =>
                {
                    var groupedStudent = g.FirstOrDefault();
                    groupedStudent.Courses = g.SelectMany(s => s.Courses ?? new List<CourseStudentDTO>()).ToList();
                    return groupedStudent;
                });

                return result.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
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
                var sql = @"INSERT INTO StudentCourse (CourseId, StudentId, Favorite, Progress, StartDate)
                            VALUES (@CourseId, @StudentId, @Favorite, @Progress, @StartDate)";
                await _connection.ExecuteAsync(sql, new 
                { 
                    CourseId = courseId,
                    StudentId = studentId,
                    Favorite = studentCourse.Favorite,
                    Progress = studentCourse.Progress,
                    StartDate = studentCourse.StartDate 
                });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateStudent(Student student, Guid id)
        {
            try
            {
                var sql = @"UPDATE Student SET
                            Name = @Name,
                            Email = @Email,
                            Document = @Document,
                            Phone = @Phone,
                            Birthdate = @Birthdate
                            WHERE Id = @id";
                await _connection.ExecuteAsync(sql, new 
                { 
                    Name = student.Name,
                    Email = student.Email,
                    Document = student.Document,
                    Phone = student.Phone,
                    Birthdate = student.Birthdate,
                    id = id 
                });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
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
                var sql = @"UPDATE StudentCourse SET 
                            Progress = @Progress,
                            Favorite = @Favorite WHERE CourseId = @CourseId AND StudentId = @StudentId";
                await _connection.ExecuteAsync(sql, new 
                { 
                    Progress = studentCourse.Progress,
                    Favorite = studentCourse.Favorite, 
                    CourseId = courseId, StudentId = studentId 
                });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StudentResponseDTO> GetStudentByEmailAndDocument(string email, string document)
        {
            try
            {
                var sql = @"SELECT 
                            s.Id AS StudentId, 
                            s.Name AS [Name], 
                            s.Email AS Email, 
                            s.Document AS Document, 
                            s.Phone AS Phone, 
                            s.Birthdate AS BirthDate, 
                            s.CreateDate AS CreateDate
                            FROM Student s
                            WHERE Document = @Document AND Email = @Email";

                return await _connection.QueryFirstOrDefaultAsync<StudentResponseDTO>(sql, new 
                { 
                    Document = document,
                    Email = email 
                });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
