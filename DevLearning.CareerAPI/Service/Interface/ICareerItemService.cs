using DevLearning.Models.DTOs.CareerItem;
using DevLearning.Models.Models.DTOs.CareerItem;

namespace DevLearning.CareerAPI.Service.Interface
{
    public interface ICareerItemService
    {
        Task<bool> CreateItemCareerAsync(CareerItemRequestCreateDTO careerItemDTO);
        Task<bool> DeleteItemCareerAsync(Guid careerId, string courseId);
        Task<bool> UpdateCareerItemAsync(Guid careerId, string courseId, CareerItemUpdateDTO updateDTO);
        Task<APICourseDTO> GetCourseFromExternalApi(string courseId);
    }
}
