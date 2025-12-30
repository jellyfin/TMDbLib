using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<T?> GetPersonMethodInternal<T>(
        int personId,
        PersonMethods personMethod,
        string? dateFormat = null,
        string? country = null,
        string? language = null,
        int page = 0,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
        where T : new()
    {
        var req = _client.Create("person/{personId}/{method}");
        req.AddUrlSegment("personId", personId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", personMethod.GetDescription());

        // TODO: Dateformat?
        // if (dateFormat is not null)
        //    req.DateFormat = dateFormat;

        if (country is not null)
        {
            req.AddParameter("country", country);
        }

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (startDate.HasValue)
        {
            req.AddParameter("startDate", startDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        if (endDate is not null)
        {
            req.AddParameter("endDate", endDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        var resp = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves the most recently created person on TMDb.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The latest person added to TMDb.</returns>
    public async Task<Person?> GetLatestPersonAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("person/latest");

        // TODO: Dateformat?
        // req.DateFormat = "yyyy-MM-dd";

        var resp = await req.GetOfT<Person>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves a person by TMDb id using the default language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="extraMethods">A list of additional methods to execute for this request as enum flags.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The requested person or null if not found.</returns>
    public async Task<Person?> GetPersonAsync(
        int personId,
        PersonMethods extraMethods = PersonMethods.Undefined,
        CancellationToken cancellationToken = default)
    {
        return await GetPersonAsync(personId, DefaultLanguage, extraMethods, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a person by TMDb id with specific language settings.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="extraMethods">A list of additional methods to execute for this request as enum flags.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The requested person or null if not found.</returns>
    public async Task<Person?> GetPersonAsync(int personId, string? language, PersonMethods extraMethods = PersonMethods.Undefined, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("person/{personId}");
        req.AddUrlSegment("personId", personId.ToString(CultureInfo.InvariantCulture));

        if (language is not null)
        {
            req.AddParameter("language", language);
        }

        var appends = string.Join(
            ",",
            Enum.GetValues(typeof(PersonMethods))
                                         .OfType<PersonMethods>()
                                         .Except([PersonMethods.Undefined])
                                         .Where(s => extraMethods.HasFlag(s))
                                         .Select(s => s.GetDescription()));

        if (appends != string.Empty)
        {
            req.AddParameter("append_to_response", appends);
        }

        // TODO: Dateformat?
        // req.DateFormat = "yyyy-MM-dd";

        using var response = await req.Get<Person>(cancellationToken).ConfigureAwait(false);

        if (!response.IsValid)
        {
            return null;
        }

        var item = await response.GetDataObject().ConfigureAwait(false);

        // Patch up data, so that the end user won't notice that we share objects between request-types.
        if (item is not null)
        {
            if (item.Images is not null)
            {
                item.Images.Id = item.Id;
            }

            if (item.TvCredits is not null)
            {
                item.TvCredits.Id = item.Id;
            }

            if (item.MovieCredits is not null)
            {
                item.MovieCredits.Id = item.Id;
            }

            if (item.CombinedCredits is not null)
            {
                item.CombinedCredits.Id = item.Id;
            }
        }

        return item;
    }

    /// <summary>
    /// Retrieves external IDs for a person (IMDB, Facebook, Twitter, Instagram, etc.).
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An object containing all known external IDs for the person.</returns>
    public async Task<ExternalIdsPerson?> GetPersonExternalIdsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<ExternalIdsPerson>(personId, PersonMethods.ExternalIds, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves profile images for a person.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The profile images for the person.</returns>
    public async Task<ProfileImages?> GetPersonImagesAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<ProfileImages>(personId, PersonMethods.Images, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a list of popular people.
    /// </summary>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with popular people.</returns>
    public async Task<SearchContainer<SearchPerson>?> GetPersonPopularListAsync(int page = 0, string? language = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("person/popular");

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (language is not null)
        {
            req.AddParameter("language", language);
        }

        // TODO: Dateformat?
        // req.DateFormat = "yyyy-MM-dd";

        var resp = await req.GetOfT<SearchContainer<SearchPerson>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves movie credits for a person using the default language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Movie credits including cast and crew roles for the person.</returns>
    public async Task<MovieCredits?> GetPersonMovieCreditsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonMovieCreditsAsync(personId, DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves movie credits for a person with specific language settings.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Movie credits including cast and crew roles for the person.</returns>
    public async Task<MovieCredits?> GetPersonMovieCreditsAsync(int personId, string? language, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<MovieCredits>(personId, PersonMethods.MovieCredits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves tagged images for a person using the default language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with tagged images featuring the person.</returns>
    public async Task<SearchContainerWithId<TaggedImage>?> GetPersonTaggedImagesAsync(int personId, int page, CancellationToken cancellationToken = default)
    {
        return await GetPersonTaggedImagesAsync(personId, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves tagged images for a person with specific language settings.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="page">The page number of results to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A search container with tagged images featuring the person.</returns>
    public async Task<SearchContainerWithId<TaggedImage>?> GetPersonTaggedImagesAsync(int personId, string? language, int page, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<SearchContainerWithId<TaggedImage>>(personId, PersonMethods.TaggedImages, language: language, page: page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves TV show credits for a person using the default language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>TV show credits including cast and crew roles for the person.</returns>
    public async Task<TvCredits?> GetPersonTvCreditsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonTvCreditsAsync(personId, DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves TV show credits for a person with specific language settings.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>TV show credits including cast and crew roles for the person.</returns>
    public async Task<TvCredits?> GetPersonTvCreditsAsync(int personId, string? language, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<TvCredits>(personId, PersonMethods.TvCredits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves available translations for a person.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A container with translation information for the person.</returns>
    public async Task<TranslationsContainer?> GePersonTranslationsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<TranslationsContainer>(personId, PersonMethods.Translations, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves combined movie and TV credits for a person using the default language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Combined credits including both movie and TV cast/crew roles for the person.</returns>
    public async Task<CombinedCredits?> GetPersonCombinedCreditsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonCombinedCreditsAsync(personId, DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves combined movie and TV credits for a person with specific language settings.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Combined credits including both movie and TV cast/crew roles for the person.</returns>
    public async Task<CombinedCredits?> GetPersonCombinedCreditsAsync(int personId, string? language, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<CombinedCredits>(personId, PersonMethods.CombinedCredits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
