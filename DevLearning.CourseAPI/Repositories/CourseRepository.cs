using DevLearning.API.Models.DTOs.Course;
using DevLearning.CourseAPI.Repositories.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Course;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DevLearning.CourseAPI.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IMongoCollection<Course> _collection;
        private readonly IMongoCollection<Author> _authorCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly ILogger<CourseRepository> _logger;

        public CourseRepository(IMongoDatabase database, ILogger<CourseRepository> logger)
        {
            _collection = database.GetCollection<Course>("courses");
            _authorCollection = database.GetCollection<Author>("authors");
            _categoryCollection = database.GetCollection<Category>("categories");
            _logger = logger;
        }

        public async Task CreateCourseAsync(Course course)
        {
            try
            {
                await _collection.InsertOneAsync(course);
            }
            catch (MongoWriteException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category)
        {
            try
            {
                FilterDefinition<Course> filter;

                if (string.IsNullOrWhiteSpace(category))
                {
                    filter = Builders<Course>.Filter.Eq(c => c.Active, true);
                }
                else
                {
                    var categoryId = Guid.Parse(category);
                    filter = Builders<Course>.Filter.Eq(c => c.CategoryId, categoryId)
                           & Builders<Course>.Filter.Eq(c => c.Active, true);
                }

                var courses = await _collection.Find(filter).ToListAsync();

                return courses.Select(c => new CourseResponseDTO
                {
                    CourseId = c.Id.ToString(),
                    Tag = c.Tag,
                    Title = c.Title,
                    Summary = c.Summary,
                    Url = c.Url,
                    Level = c.Level,
                    DurationInMinutes = c.DurationInMinutes,
                    CreateDate = c.CreateDate,
                    LastUpdateDate = c.LastUpdateDate,
                    Active = c.Active,
                    Free = c.Free,
                    Featured = c.Featured,
                    AuthorId = c.AuthorId.ToString(),
                    CategoryId = c.CategoryId.ToString(),
                    AuthorName = null,
                    CategoryName = null,
                    Tags = c.Tags
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title)
        {
            try
            {
                var filter = Builders<Course>.Filter.Eq(c => c.Title, title);
                var course = await _collection.Find(filter).FirstOrDefaultAsync();

                if (course == null) return null;

                return new CourseResponseDTO
                {
                    CourseId = course.Id.ToString(),
                    Tag = course.Tag,
                    Title = course.Title,
                    Summary = course.Summary,
                    Url = course.Url,
                    Level = course.Level,
                    DurationInMinutes = course.DurationInMinutes,
                    CreateDate = course.CreateDate,
                    LastUpdateDate = course.LastUpdateDate,
                    Active = course.Active,
                    Free = course.Free,
                    Featured = course.Featured,
                    AuthorId = course.AuthorId.ToString(),
                    CategoryId = course.CategoryId.ToString(),
                    AuthorName = null,
                    CategoryName = null,
                    Tags = course.Tags
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar curso por título {Title}", title);
                throw;
            }
        }

        public async Task<CourseResponseDTO> GetOneCourseByIdAsync(ObjectId id)
        {
            try
            {
                var guidId = new Guid(id.ToString());
                var filter = Builders<Course>.Filter.Eq(c => c.Id, guidId);
                var course = await _collection.Find(filter).FirstOrDefaultAsync();

                if (course == null) return null;

                return new CourseResponseDTO
                {
                    CourseId = course.Id.ToString(),
                    Tag = course.Tag,
                    Title = course.Title,
                    Summary = course.Summary,
                    Url = course.Url,
                    Level = course.Level,
                    DurationInMinutes = course.DurationInMinutes,
                    CreateDate = course.CreateDate,
                    LastUpdateDate = course.LastUpdateDate,
                    Active = course.Active,
                    Free = course.Free,
                    Featured = course.Featured,
                    AuthorId = course.AuthorId.ToString(),
                    CategoryId = course.CategoryId.ToString(),
                    AuthorName = null,
                    CategoryName = null,
                    Tags = course.Tags
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar curso por ID {Id}", id);
                throw;
            }
        }

        public async Task UpdateActiveCourseByTitleAsync(string title, bool active, DateTime lastUpdateDate)
        {
            try
            {
                var filter = Builders<Course>.Filter.Eq(c => c.Title, title);
                var update = Builders<Course>.Update
                    .Set(c => c.Active, active)
                    .Set(c => c.LastUpdateDate, lastUpdateDate);

                await _collection.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CourseResponseDTO>> GetCoursesByCategoryAsync(ObjectId categoryId)
        {
            try
            {
                var guidCategory = new Guid(categoryId.ToString());
                var filter = Builders<Course>.Filter.Eq(c => c.CategoryId, guidCategory)
                           & Builders<Course>.Filter.Eq(c => c.Active, true);

                var courses = await _collection.Find(filter)
                    .SortByDescending(c => c.CreateDate)
                    .ToListAsync();

                var result = new List<CourseResponseDTO>();
                foreach (var course in courses)
                {
                    var author = await _authorCollection.Find(a => a.Id == course.AuthorId).FirstOrDefaultAsync();
                    var category = await _categoryCollection.Find(cat => cat.Id == course.CategoryId).FirstOrDefaultAsync();

                    result.Add(new CourseResponseDTO
                    {
                        CourseId = course.Id.ToString(),
                        Tag = course.Tag,
                        Title = course.Title,
                        Summary = course.Summary,
                        Url = course.Url,
                        Level = course.Level,
                        DurationInMinutes = course.DurationInMinutes,
                        CreateDate = course.CreateDate,
                        LastUpdateDate = course.LastUpdateDate,
                        Active = course.Active,
                        Free = course.Free,
                        Featured = course.Featured,
                        AuthorId = course.AuthorId.ToString(),
                        CategoryId = course.CategoryId.ToString(),
                        AuthorName = author?.Name,
                        CategoryName = category?.Title,
                        Tags = course.Tags
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cursos por categoria {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<List<CourseResponseDTO>> GetCoursesByAuthorAsync(ObjectId authorId)
        {
            try
            {
                var guidAuthor = new Guid(authorId.ToString());
                var filter = Builders<Course>.Filter.Eq(c => c.AuthorId, guidAuthor)
                           & Builders<Course>.Filter.Eq(c => c.Active, true);

                var courses = await _collection.Find(filter)
                    .SortByDescending(c => c.CreateDate)
                    .ToListAsync();

                var result = new List<CourseResponseDTO>();
                foreach (var course in courses)
                {
                    var author = await _authorCollection.Find(a => a.Id == course.AuthorId).FirstOrDefaultAsync();
                    var category = await _categoryCollection.Find(cat => cat.Id == course.CategoryId).FirstOrDefaultAsync();

                    result.Add(new CourseResponseDTO
                    {
                        CourseId = course.Id.ToString(),
                        Tag = course.Tag,
                        Title = course.Title,
                        Summary = course.Summary,
                        Url = course.Url,
                        Level = course.Level,
                        DurationInMinutes = course.DurationInMinutes,
                        CreateDate = course.CreateDate,
                        LastUpdateDate = course.LastUpdateDate,
                        Active = course.Active,
                        Free = course.Free,
                        Featured = course.Featured,
                        AuthorId = course.AuthorId.ToString(),
                        CategoryId = course.CategoryId.ToString(),
                        AuthorName = author?.Name,
                        CategoryName = category?.Title,
                        Tags = course.Tags
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cursos por autor {AuthorId}", authorId);
                throw;
            }
        }

        public async Task UpdateCourseByTitleAsync(string title, CourseUpdateDTO update)
        {
            try
            {
                var filter = Builders<Course>.Filter.Eq(c => c.Title, title);

                var updateDef = Builders<Course>.Update
                    .Set(c => c.Free, update.Free)
                    .Set(c => c.Featured, update.Featured)
                    .Set(c => c.LastUpdateDate, DateTime.UtcNow);

                await _collection.UpdateOneAsync(filter, updateDef);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}