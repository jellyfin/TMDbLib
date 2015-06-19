namespace TMDbLib.Objects.Movies
{
    public class Cast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Character { get; set; }
        public string CreditId { get; set; }
        public int Order { get; set; }
        public int CastId { get; set; }
        public string ProfilePath { get; set; }
    }
}