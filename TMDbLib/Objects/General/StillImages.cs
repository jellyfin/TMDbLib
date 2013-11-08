using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    public class StillImages
    {
        public int Id { get; set; }
        public List<ImageData> Stills { get; set; }
    }
}