namespace TMDbLib.Objects.General
{
    public class ProfileImage
    {
        public string FilePath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Iso_639_1 { get; set; }
        public double AspectRatio { get; set; }
        public int VoteCount { get; set; }
        public double VoteAverage { get; set; }
        public string Id { get; set; }
    }
}