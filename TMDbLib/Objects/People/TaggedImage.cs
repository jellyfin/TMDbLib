using TMDbLib.Objects.General;

namespace TMDbLib.Objects.People
{
    public class TaggedImage
    {
        public double AspectRatio { get; set; }
        public string FilePath { get; set; }
        public int Height { get; set; }
        public string Id { get; set; }
        public string Iso_639_1 { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public int Width { get; set; }
        public string ImageType { get; set; }       // TODO: Turn into enum
        public Media Media { get; set; }
        public MediaType MediaType { get; set; }
    }
}