# TMDbLib

[![Build](https://github.com/jellyfin/TMDbLib/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jellyfin/TMDbLib/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/Tmdblib.svg)](https://www.nuget.org/packages/Tmdblib)
[![GitHub Packages](https://img.shields.io/badge/package-alpha-green)](https://github.com/jellyfin/TMDbLib/pkgs/nuget/TMDbLib)

A near-complete .NET wrapper for v3 of [TMDb's API](https://www.themoviedb.org/).

## Table of Contents

- [Installation](#installation)
- [Documentation](#documentation)
- [Examples](#examples)
- [Tips](#tips)

## Installation

Install via NuGet:

```bash
dotnet add package TMDbLib
```

Or for alpha packages from GitHub Packages, see the [releases page](https://github.com/jellyfin/TMDbLib/pkgs/nuget/TMDbLib).

## Documentation

Most of the library is self-explaining and closely follows the official [TMDb API documentation](https://developers.themoviedb.org/3/getting-started).

## Examples

### Basic Usage

Simple example, getting the basic info for "A Good Day to Die Hard":

```csharp
using TMDbLib.Client;

var client = new TMDbClient("APIKey");
var movie = await client.GetMovieAsync(47964);

Console.WriteLine($"Movie name: {movie.Title}");
```

### Fetching Additional Data

Using the extra features of TMDb, you can fetch more info in one request (here we fetch credits and videos):

```csharp
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

var client = new TMDbClient("APIKey");
var movie = await client.GetMovieAsync(47964, MovieMethods.Credits | MovieMethods.Videos);

Console.WriteLine($"Movie title: {movie.Title}");

foreach (var cast in movie.Credits.Cast)
    Console.WriteLine($"{cast.Name} - {cast.Character}");

Console.WriteLine();

foreach (var video in movie.Videos.Results)
    Console.WriteLine($"Trailer: {video.Type} ({video.Site}), {video.Name}");
```

### Searching for Movies

Search for people or movies. This example searches for "007", yielding James Bond films:

```csharp
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

var client = new TMDbClient("APIKey");
var results = await client.SearchMovieAsync("007");

Console.WriteLine($"Got {results.Results.Count:N0} of {results.TotalResults:N0} results");

foreach (var result in results.Results)
    Console.WriteLine(result.Title);
```

### Working with Collections

TMDb groups related movies into collections (e.g., James Bond, Die Hard). Here's how to find and list a collection:

```csharp
using TMDbLib.Client;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

var client = new TMDbClient("APIKey");
var collections = await client.SearchCollectionAsync("James Bond");

Console.WriteLine($"Got {collections.Results.Count:N0} collections");

var jamesBond = await client.GetCollectionAsync(collections.Results.First().Id);
Console.WriteLine($"Collection: {jamesBond.Name}");
Console.WriteLine($"Got {jamesBond.Parts.Count:N0} James Bond movies");

foreach (var part in jamesBond.Parts)
    Console.WriteLine(part.Title);
```

## Tips

- All methods are `async` and awaitable
- Most methods are straightforward and named accordingly: `GetMovieAsync`, `GetPersonAsync`, etc.
- Almost all method enums use `[Flags]`, allowing you to combine them: `MovieMethods.Credits | MovieMethods.Videos`
- TMDb returns minimal data by default; most properties on classes like `Movie` are null until you request extra data using the method enums
