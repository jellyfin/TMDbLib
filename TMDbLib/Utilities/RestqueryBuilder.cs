using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace TMDbLib.Utilities
{
    public class RestQueryBuilder
    {
        private string _url;
        //[DJ] private NameValueCollection _nvc;
        private List<KeyValuePair<string, string>> _nvc;

        public RestQueryBuilder(string url)
        {
            _url = url;
            //_nvc = new NameValueCollection();
            _nvc = new List<KeyValuePair<string, string>>();
        }

        public void AddParameter(string key, string value)
        {
            //_nvc[key] = value;
            _nvc.Add(new KeyValuePair<string, string>(key, value));
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

                //NameObjectCollectionBase.KeysCollection keys = _nvc.Keys;

                for (int i = 0; i < _nvc.Count; i++)
                {
                    if (i > 0)
                        urlBuilder.Append('&');

                    // TODO: Handle special characters (url-encode)
                    //urlBuilder.Append(keys[i] + "=" + _nvc[keys[i]]);
                    urlBuilder.Append(_nvc[i].Key + "=" + _nvc[i].Value);
                }
            }

            return urlBuilder.ToString();
        }
    }
}
