using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Search
{
    public class SearchMulti
    {
        public int Id { get; set; }
        public string Title { set { Name = value; } }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string OriginalTitle { set { OriginalName = value; } }
        public DateTime? FirstAirDate { get; set; }
        public string BackdropPath { get; set; }
        public string PosterPath { get; set; }
        public double Popularity { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public bool Adult { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public MediaType Type { get; set; }
        public List<int> GenreIds { get; set; }
        public List<string> OriginCountry { get; set; }

        public string MediaType
        {
            set
            {
                switch (value)
                {
                    case "tv":
                        Type = General.MediaType.TVShow;
                        break;
                    case "movie":
                        Type = General.MediaType.Movie;
                        break;
                    default:
                        Type = General.MediaType.Unknown;
                        break;
                }
            }
        }
    }
}
