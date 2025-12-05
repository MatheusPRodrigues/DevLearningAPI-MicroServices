namespace DevLearning.Models.DTOs.Category
{
    public class CategoryRequestDTO
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
    }
}
