namespace TMDbLib.Objects.Movies
{
    public class ChangeItem
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public string Time { get; set; }        // TODO: Datetype
        public string Iso_639_1 { get; set; }
        public string Value { get; set; }
    }
}