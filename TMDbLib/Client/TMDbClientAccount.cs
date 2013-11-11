using System;
using System.Globalization;
using RestSharp;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Will retrieve the details of the account associated with the current session id
        /// </summary>
        /// <exception cref="UserSessionRequiredException">Thrown when the current client object doens't have a user session assigned.</exception>
        /// <exception cref="UnauthorizedAccessException">Can be thrown if either to provided API key is invalid or if the provided session id does not grant to required access</exception>
        public AccountDetails AccountGetDetails()
        {
           RequireSessionId(SessionType.UserSession);

            var request = new RestRequest("account");
            request.AddParameter("session_id", SessionId);

            IRestResponse<AccountDetails> response = _client.Get<AccountDetails>(request);

            return response.Data;
        }

        // TODO reimplement when API dev has cleared up confusion around account id url segment
        /// <summary>
        /// Retrieve all lists associated with the provided account id
        /// This can be lists that were created by the user or lists he marked as favorite
        /// </summary>
        /// <param name="accountId">The account identifier of the person who's lists you wish to retrieve</param>
        /// <param name="sessionId"></param>
        //public SearchContainer<List> AccountGetLists(int accountId, string sessionId)
        //{
        //    RestRequest req = new RestRequest("account/1/lists");
        //    //req.AddUrlSegment("accountId", );
        //    req.AddParameter("session_id", sessionId);

        //    IRestResponse<SearchContainer<List>> resp = _client.Get<SearchContainer<List>>(req);

        //    return resp.Data;
        //}
    }
}
