using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;

namespace TMDbLibTests.TestFramework.HttpMocking
{
    internal abstract class MockingHandlerBase : HttpMessageHandler
    {
        protected string GetReducedUri(Uri uri)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            NameValueCollection uriQueryString = HttpUtility.ParseQueryString(uriBuilder.Query);
            uriQueryString.Remove("api_key");

            uriBuilder.Query = uriQueryString.ToString();

            return uriBuilder.Uri.PathAndQuery;
        }
    }
}