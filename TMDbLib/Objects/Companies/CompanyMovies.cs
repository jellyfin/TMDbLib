using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Companies
{
    public class CompanyMovies
    {
        public int id { get; set; }
        public int page { get; set; }
        public List<MovieResult> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}