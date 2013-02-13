using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Person
{
    public class ProfileImages
    {
        public int Id { get; set; }
        public List<Profile> Profiles { get; set; }
    }
}