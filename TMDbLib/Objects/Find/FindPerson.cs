using Newtonsoft.Json;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Find;

public class FindPerson : SearchPerson
{
    [JsonProperty("gender")]
    public PersonGender Gender { get; set; }

    [JsonProperty("known_for_department")]
    public string KnownForDepartment { get; set; }
}
