using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Collections
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PosterPath { get; set; }
        public string BackdropPath { get; set; }
        public List<Part> Parts { get; set; }
        public Images Images { get; set; }
    }
}