using System;
using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    [Serializable]
    public class TMDbConfig
    {
        public ConfigImageTypes Images { get; set; }
        public List<string> ChangeKeys { get; set; }
    }
}