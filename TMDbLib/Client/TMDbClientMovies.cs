using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Movie GetMovie(int id, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return GetMovie(id, DefaultLanguage, extraMethods);
        }

        public Movie GetMovie(string id, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return GetMovie(id, DefaultLanguage, extraMethods);
        }

        public Movie GetMovie(string id, string language, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            RestRequest req = new RestRequest("movie/{id}");
            req.AddUrlSegment("id", id.ToString());

            if (language != null)
                req.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(MovieMethods))
                                             .OfType<MovieMethods>()
                                             .Except(new[] { MovieMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            IRestResponse<Movie> resp = _client.Get<Movie>(req);

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (resp.Data != null)
            {
                if (resp.Data.Trailers != null)
                    resp.Data.Trailers.Id = resp.Data.Id;

                if (resp.Data.AlternativeTitles != null)
                    resp.Data.AlternativeTitles.Id = resp.Data.Id;

                if (resp.Data.Casts != null)
                    resp.Data.Casts.Id = resp.Data.Id;

                if (resp.Data.Releases != null)
                    resp.Data.Releases.Id = resp.Data.Id;

                if (resp.Data.Keywords != null)
                    resp.Data.Keywords.Id = resp.Data.Id;

                if (resp.Data.Translations != null)
                    resp.Data.Translations.Id = resp.Data.Id;
            }

            return resp.Data;
        }

        public Movie GetMovie(int id, string language, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return GetMovie(id.ToString(), language, extraMethods);
        }

        private T GetMovieMethod<T>(string id, MovieMethods movieMethod, string dateFormat = null, string country = null,
                                    string language = null, int page = -1, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            RestRequest req = new RestRequest("movie/{id}/{method}");
            req.AddUrlSegment("id", id);
            req.AddUrlSegment("method", movieMethod.GetDescription());

            if (dateFormat != null)
                req.DateFormat = dateFormat;

            if (country != null)
                req.AddParameter("country", country);
            if (language != null)
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);
            if (startDate.HasValue)
                req.AddParameter("start_date", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                req.AddParameter("end_date", endDate.Value.ToString("yyyy-MM-dd"));

            IRestResponse<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        private T GetMovieMethod<T>(int id, MovieMethods movieMethod, string dateFormat = null, string country = null,
                                    string language = null, int page = -1, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            return GetMovieMethod<T>(id.ToString(), movieMethod, dateFormat, country, language, page, startDate, endDate);
        }

        public AlternativeTitles GetMovieAlternativeTitles(int id)
        {
            return GetMovieAlternativeTitles(id, DefaultCountry);
        }

        public AlternativeTitles GetMovieAlternativeTitles(int id, string country)
        {
            return GetMovieMethod<AlternativeTitles>(id, MovieMethods.AlternativeTitles, country: country);
        }

        public Casts GetMovieCasts(int id)
        {
            return GetMovieMethod<Casts>(id, MovieMethods.Casts);
        }

        public ImagesWithId GetMovieImages(int id)
        {
            return GetMovieImages(id, DefaultLanguage);
        }

        public ImagesWithId GetMovieImages(int id, string language)
        {
            return GetMovieMethod<ImagesWithId>(id, MovieMethods.Images, language: language);
        }

        public KeywordsContainer GetMovieKeywords(int id)
        {
            return GetMovieMethod<KeywordsContainer>(id, MovieMethods.Keywords);
        }

        public Releases GetMovieReleases(int id)
        {
            return GetMovieMethod<Releases>(id, MovieMethods.Releases, dateFormat: "yyyy-MM-dd");
        }

        public Trailers GetMovieTrailers(int id)
        {
            return GetMovieMethod<Trailers>(id, MovieMethods.Trailers);
        }

        public TranslationsContainer GetMovieTranslations(int id)
        {
            return GetMovieMethod<TranslationsContainer>(id, MovieMethods.Translations);
        }

        public SearchContainer<MovieResult> GetMovieSimilarMovies(int id, int page = -1)
        {
            return GetMovieSimilarMovies(id, DefaultLanguage, page);
        }

        public SearchContainer<MovieResult> GetMovieSimilarMovies(int id, string language, int page = -1)
        {
            return GetMovieMethod<SearchContainer<MovieResult>>(id, MovieMethods.SimilarMovies, page: page, language: language, dateFormat: "yyyy-MM-dd");
        }

        public SearchContainer<ListResult> GetMovieLists(int id, int page = -1)
        {
            return GetMovieLists(id, DefaultLanguage, page);
        }

        public SearchContainer<ListResult> GetMovieLists(int id, string language, int page = -1)
        {
            return GetMovieMethod<SearchContainer<ListResult>>(id, MovieMethods.Lists, page: page, language: language);
        }

        public List<Change> GetMovieChanges(int id, DateTime? startDate = null, DateTime? endDate = null)
        {
            ChangesContainer changes = GetMovieMethod<ChangesContainer>(id, MovieMethods.Changes, startDate: startDate, endDate: endDate);

            return changes.Changes;
        }

        public Movie GetMovieLatest()
        {
            RestRequest req = new RestRequest("movie/latest");
            IRestResponse<Movie> resp = _client.Get<Movie>(req);

            return resp.Data;
        }

        public SearchContainer<MovieResult> GetMovieList(MovieListType type, int page = -1)
        {
            return GetMovieList(type, DefaultLanguage, page);
        }

        public SearchContainer<MovieResult> GetMovieList(MovieListType type, string language, int page = -1)
        {
            RestRequest req;
            switch (type)
            {
                case MovieListType.NowPlaying:
                    req = new RestRequest("movie/now_playing");
                    break;
                case MovieListType.Popular:
                    req = new RestRequest("movie/popular");
                    break;
                case MovieListType.TopRated:
                    req = new RestRequest("movie/top_rated");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (language != null)
                req.AddParameter("language", language);

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<SearchContainer<MovieResult>> resp = _client.Get<SearchContainer<MovieResult>>(req);

            return resp.Data;
        }
    }
}