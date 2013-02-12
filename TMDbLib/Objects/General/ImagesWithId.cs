using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    public class ImagesWithId
    {
        public int Id { get; set; }
        public List<Backdrop> Backdrops { get; set; }
        public List<Poster> Posters { get; set; }
    }
}