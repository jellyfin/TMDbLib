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
        string dateFormat = "yyyy-MM-dd",
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
            req.AddParameter("start_date", startDate.Value.ToString(dateFormat, CultureInfo.InvariantCulture));
        }

        if (endDate is not null)
        {
            req.AddParameter("end_date", endDate.Value.ToString(dateFormat, CultureInfo.InvariantCulture));
        }

        var resp = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the most recently created person on TMDb.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The latest person.</returns>
    public async Task<Person?> GetLatestPersonAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("person/latest");

        var resp = await req.GetOfT<Person>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets a person by id.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="extraMethods">Additional methods to append to the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person, or null if not found.</returns>
    public async Task<Person?> GetPersonAsync(
        int personId,
        PersonMethods extraMethods = PersonMethods.Undefined,
        CancellationToken cancellationToken = default)
    {
        return await GetPersonAsync(personId, DefaultLanguage, extraMethods, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a person by id in a specific language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="extraMethods">Additional methods to append to the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person, or null if not found.</returns>
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
            Enum.GetValues<PersonMethods>()
                                         .Except([PersonMethods.Undefined])
                                         .Where(s => extraMethods.HasFlag(s))
                                         .Select(s => s.GetDescription()));

        if (appends != string.Empty)
        {
            req.AddParameter("append_to_response", appends);
        }

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
    /// Gets the external ids for a person (IMDb, Facebook, Twitter, Instagram, etc.).
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's external ids.</returns>
    public async Task<ExternalIdsPerson?> GetPersonExternalIdsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<ExternalIdsPerson>(personId, PersonMethods.ExternalIds, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the profile images for a person.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's profile images.</returns>
    public async Task<ProfileImages?> GetPersonImagesAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<ProfileImages>(personId, PersonMethods.Images, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of popular people.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The popular people.</returns>
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

        var resp = await req.GetOfT<SearchContainer<SearchPerson>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the movie credits for a person.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's movie credits.</returns>
    public async Task<MovieCredits?> GetPersonMovieCreditsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonMovieCreditsAsync(personId, DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the movie credits for a person in a specific language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's movie credits.</returns>
    public async Task<MovieCredits?> GetPersonMovieCreditsAsync(int personId, string? language, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<MovieCredits>(personId, PersonMethods.MovieCredits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the tagged images for a person.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The tagged images featuring the person.</returns>
    public async Task<SearchContainerWithId<TaggedImage>?> GetPersonTaggedImagesAsync(int personId, int page, CancellationToken cancellationToken = default)
    {
        return await GetPersonTaggedImagesAsync(personId, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the tagged images for a person in a specific language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The tagged images featuring the person.</returns>
    public async Task<SearchContainerWithId<TaggedImage>?> GetPersonTaggedImagesAsync(int personId, string? language, int page, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<SearchContainerWithId<TaggedImage>>(personId, PersonMethods.TaggedImages, language: language, page: page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV credits for a person.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's TV credits.</returns>
    public async Task<TvCredits?> GetPersonTvCreditsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonTvCreditsAsync(personId, DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the TV credits for a person in a specific language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's TV credits.</returns>
    public async Task<TvCredits?> GetPersonTvCreditsAsync(int personId, string? language, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<TvCredits>(personId, PersonMethods.TvCredits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the available translations for a person.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's translations.</returns>
    public async Task<TranslationsContainer?> GetPersonTranslationsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<TranslationsContainer>(personId, PersonMethods.Translations, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the combined movie and TV credits for a person.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's combined credits.</returns>
    public async Task<CombinedCredits?> GetPersonCombinedCreditsAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await GetPersonCombinedCreditsAsync(personId, DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the combined movie and TV credits for a person in a specific language.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's combined credits.</returns>
    public async Task<CombinedCredits?> GetPersonCombinedCreditsAsync(int personId, string? language, CancellationToken cancellationToken = default)
    {
        return await GetPersonMethodInternal<CombinedCredits>(personId, PersonMethods.CombinedCredits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
