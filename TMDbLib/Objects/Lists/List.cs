using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Lists
{
    public class List
    {
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public int FavoriteCount { get; set; }
        public string Id { get; set; }
        public List<MovieResult> Items { get; set; }
        public int ItemCount { get; set; }
        /// <summary>
        /// The Language iso code of a language the list is targeting. Ex en
        /// </summary>
        public string Iso_639_1 { get; set; }
        public string Name { get; set; }
        public string PosterPath { get; set; }
    }
}