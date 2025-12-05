namespace DevLearning.Models
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Url { get; private set; }
        public string Summary { get; private set; }
        public int Order { get; private set; }
        public string Description { get; private set; }
        public bool Featured { get; private set; }

        public Category(string title, string url, string summary, int order, string description, bool featured)
        {
            Id = Guid.NewGuid();
            Title = title;
            Url = url;
            Summary = summary;
            Order = order;
            Description = description;
            Featured = featured;
        }

        public Category(Guid id, string title, string url, string summary, int order, string description, bool featured)
        {
            Id = id;
            Title = title;
            Url = url;
            Summary = summary;
            Order = order;
            Description = description;
            Featured = featured;
        }

        public Category() { }


        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length > 160)
                throw new ArgumentException("Titulo inválido (máximo 160 caracteres).");

            Title = title;
        }

        public void SetUrl(string url)
        {
            Url = url;
        }

        public void SetSummary(string summary)
        {
            if (string.IsNullOrWhiteSpace(summary) || summary.Length > 2000)
                throw new ArgumentException("Summary inválido (máximo 2000 caracteres).");

            Summary = summary;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição inválida.");

            Description = description;
        }

        public void SetFeatured(bool featured)
        {
            Featured = featured;
        }

        public void SetOrder(int order)
        {
            if (order < 0)
                throw new ArgumentException("Ordem deve ser >= 0.");

            Order = order;
        }
    }
}
