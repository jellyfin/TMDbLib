using System;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Search {
    public class SearchMulti {
        private string _name;
        private string _originalName;
        private MediaType _type;
        public int Id { get; set; }
        public string Title { set { _name = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string OriginalName { get { return _originalName; } set { _originalName = value; } }
        public string OriginalTitle { set { _originalName = value; } }
        public DateTime? FirstAirDate { get; set; }
        public string BackdropPath { get; set; }
        public string PosterPath { get; set; }
        public double Popularity { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public bool Adult { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public string MediaType {
            set {
                switch (value) {
                    case "tv":
                        _type = General.MediaType.TVShow;
                        break;
                    case "movie":
                        _type = General.MediaType.Movie;
                        break;
                    default:
                        _type = General.MediaType.Unknown;
                        break;
                }
            }
        }

        public MediaType Type { get { return _type; } set { _type = value; } }
    }
}
