using System.Collections.Generic;
using System.Linq;

namespace TMDbLib.Objects.Discover
{
    public abstract class DiscoverBase
    {
        protected Dictionary<string, string> Parameters;

        public DiscoverBase()
        {
            Parameters = new Dictionary<string, string>();
        }

        internal List<KeyValuePair<string, string>> GetAllParameters()
        {
            return Parameters.ToList();
        }
    }
}