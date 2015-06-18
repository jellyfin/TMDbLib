using System.Collections.Generic;

namespace TMDbLib.Objects.Discover
{
    public abstract class DiscoverBase
    {
        protected Dictionary<string, string> Parameters;

        public DiscoverBase()
        {
            Parameters = new Dictionary<string, string>();
        }
    }
}