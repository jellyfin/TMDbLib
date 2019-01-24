using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMDbLibTests.Exceptions;

namespace TMDbLibTests.TestFramework.HttpMocking
{
    internal class ResponseContainer
    {
        private readonly JsonSerializer _serializer;
        private readonly List<ResponseObject> _responses;

        public ResponseContainer()
        {
            _responses = new List<ResponseObject>();
            _serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public void Save(string file)
        {
            lock (_responses)
            {
                IOrderedEnumerable<ResponseObject> data = _responses
                    .OrderBy(s => s.ReducedUri)
                    .ThenBy(s => s.ReqMethod)
                    .ThenBy(s => s.RespStatusCode);

                using (StreamWriter sw = new StreamWriter(file))
                using (JsonTextWriter tw = new JsonTextWriter(sw))
                    _serializer.Serialize(tw, data);
            }
        }

        public void Load(string file)
        {
            if (!File.Exists(file))
                return;

            lock (_responses)
            {
                using (StreamReader sr = new StreamReader(file))
                using (JsonTextReader tr = new JsonTextReader(sr))
                    _responses.AddRange(_serializer.Deserialize<IEnumerable<ResponseObject>>(tr));
            }
        }

        public void AddResponse(ResponseObject responseObject)
        {
            lock (_responses)
            {
                Predicate<ResponseObject> removals = s => s.ReducedUri == responseObject.ReducedUri && s.ReqMethod == responseObject.ReqMethod;

                if (responseObject.ReqData != null)
                {
                    Predicate<ResponseObject> tmp = removals;
                    removals = s => tmp(s) && JToken.DeepEquals(s.ReqData, responseObject.ReqData);
                }

                _responses.RemoveAll(removals);
                _responses.Add(responseObject);
            }
        }

        public ResponseObject FindResponse(string reducedUri, string method, JObject requestObject)
        {
            lock (_responses)
            {
                IEnumerable<ResponseObject> applicables = _responses.Where(s =>
                    s.ReducedUri.Equals(reducedUri, StringComparison.OrdinalIgnoreCase) &&
                    s.ReqMethod.Equals(method, StringComparison.OrdinalIgnoreCase));

                if (requestObject != null)
                    applicables = applicables.Where(s => JToken.DeepEquals(s.ReqData, requestObject));

                var count = applicables.Count();
                if (count == 0)
                    throw new TestFrameworkException($"There was no matching mocked response for {method} '{reducedUri}' {(requestObject == null ? "" : "with an accompanying request body")}");

                if (count > 1)
                    throw new TestFrameworkException($"There were {count} matching response to replay");

                return applicables.First();
            }
        }
    }
}