using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a person with their biographical information and credits.
/// </summary>
public class Person
{
    /// <summary>
    /// Gets or sets a value indicating whether the person is an adult performer.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the list of alternative names.
    /// </summary>
    [JsonPropertyName("also_known_as")]
    public List<string>? AlsoKnownAs { get; set; }

    /// <summary>
    /// Gets or sets the biography.
    /// </summary>
    [JsonPropertyName("biography")]
    public string? Biography { get; set; }

    /// <summary>
    /// Gets or sets the birthday.
    /// </summary>
    [JsonPropertyName("birthday")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Gets or sets the change history.
    /// </summary>
    [JsonPropertyName("changes")]
    public ChangesContainer? Changes { get; set; }

    /// <summary>
    /// Gets or sets the date of death.
    /// </summary>
    [JsonPropertyName("deathday")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]
    public DateTime? Deathday { get; set; }

    /// <summary>
    /// Gets or sets the external IDs.
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIdsPerson? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the gender.
    /// </summary>
    [JsonPropertyName("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL.
    /// </summary>
    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    /// <summary>
    /// Gets or sets the TMDb person ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the profile images.
    /// </summary>
    [JsonPropertyName("images")]
    public ProfileImages? Images { get; set; }

    /// <summary>
    /// Gets or sets the IMDb ID.
    /// </summary>
    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; set; }

    /// <summary>
    /// Gets or sets the movie credits.
    /// </summary>
    [JsonPropertyName("movie_credits")]
    public MovieCredits? MovieCredits { get; set; }

    /// <summary>
    /// Gets or sets the person name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the place of birth.
    /// </summary>
    [JsonPropertyName("place_of_birth")]
    public string? PlaceOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    /// <summary>
    /// Gets or sets the department the person is known for.
    /// </summary>
    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }

    /// <summary>
    /// Gets or sets the tagged images.
    /// </summary>
    [JsonPropertyName("tagged_images")]
    public SearchContainer<TaggedImage>? TaggedImages { get; set; }

    /// <summary>
    /// Gets or sets the TV credits.
    /// </summary>
    [JsonPropertyName("tv_credits")]
    public TvCredits? TvCredits { get; set; }

    /// <summary>
    /// Gets or sets the combined movie and TV credits.
    /// </summary>
    [JsonPropertyName("combined_credits")]
    public CombinedCredits? CombinedCredits { get; set; }

    /// <summary>
    /// Gets or sets the translations.
    /// </summary>
    [JsonPropertyName("translations")]
    public TranslationsContainer? Translations { get; set; }
}
