using DevLearning.Models.DTOs.CareerItem;

namespace DevLearning.API.Models.DTOs.Carrer
{
    public class CareerWhitCareerItemResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public int DurationInMinutes { get; set; }
        public bool Active { get; set; }
        public bool Featured { get; set; }
        public string Tags { get; set; }

        public List<CareerItemResponseDTO> Items { get; set; } = new();
    }
}
