using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Reviews
{
    public class Review
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }

        public string Iso_639_1 { get; set; }

        public int MediaId { get; set; }
        public string MediaTitle { get; set; }
        public MediaType MediaType { get; set; }

        public string Url { get; set; }
    }
}
