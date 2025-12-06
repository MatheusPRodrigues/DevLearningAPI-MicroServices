namespace DevLearning.CareerAPI.Service.Interface
{
    public interface ICareerItemService
    {
        Task<bool> CreateItemCareerAsync(CareerItemRequestCreateDTO careerItemDTO);
        Task<bool> DeleteItemCareerAsync(Guid careerId, Guid courseId);
        Task<bool> UpdateCareerItemAsync(Guid careerId, Guid courseId, CareerItemUpdateDTO updateDTO);
    }
}
