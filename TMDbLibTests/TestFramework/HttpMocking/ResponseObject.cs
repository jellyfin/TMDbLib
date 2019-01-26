using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;

namespace TMDbLibTests.TestFramework.HttpMocking
{
    internal class ResponseObject
    {
        public DateTime? RecordedAt { get; set; }
        public string ReducedUri { get; set; }
        public JToken RespData { get; set; }
        public JToken ReqData { get; set; }
        public HttpStatusCode RespStatusCode { get; set; }
        public string ReqMethod { get; set; }
        public Dictionary<string, string> RespHeaders { get; set; }
    }
}