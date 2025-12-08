using DevLearning.Models;
using DevLearning.Models.DTOs.CareerItem;

namespace DevLearning.API.Services
{
    public class CareerRequestDTO
    {
        public string Title { get; init; }
        public string Summary { get; init; }
        public int DurationInMinutes { get; init; }
        public string Tags { get; init; }
        public List<CareerItemRequestCreateDTO> careerItems {get; init;}
    }
}