using System;
using System.Globalization;

namespace TMDbLib.Objects.Movies
{
    public class ChangeItem
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public DateTime Time { get; set; }
        public string Iso_639_1 { get; set; }
        public string Value { get; set; }
    }
}