using System;
using System.Globalization;

namespace TMDbLib.Objects.Search
{
    public class SearchMovie
    {
        public int Id { get; set; }
        public bool Adult { get; set; }
        public string BackdropPath { get; set; }
        public string OriginalTitle { get; set; }
        public string Release_Date { get; set; }
        public string PosterPath { get; set; }
        public double Popularity { get; set; }
        public string Title { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }

        public DateTime ReleaseDate
        {
            get
            {
                DateTime dt;
                DateTime.TryParseExact(Release_Date, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dt);

                return dt;
            }
        }
    }
}