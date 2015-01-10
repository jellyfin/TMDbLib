using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    public class ResultContainer<T>
    {
        public int Id { get; set; }
        public List<T> Results { get; set; }
    }
}