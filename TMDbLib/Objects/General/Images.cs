using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    public class Images
    {
        public List<ImageData> Backdrops { get; set; }
        public List<ImageDataWithId> Posters { get; set; }
    }
}