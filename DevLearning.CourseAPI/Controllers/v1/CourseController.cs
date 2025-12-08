using DevLearning.API.Models.DTOs.Course;
using DevLearning.CourseAPI.Services;
using DevLearning.CourseAPI.Services.Interfaces;
using DevLearning.Models.DTOs.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace DevLearning.CourseAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService service, ILogger<CourseController> logger)
        {
            _courseService = service;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<ActionResult<List<CourseResponseDTO>>> GetAllCoursesAsync([FromQuery] string? category)
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync(category);
                return Ok(courses);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("get-by-title")]
        public async Task<ActionResult<CourseResponseDTO>> GetOneCourseByTitleAsync([FromBody] CourseRequestTitleDTO dto)
        {
            try
            {
                var course = await _courseService.GetOneCourseByTitleAsync(dto);
                if (course is null)
                    return NotFound(new { message = "Curso não encontrado" });

                return Ok(course);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost()]
        public async Task<ActionResult> CreateUserAsync(CourseRequestDTO course)
        {
            try
            {
                await _courseService.CreateCourseAsync(course);
                return Created();
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{title}")]
        public async Task<IActionResult> UpdateCourseByTitleAsync(string title, CourseUpdateDTO update)
        {
            try
            {
                await _courseService.UpdateCourseByTitleAsync(title, update);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("Active/{title}")]
        public async Task<IActionResult> UpdateActiveCourseByTitleAsync(string title, CourseActiveDTO update)
        {
            try
            {
                await _courseService.UpdateActiveCourseByTitleAsync(title, update);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<CourseResponseDTO>>> GetCoursesByCategory(string categoryId)
        {
            try
            {
                var id = Guid.Parse(categoryId);
                var courses = await _courseService.GetCoursesByCategoryAsync(id);
                if (!courses.Any())
                    return NotFound(new { message = "Nenhum curso encontrado para esta categoria" });
                return Ok(courses);
            }
            catch (FormatException)
            {
                return BadRequest("ID da categoria inválido");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cursos por categoria");
                return StatusCode(500, "Erro interno");
            }
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<CourseResponseDTO>>> GetCoursesByAuthor(string authorId)
        {
            try
            {
                var id = Guid.Parse(authorId);
                var courses = await _courseService.GetCoursesByAuthorAsync(id);
                if (!courses.Any())
                    return NotFound(new { message = "Nenhum curso encontrado para este autor" });
                return Ok(courses);
            }
            catch (FormatException)
            {
                return BadRequest("ID do autor inválido");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cursos por autor");
                return StatusCode(500, "Erro interno");
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<CourseResponseDTO>> GetOneCourseByIdAsync(string Id)
        {
            try
            {
                var objectId = new ObjectId(Id);

                var course = await _courseService.GetOneCourseByIdAsync(objectId);

                if (course is null)
                    return NotFound(new { message = "Curso não encontrado" });

                return Ok(course);
            }
            catch (FormatException)
            {
                return BadRequest("ID do curso inválido");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar curso por ID");
                return StatusCode(500, "Erro interno");
            }
        }
    }
}
