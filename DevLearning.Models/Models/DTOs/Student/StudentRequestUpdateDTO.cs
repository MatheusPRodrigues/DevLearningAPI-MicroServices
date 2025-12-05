namespace DevLearning.Models.DTOs.Student
{
    public class StudentRequestUpdateDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Document { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
