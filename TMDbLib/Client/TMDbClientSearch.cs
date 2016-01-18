using System.Threading.Tasks;
﻿using System;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        private async Task<T> SearchMethod<T>(string method, string query, int page, string language = null, bool? includeAdult = null, int year = 0, string dateFormat = null) where T : new()
        {
            TmdbRestRequest req = _client2.Create("search/{method}");
            req.AddUrlSegment("method", method);
            req.AddParameter("query", query);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (year >= 1)
                req.AddParameter("year", year.ToString());
            if (includeAdult.HasValue)
                req.AddParameter("include_adult", includeAdult.Value ? "true" : "false");

            // TODO: Dateformat?
            //if (dateFormat != null)
            //    req.DateFormat = dateFormat;

            TmdbRestResponse<T> resp = await req.ExecuteGet<T>().ConfigureAwait(false);

            return resp;
        }

        public async Task<SearchContainer<SearchMovie>> SearchMovie(string query, int page = 0, bool includeAdult = false, int year = 0)
        {
            return await SearchMovie(query, DefaultLanguage, page, includeAdult, year);
        }

        public async Task<SearchContainer<SearchMovie>> SearchMovie(string query, string language, int page = 0, bool includeAdult = false, int year = 0)
        {
            return await SearchMethod<SearchContainer<SearchMovie>>("movie", query, page, language, includeAdult, year, "yyyy-MM-dd");
        }

        public async Task<SearchContainer<SearchMulti>> SearchMulti(string query, int page = 0, bool includeAdult = false, int year = 0)
        {
            return await SearchMulti(query, DefaultLanguage, page, includeAdult, year);
        }

        public async Task<SearchContainer<SearchMulti>> SearchMulti(string query, string language, int page = 0, bool includeAdult = false, int year = 0)
        {
            return await SearchMethod<SearchContainer<SearchMulti>>("multi", query, page, language, includeAdult, year, "yyyy-MM-dd");
        }

        public async Task<SearchContainer<SearchResultCollection>> SearchCollection(string query, int page = 0)
        {
            return await SearchCollection(query, DefaultLanguage, page);
        }

        public async Task<SearchContainer<SearchResultCollection>> SearchCollection(string query, string language, int page = 0)
        {
            return await SearchMethod<SearchContainer<SearchResultCollection>>("collection", query, page, language);
        }

        public async Task<SearchContainer<SearchPerson>> SearchPerson(string query, int page = 0, bool includeAdult = false)
        {
            return await SearchMethod<SearchContainer<SearchPerson>>("person", query, page, includeAdult: includeAdult);
        }

        public async Task<SearchContainer<SearchList>> SearchList(string query, int page = 0, bool includeAdult = false)
        {
            return await SearchMethod<SearchContainer<SearchList>>("list", query, page, includeAdult: includeAdult);
        }

        public async Task<SearchContainer<SearchCompany>> SearchCompany(string query, int page = 0)
        {
            return await SearchMethod<SearchContainer<SearchCompany>>("company", query, page);
        }

        public async Task<SearchContainer<SearchKeyword>> SearchKeyword(string query, int page = 0)
        {
            return await SearchMethod<SearchContainer<SearchKeyword>>("keyword", query, page);
        }

        public async Task<SearchContainer<SearchTv>> SearchTvShow(string query, int page = 0)
        {
            return await SearchMethod<SearchContainer<SearchTv>>("tv", query, page);
        }
    }
}