using System;
using System.ComponentModel;
using System.Reflection;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Utilities
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();

            /*if (!type.GetElementType() == IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
            */

            string value = enumerationValue.ToString();
            if (value.ToLower() == "undefined")
                return "Undefined";

            #region AccountListsMethods
            if (type == typeof(AccountListsMethods))
            {
                switch ((AccountListsMethods)Enum.Parse(typeof(AccountListsMethods), value, true))
                {
                    case AccountListsMethods.FavoriteMovies:
                        return "favorite_movies";
                    case AccountListsMethods.RatedMovies:
                        return "rated_movies";
                    case AccountListsMethods.MovieWatchlist:
                        return "movie_watchlist";
                }
            }
            #endregion

            #region AccountMovieSortBy
            if (type == typeof(AccountMovieSortBy))
            {
                switch ((AccountMovieSortBy)Enum.Parse(typeof(AccountMovieSortBy), value, true))
                {
                    case AccountMovieSortBy.CreatedAt:
                        return "created_at";
                }
            }
            #endregion

            #region CollectionMethods
            if (type == typeof(CollectionMethods))
            {
                switch ((CollectionMethods)Enum.Parse(typeof(CollectionMethods), value, true))
                {
                    case CollectionMethods.Images:
                        return "images";
                }
            }
            #endregion

            #region CompanyMethods
            if (type == typeof(CompanyMethods))
            {
                switch ((CompanyMethods)Enum.Parse(typeof(CompanyMethods), value, true))
                {
                    case CompanyMethods.Movies:
                        return "movies";
                }
            }
            #endregion

            #region DiscoverMovieSortBy
            if (type == typeof(DiscoverMovieSortBy))
            {
                switch ((DiscoverMovieSortBy)Enum.Parse(typeof(DiscoverMovieSortBy), value, true))
                {
                    case DiscoverMovieSortBy.VoteAverageDescending:
                        return "vote_average.desc";
                    case DiscoverMovieSortBy.VoteAverageAscending:
                        return "vote_average.asc";
                    case DiscoverMovieSortBy.ReleaseDateDescending:
                        return "release_date.desc";
                    case DiscoverMovieSortBy.ReleaseDateAscending:
                        return "release_date.asc";
                    case DiscoverMovieSortBy.PopularityDescending:
                        return "popularity.desc";
                    case DiscoverMovieSortBy.PopularityAscending:
                        return "popularity.asc";
                }
            }
            #endregion

            #region DiscoverTvShowSortBy
            if (type == typeof(DiscoverTvShowSortBy))
            {
                switch ((DiscoverTvShowSortBy)Enum.Parse(typeof(DiscoverTvShowSortBy), value, true))
                {
                    case DiscoverTvShowSortBy.VoteAverageDescending:
                        return "vote_average.desc";
                    case DiscoverTvShowSortBy.VoteAverageAscending:
                        return "vote_average.asc";
                    case DiscoverTvShowSortBy.FirstAirDateDescending:
                        return "first_air_date.desc";
                    case DiscoverTvShowSortBy.FirstAirDateAscending:
                        return "first_air_date.asc";
                    case DiscoverTvShowSortBy.PopularityDescending:
                        return "popularity.desc";
                    case DiscoverTvShowSortBy.PopularityAscending:
                        return "popularity.asc";
                }
            }
            #endregion

            #region FindExternalSource
            if (type == typeof(FindExternalSource))
            {
                switch ((FindExternalSource)Enum.Parse(typeof(FindExternalSource), value, true))
                {
                    case FindExternalSource.Imdb:
                        return "imdb_id";
                    case FindExternalSource.FreeBaseMid:
                        return "freebase_mid";
                    case FindExternalSource.FreeBaseId:
                        return "freebase_id";
                    case FindExternalSource.TvRage:
                        return "tvrage_id";
                    case FindExternalSource.TvDb:
                        return "tvdb_id";
                }
            }
            #endregion

            #region SortOrder
            if (type == typeof(SortOrder))
            {
                switch ((SortOrder)Enum.Parse(typeof(SortOrder), value, true))
                {
                    case SortOrder.Ascending:
                        return "asc";
                    case SortOrder.Descending:
                        return "desc";
                }
            }
            #endregion

            #region MovieMethods
            if (type == typeof(MovieMethods))
            {
                switch ((MovieMethods)Enum.Parse(typeof(MovieMethods), value, true))
                {
                    case MovieMethods.AlternativeTitles:
                        return "alternative_titles";
                    case MovieMethods.Credits:
                        return "credits";
                    case MovieMethods.Images:
                        return "images";
                    case MovieMethods.Keywords:
                        return "keywords";
                    case MovieMethods.Releases:
                        return "releases";
                    case MovieMethods.Trailers:
                        return "trailers";
                    case MovieMethods.Translations:
                        return "translations";
                    case MovieMethods.SimilarMovies:
                        return "similar_movies";
                    case MovieMethods.Lists:
                        return "lists";
                    case MovieMethods.Changes:
                        return "changes";
                    case MovieMethods.AccountStates:
                        return "account_states";
                }
            }
            #endregion

            #region PersonMethods
            if (type == typeof(PersonMethods))
            {
                switch ((PersonMethods)Enum.Parse(typeof(PersonMethods), value, true))
                {
                    case PersonMethods.Credits:
                        return "credits";
                    case PersonMethods.Images:
                        return "images";
                    case PersonMethods.Changes:
                        return "changes";
                }
            }
            #endregion

            #region TvEpisodeMethods
            if (type == typeof(TvEpisodeMethods))
            {
                switch ((TvEpisodeMethods)Enum.Parse(typeof(TvEpisodeMethods), value, true))
                {
                    case TvEpisodeMethods.Credits:
                        return "credits";
                    case TvEpisodeMethods.Images:
                        return "images";
                    case TvEpisodeMethods.ExternalIds:
                        return "external_ids";
                }
            }
            #endregion

            #region TvSeasonMethods
            if (type == typeof(TvSeasonMethods))
            {
                switch ((TvSeasonMethods)Enum.Parse(typeof(TvSeasonMethods), value, true))
                {
                    case TvSeasonMethods.Credits:
                        return "credits";
                    case TvSeasonMethods.Images:
                        return "images";
                    case TvSeasonMethods.ExternalIds:
                        return "external_ids";
                }
            }
            #endregion

            #region TvShowMethods
            if (type == typeof(TvShowMethods))
            {
                switch ((TvShowMethods)Enum.Parse(typeof(TvShowMethods), value, true))
                {
                    case TvShowMethods.Credits:
                        return "credits";
                    case TvShowMethods.Images:
                        return "images";
                    case TvShowMethods.ExternalIds:
                        return "external_ids";
                }
            }
            #endregion

            return value;
        }
    }
}
