namespace DevLearning.Models
{
    public class CareerItem
    {
        public Guid CareerId { get; set; }
        public string CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Order { get; set; }


        public CareerItem() { }

        public CareerItem(Guid careerId, string courseId, string title, string description, byte order)
        {
            CareerId = careerId;
            CourseId = courseId;
            Title = title;
            Description = description;
            Order = order;
        }
    }




}
