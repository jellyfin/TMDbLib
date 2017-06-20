TMDbLib [![Build status](https://ci.appveyor.com/api/projects/status/t7wph9cawrl9qho0?svg=true)](https://ci.appveyor.com/project/LordMike/tmdblib) [![NuGet](https://img.shields.io/nuget/v/Tmdblib.svg)](https://www.nuget.org/packages/Tmdblib)
=======

A near-complete wrapper for v3 of TMDb's API (TheMovieDb - https://www.themoviedb.org/).

Nuget
-----

Install from Nuget using the command: **Install-Package TMDbLib**
View more about that here: http://nuget.org/packages/TMDbLib/

Index
---------

- [Nuget](#nuget)
- [Documentation](#documentation)
- [Examples](#examples)
- [Tips](#tips)
- [Changelog](#changelog)

Documentation
-------- 

Most of the library is self-explaining, and closely follows the possibilities at the official TMDb documentation site: [developers.themoviedb.org](https://developers.themoviedb.org/3/getting-started).

Examples
-------- 

Simple example, getting the basic info for "A good day to die hard".

    TMDbClient client = new TMDbClient("APIKey");
    Movie movie = client.GetMovieAsync(47964).Result;
    
    Console.WriteLine($"Movie name: {movie.Title}");

Using the extra features of TMDb, we can fetch more info in one go (here we fetch casts as well as trailers):

    TMDbClient client = new TMDbClient("APIKey");
    Movie movie = client.GetMovie(47964, MovieMethods.Casts | MovieMethods.Trailers);
    
    Console.WriteLine($"Movie title: {movie.Title}");
    foreach (Cast cast in movie.Credits.Cast)
        Console.WriteLine($"{cast.Name} - {cast.Character}");

    Console.WriteLine();
    foreach (Video video in movie.Videos.Results)
        Console.WriteLine($"Trailer: {video.Type} ({video.Site}), {video.Name}");

It is likewise simple to search for people or movies, for example here we search for "007". This yields basically every James Bond film ever made:

    TMDbClient client = new TMDbClient("APIKey");
    SearchContainer<SearchMovie> results = client.SearchMovieAsync("007").Result;

    Console.WriteLine($"Got {results.Results.Count:N0} of {results.TotalResults:N0} results");
    foreach (SearchMovie result in results.Results)
        Console.WriteLine(result.Title);

However, another way to get all James Bond movies, is to use the collection-approach. TMDb makes collections for series of movies, such as Die Hard and James Bond. I know there is one, so I will show how to search for the collection, and then list all movies in it:

    TMDbClient client = new TMDbClient("APIKey");
    SearchContainer<SearchCollection> collectons = client.SearchCollectionAsync("James Bond").Result;
    Console.WriteLine($"Got {collectons.Results.Count:N0} collections");

    Collection jamesBonds = client.GetCollectionAsync(collectons.Results.First().Id).Result;
    Console.WriteLine($"Collection: {jamesBonds.Name}" );
    Console.WriteLine();

    Console.WriteLine($"Got {jamesBonds.Parts.Count:N0} James Bond Movies");
    foreach (SearchMovie part in jamesBonds.Parts)
        Console.WriteLine(part.Title);

Tips
---------

* All methods are `async` and awaitable
* Most methods are very straightforward, and do as they are named, `GetMovie`, `GetPerson` etc.
* Almost all enums are of the `[Flags]` type. This means you can combine them: `MovieMethods.Casts | MovieMethods.Trailers`
* TMDb are big fans of serving as little as possible, so most properties on primary classes like `Movie` are null, until you request the extra data using the enums like above.

Changelog
---------

**1.0.7-alpha**
Changes:
   - Added gender to cast and crew for movies
   - Modified enum handeling for when null is returned from the api

**1.0.6-alpha**
Changes:
   - Added gender to cast and crew for TV shows
   - Added recommendations for tvshows and movies

**1.0.5-alpha**
Changes:
   - Added `Rating` to `AccountGetRatedMoviesAsync` results
   
**1.0.4-alpha**
Changes:
   - Implemented `Serialize()` for a number of custom serializers
   - Implemented `destroyed` Changes action
   - Added ability to pass a WebProxy to the TMDbClient
   
**1.0.0**

No changes. We finally moved to the first stable release in over a year. Compare to `0.8.3`, there will be some breaking changes - but most should be trivial to fix.

**0.9.6-alpha**
Changes:
   - Added UAP10.0 as a target
   - Changed netstandard1.6 target to netstandard1.0

**0.9.5-alpha**
Changes:
   - ChangeItem has been refactored into separate types for each of the applicable actions (#197)
   - Created a new object for Episodes of a TvSeason
   - More cleanup of types, removing unset properties and adding missing ones
   - Adjust the AccountStates object for TvEpisodes in particular, so the properties match returned values from TMDb

**0.9.4-alpha**
Changes:
   - Refining tests revealed a number of mismatches between the API and the library. These are fixed as I go along
   - A number of types will have moved namespaces, and properties have been added / removed

**0.9.2-alpha**
Changes:
   - Combined a number of classes (#195)
   - Drastically improved the SearchMulti method (#145)
   - Used Newtonsoft.Json tricks to deserialize JSON into multiple different types

**0.9.1-alpha**
Changes:
 - Changed project to a .NET Core project (#188), changes required:
   - Removed the [Display] attribute, replaced with custom attribute
   - Changed StringComparison.InvariantCultureIgnoreCase to OrdinalIgnoreCase
   - Removed [Serializable]
   - Support `net45` and `netcoreapp1.0` (also added `net451`, `net452`, `net46` and `netstandard1.6`)
   - Removed ObjectHelper
   - Upgraded to `Newtonsoft.Json 9.0.1` to support `netcoreapp1.0`

**0.9.0-alpha**
 - Removed Restsharp in favour of HttpClient and Json.Net
   - Fixes a lot of weirdness in Json parsing and gives flexibility
   - Simplified retry logic
 - Brings the API up to date
   - Broke up Tv shows and Movie methods
   - Refactorings in code
   - Breaking changes, mostly renames and splitting methods
 - Async support

**0.8.3**

 - Major update which brings the API up to date (minus a few features)
 - Multiple breaking changes from 0.7, but mostly parameter changes or renames.
 - Prepared for a 0.9 release with `Async` support.

**0.7**

 - First release
 - Available on Nuget
 - Basic API design with a great potential for refactoring (be warned on design changes)
 - Supports most (if not all) read-only operations (login sessions not supported - yet).
