using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMDbLib.Objects.Changes
{
    public class ChangeItemDestroyed : ChangeItemBase
    {
        public ChangeItemDestroyed()
        {
            Action = ChangeAction.Destroyed;
        }

        [JsonProperty("value")]
        public JToken Value { get; set; }
    }
}