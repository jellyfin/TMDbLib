using System.Threading.Tasks;
﻿using System;
using RestSharp;
using TMDbLib.Objects.General;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Keyword> GetKeyword(int keywordId)
        {
            RestRequest req = new RestRequest("keyword/{keywordId}");
            req.AddUrlSegment("keywordId", keywordId.ToString());

            IRestResponse<Keyword> resp = await _client.ExecuteGetTaskAsync<Keyword>(req).ConfigureAwait(false);

            return resp.Data;
        }

        public async Task<SearchContainer<MovieResult>> GetKeywordMovies(int keywordId, int page = 0)
        {
            return await GetKeywordMovies(keywordId, DefaultLanguage, page);
        }

        public async Task<SearchContainer<MovieResult>> GetKeywordMovies(int keywordId, string language, int page = 0)
        {
            RestRequest req = new RestRequest("keyword/{keywordId}/movies");
            req.AddUrlSegment("keywordId", keywordId.ToString());

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);

            IRestResponse<SearchContainer<MovieResult>> resp = await _client.ExecuteGetTaskAsync<SearchContainer<MovieResult>>(req).ConfigureAwait(false);

            return resp.Data;
        }
    }
}