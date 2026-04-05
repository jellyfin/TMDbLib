using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

[JsonDerivedType(typeof(AccountChangeBody))]
[JsonDerivedType(typeof(ListCreateIBody))]
[JsonDerivedType(typeof(MovieIdIBody))]
[JsonDerivedType(typeof(RatingIBody))]
[JsonDerivedType(typeof(WatchlistIBody))]
public interface IBody;
