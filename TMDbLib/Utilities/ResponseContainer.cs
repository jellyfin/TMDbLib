using System.Collections.Specialized;
using System.Net.Http.Headers;

namespace TMDbLib.Utilities
{
    public class ResponseContainer<T>
    {
        public T Data { get; set; }
        public HttpResponseHeaders Headers { get; set; }
        public string Content { get; set; }
    }
}
