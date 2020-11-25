namespace ImageApi.Infrastructure.Options
{
    public class ElasticSearchOptions
    {
        public string[] URIs { get; set; }
        public string ImageIndex { get; set; } = "images";
    }
}