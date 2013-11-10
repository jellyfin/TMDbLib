using System;
using RestSharp;
using TMDbLib.Objects.Lists;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public List GetList(string listId)
        {
            if (string.IsNullOrWhiteSpace(listId))
                throw new ArgumentNullException("listId");

            var request = new RestRequest("list/{listId}");
            request.AddUrlSegment("listId", listId);

            request.DateFormat = "yyyy-MM-dd";

            IRestResponse<List> response = _client.Get<List>(request);

            return response.Data;
        }

        public bool GetListIsMoviePresent(string listId, int movieId)
        {
            if (string.IsNullOrWhiteSpace(listId))
                throw new ArgumentNullException("listId");

            if (movieId <= 0)
                throw new ArgumentOutOfRangeException("movieId");

            var request = new RestRequest("list/{listId}/item_status");
            request.AddUrlSegment("listId", listId);
            request.AddParameter("movie_id", movieId);

            IRestResponse<ListStatus> response = _client.Get<ListStatus>(request);

            return response.Data.ItemPresent;
        }

        public string ListCreate(string sessionId, string name, string description = "", string language = null)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentNullException("sessionId");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            // Description is expected by the API and can not be null
            if (string.IsNullOrWhiteSpace(description))
                description = "";

            var request = new RestRequest("list") { RequestFormat = DataFormat.Json };
            request.AddParameter("session_id", sessionId, ParameterType.QueryString);
            if (string.IsNullOrWhiteSpace(language))
            {
                request.AddBody(new { name = name, description = description });
            }
            else
            {
                request.AddBody(new { name = name, description = description, language = language });
            }

            IRestResponse<ListPostReply> response = _client.Post<ListPostReply>(request);

            return response.Data == null ? null : response.Data.ListId;
        }

        public bool ListDelete(string sessionId, string listId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentNullException("sessionId");

            if (string.IsNullOrWhiteSpace(listId))
                throw new ArgumentNullException("listId");

            var request = new RestRequest("list/{listId}");
            request.AddUrlSegment("listId", listId);
            request.AddParameter("session_id", sessionId, ParameterType.QueryString);

            IRestResponse<ListPostReply> response = _client.Delete<ListPostReply>(request);

            // Status code 13 = success
            return response.Data != null && response.Data.StatusCode == 13;
        }

        public bool ListAddMovie(string sessionId, string listId, int movieId)
        {
            return ManipulateMediaList(sessionId, listId, movieId, "add_item");
        }

        public bool ListRemoveMovie(string sessionId, string listId, int movieId)
        {
            return ManipulateMediaList(sessionId, listId, movieId, "remove_item");
        }

        private bool ManipulateMediaList(string sessionId, string listId, int movieId, string method)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentNullException("sessionId");

            if (string.IsNullOrWhiteSpace(listId))
                throw new ArgumentNullException("listId");

            // Description is expected by the API and can not be null
            if (movieId <= 0)
                throw new ArgumentOutOfRangeException("movieId");

            var request = new RestRequest("list/{listId}/{method}") { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("listId", listId);
            request.AddUrlSegment("method", method);
            request.AddParameter("session_id", sessionId, ParameterType.QueryString);
            request.AddBody(new { media_id = movieId });

            IRestResponse<ListPostReply> response = _client.Post<ListPostReply>(request);

            // Status code 8 = "Duplicate entry - The data you tried to submit already exists"
            // Status code 12 = "The item/record was updated successfully"
            // Status code 13 = "The item/record was deleted successfully"
            return response.Data != null && (response.Data.StatusCode == 12 || response.Data.StatusCode == 13);
        }
    }
}