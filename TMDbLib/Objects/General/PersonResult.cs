using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    public class PersonResult
    {
        public bool Adult { get; set; }
        public int Id { get; set; }
        public List<MediaKnownFor> KnownFor { get; set; }
        public string Name { get; set; }
        public string Bson_Id { get; set; }
        public string ProfilePath { get; set; }
        public string Url { get; set; }
    }
}