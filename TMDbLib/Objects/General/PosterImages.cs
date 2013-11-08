using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    public class PosterImages
    {
        public int Id { get; set; }
        public List<ImageData> Posters { get; set; }
    }
}