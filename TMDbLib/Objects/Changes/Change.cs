using System.Collections.Generic;

namespace TMDbLib.Objects.Changes
{
    public class Change
    {
        public string Key { get; set; }
        public List<ChangeItem> Items { get; set; }
    }
}