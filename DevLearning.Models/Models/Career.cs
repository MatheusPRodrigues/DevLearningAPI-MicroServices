namespace DevLearning.Models
{
    public class Career
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public string url { get; private set; }
        public int DurationInMinutes { get; private set; }
        public bool Active { get; private set; }
        public bool Featured { get; private set; }
        public string Tags { get; private set; }
        public List<CareerItem> items { get; private set; } = new();

        public Career() { }

        public Career(string title, string summary, string url, int durationInMinutes, string tags)
        {
            Id = Guid.NewGuid();
            this.Title = title;
            this.Summary = summary;
            this.url = url;
            this.DurationInMinutes = durationInMinutes;
            this.Active = true;
            Featured = false;
            Tags = tags;
        }

        public void AddItem(CareerItem item)
        {
            items.Add(item);
        }
    }
}
