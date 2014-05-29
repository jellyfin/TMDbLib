using System.Collections.Specialized;
using System.Text;

namespace TMDbLib.Utilities
{
    public class RestQueryBuilder
    {
        private string _url;
        private NameValueCollection _nvc;

        public RestQueryBuilder(string url)
        {
            _url = url;
            _nvc = new NameValueCollection();
        }

        public void AddParameter(string key, string value)
        {
            _nvc[key] = value;
        }

        public void AddUrlSegment(string key, string value)
        {
            _url = _url.Replace("{" + key + "}", value);
        }

        public string GetUri()
        {
            StringBuilder urlBuilder = new StringBuilder(_url);

            if (_nvc.Count > 0)
            {
                urlBuilder.Append('?');

                NameObjectCollectionBase.KeysCollection keys = _nvc.Keys;

                for (int i = 0; i < keys.Count; i++)
                {
                    if (i > 0)
                        urlBuilder.Append('&');

                    // TODO: Handle special characters (url-encode)
                    urlBuilder.Append(keys[i] + "=" + _nvc[keys[i]]);
                }
            }

            return urlBuilder.ToString();
        }
    }
}
