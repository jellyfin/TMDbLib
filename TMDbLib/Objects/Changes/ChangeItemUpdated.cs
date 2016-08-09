using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMDbLib.Objects.Changes
{
    public class ChangeItemUpdated : ChangeItemBase
    {
        public ChangeItemUpdated()
        {
            Action = ChangeAction.Updated;
        }

        /// <summary>
        /// This field is not always set
        /// </summary>
        [JsonProperty("original_value")]
        public JToken OriginalValue { get; set; }

        /// <summary>
        /// This field is not always set
        /// </summary>
        [JsonProperty("value")]
        public JToken Value { get; set; }
    }
}