using DevLearning.API.Models.DTOs.Carrer;
using DevLearning.API.Services;
using DevLearning.Models.DTOs.Carrer;
using DevLearning.Models.Models.DTOs.CareerItem;

namespace DevLearning.CareerAPI.Service.Interface
{
    public interface ICareerService
    {
        Task CreateCareerAsync(CareerRequestDTO careerDTO);
        Task<List<CareerWhitCareerItemResponseDTO>> GetAllCareerAsync();
        Task<CareerWhitCareerItemResponseDTO?> GetCareerByIdAsync(Guid careerId);
        Task<bool> DeleteCareerAsync(Guid careerId);
        Task<bool> UpdateCareerAsync(Guid id, CareerUpdateDTO updateDTO);
        Task FillCourseAttributes(List<CareerWhitCareerItemResponseDTO> careers);
        Task<APICourseDTO> GetCourseFromExternalApi(string courseId);

    }
}
