using DevLearning.API.Models.DTOs.Author;
using DevLearning.API.Models.DTOs.Course;
using DevLearning.CourseAPI.Repositories;
using DevLearning.CourseAPI.Repositories.Interfaces;
using DevLearning.CourseAPI.Services.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Category;
using DevLearning.Models.DTOs.Course;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
using System.Net;

namespace DevLearning.CourseAPI.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly HttpClient _authorClient;
        private readonly HttpClient _categoryClient;
        private readonly HttpClient _studentClient;
        private readonly ILogger<CourseService> _logger;

        public CourseService(
         ICourseRepository courseRepository,
         IHttpClientFactory httpClientFactory,
         ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
            _authorClient = httpClientFactory.CreateClient("AuthorAPI");
            _categoryClient = httpClientFactory.CreateClient("CategoryAPI");
            _studentClient = httpClientFactory.CreateClient("StudentAPI");
        }

        public async Task CreateCourseAsync(CourseRequestDTO course)
        {
            var verifyTitle = await _courseRepository.GetOneCourseByTitleAsync(course.Title);
            if (verifyTitle != null)
                throw new Exception("Título de curso já existente!");

            var authorResp = await _authorClient.GetAsync($"{course.AuthorId}");
            if (!authorResp.IsSuccessStatusCode)
                throw new Exception("Autor inexistente!");

            var categoryResp = await _categoryClient.GetAsync($"{course.CategoryId}");
            if (!categoryResp.IsSuccessStatusCode)
                throw new Exception("Categoria inexistente!");

            var newCourse = new Course(
                ObjectId.GenerateNewId(),
                course.Tag,
                course.Title,
                course.Summary,
                course.Url,
                course.Level,
                course.DurationInMinutes,
                DateTime.UtcNow,
                DateTime.UtcNow,
                true,
                false,
                false,
                course.AuthorId,
                course.CategoryId,
                course.Tags
            );

            await _courseRepository.CreateCourseAsync(newCourse);
        }

        public async Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category)
        {
            var courses = await _courseRepository.GetAllCoursesAsync(category);

            foreach (var c in courses)
            {
                AuthorResponseDTO author = null;
                var authorResp = await _authorClient.GetAsync($"{c.AuthorId}");
                if (authorResp.IsSuccessStatusCode)
                    author = await authorResp.Content.ReadFromJsonAsync<AuthorResponseDTO>();

                CategoryResponseDTO categoryDto = null;
                var categoryResp = await _categoryClient.GetAsync($"{c.CategoryId}");
                if (categoryResp.IsSuccessStatusCode)
                    categoryDto = await categoryResp.Content.ReadFromJsonAsync<CategoryResponseDTO>();

                c.AuthorName = author?.Name;
                c.CategoryName = categoryDto?.Title;
            }

            return courses;
        }

        public async Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title)
        {
            try
            {
                return await _courseRepository.GetOneCourseByTitleAsync(title);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateActiveCourseByTitleAsync(string title, CourseActiveDTO update)
        {
            try
            {
                var courseStorage = await _courseRepository.GetOneCourseByTitleAsync(title);
                if (courseStorage is null)
                    throw new Exception("Você não modificar um curso inexistente!");

                var requestUrl = $"CountStudentsInCourse/{courseStorage.CourseId}";

                var response = await _studentClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Erro ao consultar alunos do curso no StudentAPI.");

                var countString = await response.Content.ReadAsStringAsync();
                if (!int.TryParse(countString, out var verifyStudentCourse))
                    throw new Exception("Resposta inválida do StudentAPI ao contar alunos.");

                if (verifyStudentCourse > 0)
                    throw new Exception("Você não pode inativar um curso com alunos nele!");

                await _courseRepository.UpdateActiveCourseByTitleAsync(
                    title,
                    update.Active,
                    DateTime.UtcNow
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar Active do curso {Title}", title);
                throw;
            }
        }

        public async Task<CourseResponseDTO> GetOneCourseByIdAsync(ObjectId id)
        {
            try
            {
                return await _courseRepository.GetOneCourseByIdAsync((id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CourseResponseDTO>> GetCoursesByCategoryAsync(Guid categoryId)
        {
            var courses = await _courseRepository.GetCoursesByCategoryAsync(categoryId);

            foreach (var c in courses)
            {
                var authorResp = await _authorClient.GetAsync($"{c.AuthorId}");
                if (authorResp.IsSuccessStatusCode)
                {
                    var author = await authorResp.Content.ReadFromJsonAsync<AuthorResponseDTO>();
                    c.AuthorName = author?.Name;
                }

                var categoryResp = await _categoryClient.GetAsync($"{c.CategoryId}");
                if (categoryResp.IsSuccessStatusCode)
                {
                    var category = await categoryResp.Content.ReadFromJsonAsync<CategoryResponseDTO>();
                    c.CategoryName = category?.Title;
                }
            }

            return courses;
        }

        public async Task<List<CourseResponseDTO>> GetCoursesByAuthorAsync(Guid authorId)
        {
            var courses = await _courseRepository.GetCoursesByAuthorAsync(authorId);

            foreach (var c in courses)
            {
                var authorResp = await _authorClient.GetAsync($"{c.AuthorId}");
                if (authorResp.IsSuccessStatusCode)
                {
                    var author = await authorResp.Content.ReadFromJsonAsync<AuthorResponseDTO>();
                    c.AuthorName = author?.Name;
                }

                var categoryResp = await _categoryClient.GetAsync($"{c.CategoryId}");
                if (categoryResp.IsSuccessStatusCode)
                {
                    var category = await categoryResp.Content.ReadFromJsonAsync<CategoryResponseDTO>();
                    c.CategoryName = category?.Title;
                }
            }

            return courses;
        }

        public async Task UpdateCourseByTitleAsync(string title, CourseUpdateDTO update)
        {
            try
            {
                var courseStorage = await _courseRepository.GetOneCourseByTitleAsync(title);
                if (courseStorage is null)
                    throw new Exception("Você não pode modificar um curso inexistente!");

                await _courseRepository.UpdateCourseByTitleAsync(title, update);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar curso {Title}", title);
                throw;
            }
        }
    }
}
