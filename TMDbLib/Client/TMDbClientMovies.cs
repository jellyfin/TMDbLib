using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RestSharp;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Movie GetMovie(int movieId, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return GetMovie(movieId, DefaultLanguage, extraMethods);
        }

        public Movie GetMovie(string imdbId, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return GetMovie(imdbId, DefaultLanguage, extraMethods);
        }

        public Movie GetMovie(int movieId, string language, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            return GetMovie(movieId.ToString(CultureInfo.InvariantCulture), language, extraMethods);
        }

        public Movie GetMovie(string imdbId, string language, MovieMethods extraMethods = MovieMethods.Undefined)
        {
            var req = new RestRequest("movie/{movieId}");
            req.AddUrlSegment("movieId", imdbId);

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

                if (resp.Data.Credits != null)
                    resp.Data.Credits.Id = resp.Data.Id;

                if (resp.Data.Releases != null)
                    resp.Data.Releases.Id = resp.Data.Id;

                if (resp.Data.Keywords != null)
                    resp.Data.Keywords.Id = resp.Data.Id;

                if (resp.Data.Translations != null)
                    resp.Data.Translations.Id = resp.Data.Id;
            }

            return resp.Data;
        }

        private T GetMovieMethod<T>(int movieId, MovieMethods movieMethod, string dateFormat = null, string country = null,
                                    string language = null, int page = 0, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            var req = new RestRequest("movie/{movieId}/{method}");
            req.AddUrlSegment("movieId", movieId.ToString());
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

        public AlternativeTitles GetMovieAlternativeTitles(int movieId)
        {
            return GetMovieAlternativeTitles(movieId, DefaultCountry);
        }

        public AlternativeTitles GetMovieAlternativeTitles(int movieId, string country)
        {
            return GetMovieMethod<AlternativeTitles>(movieId, MovieMethods.AlternativeTitles, country: country);
        }

        public Credits GetMovieCredits(int movieId)
        {
            return GetMovieMethod<Credits>(movieId, MovieMethods.Credits);
        }

        public ImagesWithId GetMovieImages(int movieId)
        {
            return GetMovieImages(movieId, DefaultLanguage);
        }

        public ImagesWithId GetMovieImages(int movieId, string language)
        {
            return GetMovieMethod<ImagesWithId>(movieId, MovieMethods.Images, language: language);
        }

        public KeywordsContainer GetMovieKeywords(int movieId)
        {
            return GetMovieMethod<KeywordsContainer>(movieId, MovieMethods.Keywords);
        }

        public Releases GetMovieReleases(int movieId)
        {
            return GetMovieMethod<Releases>(movieId, MovieMethods.Releases, dateFormat: "yyyy-MM-dd");
        }

        public Trailers GetMovieTrailers(int movieId)
        {
            return GetMovieMethod<Trailers>(movieId, MovieMethods.Trailers);
        }

        public TranslationsContainer GetMovieTranslations(int movieId)
        {
            return GetMovieMethod<TranslationsContainer>(movieId, MovieMethods.Translations);
        }

        public SearchContainer<MovieResult> GetMovieSimilarMovies(int movieId, int page = 0)
        {
            return GetMovieSimilarMovies(movieId, DefaultLanguage, page);
        }

        public SearchContainer<MovieResult> GetMovieSimilarMovies(int movieId, string language, int page = 0)
        {
            return GetMovieMethod<SearchContainer<MovieResult>>(movieId, MovieMethods.SimilarMovies, page: page, language: language, dateFormat: "yyyy-MM-dd");
        }

        public SearchContainer<ListResult> GetMovieLists(int movieId, int page = 0)
        {
            return GetMovieLists(movieId, DefaultLanguage, page);
        }

        public SearchContainer<ListResult> GetMovieLists(int movieId, string language, int page = 0)
        {
            return GetMovieMethod<SearchContainer<ListResult>>(movieId, MovieMethods.Lists, page: page, language: language);
        }

        public List<Change> GetMovieChanges(int movieId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return GetMovieMethod<ChangesContainer>(movieId, MovieMethods.Changes, startDate: startDate, endDate: endDate, dateFormat: "yyyy-MM-dd HH:mm:ss UTC").Changes;
        }

        public Movie GetMovieLatest()
        {
            var req = new RestRequest("movie/latest");
            IRestResponse<Movie> resp = _client.Get<Movie>(req);

            return resp.Data;
        }

        public SearchContainer<MovieResult> GetMovieList(MovieListType type, int page = 0)
        {
            return GetMovieList(type, DefaultLanguage, page);
        }

        public SearchContainer<MovieResult> GetMovieList(MovieListType type, string language, int page = 0)
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
