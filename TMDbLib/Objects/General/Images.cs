using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    public class Images
    {
        public int Id { get; set; }
        public List<ImageData> Backdrops { get; set; }
        public List<ImageData> Posters { get; set; }
        public List<ImageData> Stills { get; set; }
    }
}