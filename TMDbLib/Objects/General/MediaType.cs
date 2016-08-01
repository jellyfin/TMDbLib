using TMDbLib.Utilities;

namespace TMDbLib.Objects.General
{
    public enum MediaType
    {
        Unknown,

        [EnumValue("movie")]
        Movie = 1,

        [EnumValue("tv")]
        Tv = 2,

        [EnumValue("person")]
        Person = 3
    }
}