using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RestSharp;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Discover
{
    public abstract class DiscoverBase<T>
    {
        private readonly string _endpoint;
        private readonly TMDbClient _client;
        protected NameValueCollection Parameters;

        public DiscoverBase(string endpoint, TMDbClient client)
        {
            _endpoint = endpoint;
            _client = client;
            Parameters = new NameValueCollection();
        }

        public SearchContainer<T> Query(int page = 0)
        {
            return Query(_client.DefaultLanguage, page);
        }

        public SearchContainer<T> Query(string language, int page = 0)
        {
            return _client.DiscoverPerform<T>(_endpoint, language, page, Parameters);
        }
    }
}