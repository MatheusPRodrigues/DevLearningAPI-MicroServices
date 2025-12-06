using Dapper;
using DevLearning.Models;

namespace DevLearning.CareerAPI.Repository.Interface
{
    public interface ICareerItemRepository
    {
        Task CreateCareerItemAsync(CareerItem career);
        Task<bool> GetCareerItemByIdAsync(Guid CareerId, string CourseId);
        Task<bool> UpdateCareerItemAsync(Guid CareerId, string CourseId, List<string> updates, DynamicParameters parameters);
        Task<bool> DeleteCareerItemAsync(Guid CareerId, string CourseId);
        Task<bool> GetCareerItemByTitleAsync(string title);
        Task<bool> ExistingCarrerItemwhitOrder(byte order);
    }
}
