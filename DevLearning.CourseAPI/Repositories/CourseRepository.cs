using DevLearning.API.Models.DTOs.Course;
using DevLearning.CourseAPI.Data;
using DevLearning.CourseAPI.Repositories.Interfaces;
using DevLearning.Models;
using DevLearning.Models.DTOs.Category;
using MongoDB.Driver;

namespace DevLearning.CourseAPI.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ConnectionDB _connection;

        public CourseRepository(ConnectionDB connection)
        {
            _connection = connection;
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
                var filter = Builders<Course>.Filter.Eq(c => c.CategoryId, category);
                return (await _connection.GetMongoCollection().Find(filter).ToListAsync()).Select(c => new CourseResponseDTO
                {
                    CourseId = c.Id,
                    Tag = c.Tag,
                    Title = c.Title,
                    Summary = c.Summary,
                    Url = c.Url,
                    Level = c.Level,
                    Order = c.Order

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
                var filter = Builders<Course>.Filter.Eq(c => c.Title, title);
                return (await _connection.GetMongoCollection().Find(filter).FirstOrDefaultAsync()).Select(c => new CourseResponseDTO
                {
                    CourseId = c.Id,
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
                    AuthorId = c.AuthorId,
                    CategoryId = c.CategoryId,
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

        public async Task<CourseResponseDTO> GetOneCourseByIdAsync(Guid id)
        {
            var filter = Builders<Course>.Filter.Eq(c => c.Id, id);
            return (await _connection.GetMongoCollection().Find(filter).FirstOrDefaultAsync()).Select(c => new CourseResponseDTO
            {
                CourseId = c.Id,
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
                AuthorId = c.AuthorId,
                CategoryId = c.CategoryId,
                Tags = c.Tags
            }).ToList();
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
    }
}
