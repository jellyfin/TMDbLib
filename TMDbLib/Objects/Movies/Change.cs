using System.Collections.Generic;

namespace TMDbLib.Objects.Movies
{
    public class Change
    {
        public string Key { get; set; }
        public List<ChangeItem> Items { get; set; }
    }
}