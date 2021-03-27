TMDbLib [![Generic Build](https://github.com/LordMike/TMDbLib/actions/workflows/dotnet.yml/badge.svg)](https://github.com/LordMike/TMDbLib/actions/workflows/dotnet.yml) [![NuGet](https://img.shields.io/nuget/v/Tmdblib.svg)](https://www.nuget.org/packages/Tmdblib) [![GHPackages](https://img.shields.io/badge/package-alpha-green)](https://github.com/LordMike/TMDbLib/packages/691970)
=======

A near-complete wrapper for v3 of TMDb's API (TheMovieDb - https://www.themoviedb.org/).

Nuget
-----

Install from Nuget using the command: **Install-Package TMDbLib**
View more about that here: http://nuget.org/packages/TMDbLib/

## Alpha packages
.. can be found [here](https://github.com/LordMike/TMDbLib/packages/691970)

Index
---------

- [Nuget](#nuget)
- [Documentation](#documentation)
- [Examples](#examples)
- [Tips](#tips)

Documentation
-------- 

Most of the library is self-explaining, and closely follows the possibilities at the official TMDb documentation site: [developers.themoviedb.org](https://developers.themoviedb.org/3/getting-started).

Examples
-------- 

Simple example, getting the basic info for "A good day to die hard".

```csharp
TMDbClient client = new TMDbClient("APIKey");
Movie movie = client.GetMovieAsync(47964).Result;

Console.WriteLine($"Movie name: {movie.Title}");
```

Using the extra features of TMDb, we can fetch more info in one go (here we fetch casts as well as trailers):

```csharp
TMDbClient client = new TMDbClient("APIKey");
Movie movie = client.GetMovie(47964, MovieMethods.Casts | MovieMethods.Trailers);

Console.WriteLine($"Movie title: {movie.Title}");
foreach (Cast cast in movie.Credits.Cast)
    Console.WriteLine($"{cast.Name} - {cast.Character}");

Console.WriteLine();
foreach (Video video in movie.Videos.Results)
    Console.WriteLine($"Trailer: {video.Type} ({video.Site}), {video.Name}");
```

It is likewise simple to search for people or movies, for example here we search for "007". This yields basically every James Bond film ever made:

```csharp
TMDbClient client = new TMDbClient("APIKey");
SearchContainer<SearchMovie> results = client.SearchMovieAsync("007").Result;

Console.WriteLine($"Got {results.Results.Count:N0} of {results.TotalResults:N0} results");
foreach (SearchMovie result in results.Results)
    Console.WriteLine(result.Title);
```

However, another way to get all James Bond movies, is to use the collection-approach. TMDb makes collections for series of movies, such as Die Hard and James Bond. I know there is one, so I will show how to search for the collection, and then list all movies in it:

```csharp
TMDbClient client = new TMDbClient("APIKey");
SearchContainer<SearchCollection> collectons = client.SearchCollectionAsync("James Bond").Result;
Console.WriteLine($"Got {collectons.Results.Count:N0} collections");

Collection jamesBonds = client.GetCollectionAsync(collectons.Results.First().Id).Result;
Console.WriteLine($"Collection: {jamesBonds.Name}" );
Console.WriteLine();

Console.WriteLine($"Got {jamesBonds.Parts.Count:N0} James Bond Movies");
foreach (SearchMovie part in jamesBonds.Parts)
    Console.WriteLine(part.Title);
```

Tips
---------

* All methods are `async` and awaitable
* Most methods are very straightforward, and do as they are named, `GetMovie`, `GetPerson` etc.
* Almost all enums are of the `[Flags]` type. This means you can combine them: `MovieMethods.Casts | MovieMethods.Trailers`
* TMDb are big fans of serving as little as possible, so most properties on primary classes like `Movie` are null, until you request the extra data using the enums like above.