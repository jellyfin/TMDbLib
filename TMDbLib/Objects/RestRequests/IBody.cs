using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

/// <summary>
/// Marker interface for TMDb API request bodies.
/// </summary>
[JsonDerivedType(typeof(AccountChangeBody))]
[JsonDerivedType(typeof(ListCreateBody))]
[JsonDerivedType(typeof(MovieIdBody))]
[JsonDerivedType(typeof(RatingBody))]
[JsonDerivedType(typeof(WatchlistBody))]
public interface IBody;
