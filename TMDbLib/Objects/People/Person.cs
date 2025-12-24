using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the list of alternative names.
    /// </summary>
    [JsonProperty("also_known_as")]
    public List<string> AlsoKnownAs { get; set; }

    /// <summary>
    /// Gets or sets the biography.
    /// </summary>
    [JsonProperty("biography")]
    public string Biography { get; set; }

    /// <summary>
    /// Gets or sets the birthday.
    /// </summary>
    [JsonProperty("birthday")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Gets or sets the change history.
    /// </summary>
    [JsonProperty("changes")]
    public ChangesContainer Changes { get; set; }

    /// <summary>
    /// Gets or sets the date of death.
    /// </summary>
    [JsonProperty("deathday")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]
    public DateTime? Deathday { get; set; }

    /// <summary>
    /// Gets or sets the external IDs.
    /// </summary>
    [JsonProperty("external_ids")]
    public ExternalIdsPerson ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the gender.
    /// </summary>
    [JsonProperty("gender")]
    public PersonGender Gender { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL.
    /// </summary>
    [JsonProperty("homepage")]
    public string Homepage { get; set; }

    /// <summary>
    /// Gets or sets the TMDb person ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the profile images.
    /// </summary>
    [JsonProperty("images")]
    public ProfileImages Images { get; set; }

    /// <summary>
    /// Gets or sets the IMDb ID.
    /// </summary>
    [JsonProperty("imdb_id")]
    public string ImdbId { get; set; }

    /// <summary>
    /// Gets or sets the movie credits.
    /// </summary>
    [JsonProperty("movie_credits")]
    public MovieCredits MovieCredits { get; set; }

    /// <summary>
    /// Gets or sets the person name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the place of birth.
    /// </summary>
    [JsonProperty("place_of_birth")]
    public string PlaceOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonProperty("popularity")]
    public double Popularity { get; set; }

    /// <summary>
    /// Gets or sets the department the person is known for.
    /// </summary>
    [JsonProperty("known_for_department")]
    public string KnownForDepartment { get; set; }

    /// <summary>
    /// Gets or sets the profile image path.
    /// </summary>
    [JsonProperty("profile_path")]
    public string ProfilePath { get; set; }

    /// <summary>
    /// Gets or sets the tagged images.
    /// </summary>
    [JsonProperty("tagged_images")]
    public SearchContainer<TaggedImage> TaggedImages { get; set; }

    /// <summary>
    /// Gets or sets the TV credits.
    /// </summary>
    [JsonProperty("tv_credits")]
    public TvCredits TvCredits { get; set; }

    /// <summary>
    /// Gets or sets the combined movie and TV credits.
    /// </summary>
    [JsonProperty("combined_credits")]
    public CombinedCredits CombinedCredits { get; set; }

    /// <summary>
    /// Gets or sets the translations.
    /// </summary>
    [JsonProperty("translations")]
    public TranslationsContainer Translations { get; set; }
}
