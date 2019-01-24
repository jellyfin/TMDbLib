using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;

namespace TMDbLibTests.TestFramework.HttpMocking
{
    internal class ResponseObject
    {
        public string ReducedUri { get; set; }
        public JObject RespData { get; set; }
        public JObject ReqData { get; set; }
        public HttpStatusCode RespStatusCode { get; set; }
        public string ReqMethod { get; set; }
        public Dictionary<string, string> RespHeaders { get; set; }
    }
}