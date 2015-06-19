namespace TMDbLib.Objects.General
{
    public class ExternalIds
    {
        public int Id { get; set; }
        public string ImdbId { get; set; }
        public string FreebaseId { get; set; }
        public string FreebaseMid { get; set; }
        public int? TvdbId { get; set; }
        public int? TvrageId { get; set; }
    }
}