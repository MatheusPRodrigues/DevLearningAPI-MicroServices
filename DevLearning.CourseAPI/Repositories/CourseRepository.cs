using DevLearning.API.Models.DTOs.Course;
using DevLearning.CourseAPI.Data;
using DevLearning.CourseAPI.Repositories.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Category;
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
        private readonly ConnectionDB _connection;

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
                await _connection.GetMongoCollection().InsertOneAsync(course);
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
                var filter = Builders<Course>.Filter.Eq("CategoryId", category)
                           & Builders<Course>.Filter.Eq("Active", true);
                return (await _connection.GetMongoCollection().Find(filter).ToListAsync()).
                    Select(c => new CourseResponseDTO
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
                    AuthorName = c.AuthorId.ToString(),
                    CategoryName = c.CategoryId.ToString(),
                    Tags = c.Tags
                }).ToList();
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

        public async Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title)
        {
            try
            {
                var filter = Builders<Course>.Filter.Eq("Title", title);
                var course = await _connection.mongoCollection.Find(filter).FirstOrDefaultAsync();

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
                    AuthorName = course.AuthorId.ToString(),  
                    CategoryName = course.CategoryId.ToString(), 
                    Tags = course.Tags
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar curso por título {Title}", title);
                throw;
            }
        }

        public async Task<CourseResponseDTO> GetOneCourseByIdAsync(string id) 
        {
            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<Course>.Filter.Eq("_id", objectId);
                var course = await _connection.mongoCollection.Find(filter).FirstOrDefaultAsync();

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
                    AuthorName = course.AuthorId.ToString(),
                    CategoryName = course.CategoryId.ToString(),
                    Tags = course.Tags
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar curso por ID {Id}", id);
                throw;
            }
        }


        public async Task UpdateCourseByTitleAsync(string title, bool free, bool featured, DateTime lastUpdateDate)
        {
            try
            {
                var filter = Builders<Course>.Filter.Eq(c => c.Title, title);
                var update = Builders<Course>.Update
                    .Set(c => c.Free, free)
                    .Set(c => c.Featured, featured)
                    .Set(c => c.LastUpdateDate, lastUpdateDate);

                await _connection.GetMongoCollection().UpdateOneAsync(filter, update);
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
        public async Task UpdateActiveCourseByTitleAsync(string title, bool active, DateTime lastUpdateDate)
        {
            try
            {
                var filter = Builders<Course>.Filter.Eq(c => c.Title, title);
                var update = Builders<Course>.Update
                    .Set(c => c.Active, active)
                    .Set(c => c.LastUpdateDate, lastUpdateDate);

                await _connection.GetMongoCollection().UpdateOneAsync(filter, update);
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

        public async Task<List<CourseResponseDTO>> GetCoursesByCategoryAsync(ObjectId categoryId)
        {
            try
            {
                var filter = Builders<Course>.Filter.Eq("CategoryId", categoryId)
                           & Builders<Course>.Filter.Eq("Active", true);

                var courses = await _collection.Find(filter)
                    .SortByDescending(c => c.CreateDate)
                    .ToListAsync();

                var result = new List<CourseResponseDTO>();
                foreach (var course in courses)
                {
                    var author = await _authorCollection.Find(a => a.Id == course.AuthorId).FirstOrDefaultAsync();
                    var category = await _categoryCollection.Find(cat => cat.Id == course.CategoryId)
                        .FirstOrDefaultAsync();

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
                var filter = Builders<Course>.Filter.Eq("AuthorId", authorId)
                           & Builders<Course>.Filter.Eq("Active", true);

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
                        AuthorName = author?.Name,
                        CategoryName = category?.Title,
                        Tags = string.Join(", ", course.Tags)
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
    }
}
