using System;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Authentication;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Retrieve a list by it's id
        /// </summary>
        /// <param name="listId">The id of the list you want to retrieve</param>
        public List GetList(string listId)
        {
            if (string.IsNullOrWhiteSpace(listId))
                throw new ArgumentNullException("listId");

            RestQueryBuilder request = new RestQueryBuilder("list/{listId}");
            request.AddUrlSegment("listId", listId);

            //request.DateFormat = "yyyy-MM-dd";

            ResponseContainer<List> response = _client.Get<List>(request);

            return response.Data;
        }

        /// <summary>
        /// Will check if the provided movie id is present in the specified list
        /// </summary>
        /// <param name="listId">Id of the list to check in</param>
        /// <param name="movieId">Id of the movie to check for in the list</param>
        public bool GetListIsMoviePresent(string listId, int movieId)
        {
            if (string.IsNullOrWhiteSpace(listId))
                throw new ArgumentNullException("listId");

            if (movieId <= 0)
                throw new ArgumentOutOfRangeException("movieId");

            RestQueryBuilder request = new RestQueryBuilder("list/{listId}/item_status");
            request.AddUrlSegment("listId", listId);
            request.AddParameter("movie_id", movieId.ToString());

            ResponseContainer<ListStatus> response = _client.Get<ListStatus>(request);

            return response.Data.ItemPresent;
        }

        /// <summary>
        /// Creates a new list for the user associated with the current session
        /// </summary>
        /// <param name="name">The name of the new list</param>
        /// <param name="description">Optional description for the list</param>
        /// <param name="language">Optional language that might indicate the language of the content in the list</param>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public string ListCreate(string name, string description = "", string language = null)
        {
            RequireSessionId(SessionType.UserSession);

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            // Description is expected by the API and can not be null
            if (string.IsNullOrWhiteSpace(description))
                description = "";

            RestQueryBuilder request = new RestQueryBuilder("list");
            request.AddParameter("session_id", SessionId);

            ResponseContainer<ListCreateReply> response;
            if (string.IsNullOrWhiteSpace(language))
            {
                var body = new { name = name, description = description };
                response = _client.Post<ListCreateReply>(request, body);
            }
            else
            {
                var body = new { name = name, description = description, language = language };
                response = _client.Post<ListCreateReply>(request, body);
            }

            return response.Data == null ? null : response.Data.ListId;
        }

        /// <summary>
        /// Deletes the specified list that is owned by the user
        /// </summary>
        /// <param name="listId">A list id that is owned by the user associated with the current session id</param>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public bool ListDelete(string listId)
        {
            RequireSessionId(SessionType.UserSession);

            if (string.IsNullOrWhiteSpace(listId))
                throw new ArgumentNullException("listId");

            RestQueryBuilder request = new RestQueryBuilder("list/{listId}");
            request.AddUrlSegment("listId", listId);
            request.AddParameter("session_id", SessionId);

            ResponseContainer<PostReply> response = _client.Delete<PostReply>(request);

            // Status code 13 = success
            return response.Data != null && response.Data.StatusCode == 13;
        }

        /// <summary>
        /// Adds a movie to a specified list
        /// </summary>
        /// <param name="listId">The id of the list to add the movie to</param>
        /// <param name="movieId">The id of the movie to add</param>
        /// <returns>True if the method was able to add the movie to the list, will retrun false in case of an issue or when the movie was already added to the list</returns>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public bool ListAddMovie(string listId, int movieId)
        {
            return ManipulateMediaList(listId, movieId, "add_item");
        }

        /// <summary>
        /// Removes a movie from the specified list
        /// </summary>
        /// <param name="listId">The id of the list to add the movie to</param>
        /// <param name="movieId">The id of the movie to add</param>
        /// <returns>True if the method was able to remove the movie from the list, will retrun false in case of an issue or when the movie was not present in the list</returns>
        /// <remarks>Requires a valid user session</remarks>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        public bool ListRemoveMovie(string listId, int movieId)
        {
            return ManipulateMediaList(listId, movieId, "remove_item");
        }

        private bool ManipulateMediaList(string listId, int movieId, string method)
        {
            RequireSessionId(SessionType.UserSession);

            if (string.IsNullOrWhiteSpace(listId))
                throw new ArgumentNullException("listId");

            // Description is expected by the API and can not be null
            if (movieId <= 0)
                throw new ArgumentOutOfRangeException("movieId");

            RestQueryBuilder request = new RestQueryBuilder("list/{listId}/{method}");
            request.AddUrlSegment("listId", listId);
            request.AddUrlSegment("method", method);
            request.AddParameter("session_id", SessionId);

            var body = new { media_id = movieId };

            ResponseContainer<PostReply> response = _client.Post<PostReply>(request, body);

            // Status code 8 = "Duplicate entry - The data you tried to submit already exists"
            // Status code 12 = "The item/record was updated successfully"
            // Status code 13 = "The item/record was deleted successfully"
            return response.Data != null && (response.Data.StatusCode == 12 || response.Data.StatusCode == 13);
        }
    }
}