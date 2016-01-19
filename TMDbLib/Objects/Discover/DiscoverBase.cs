using System.Collections.Specialized;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;

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

        public async Task<SearchContainer<T>> Query(int page = 0)
        {
            return await Query(_client.DefaultLanguage, page).ConfigureAwait(false);
        }

        public async Task<SearchContainer<T>> Query(string language, int page = 0)
        {
            return await _client.DiscoverPerform<T>(_endpoint, language, page, Parameters).ConfigureAwait(false);
        }
    }
}