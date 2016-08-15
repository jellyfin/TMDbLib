using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMDbLib.Objects.Changes
{
    public class ChangeItemAdded : ChangeItemBase
    {
        public ChangeItemAdded()
        {
            Action = ChangeAction.Added;
        }

        [JsonProperty("value")]
        public JToken Value { get; set; }
    }
}