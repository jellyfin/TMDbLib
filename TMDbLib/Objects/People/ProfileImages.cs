using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.People
{
    public class ProfileImages
    {
        public int Id { get; set; }
        public List<ProfileImage> Profiles { get; set; }
    }
}