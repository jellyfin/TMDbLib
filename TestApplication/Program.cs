using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;

namespace TestApplication;

/// <summary>
/// Sample application demonstrating TMDbLib usage.
/// </summary>
internal class Program
{
    private static async Task Main(string[] args)
    {
        // Instantiate a new client, all that's needed is an API key, but it's possible to
        // also specify if SSL should be used, and if another server address should be used.
        using var client = new TMDbClient("c6b31d1cdad6a56a23f0c913e2482a31");

        // We need the config from TMDb in case we want to get stuff like images
        // The config needs to be fetched for each new client we create, but we can cache it to a file (as in this example).
        await FetchConfig(client);

        // Try fetching a movie
        await FetchMovieExample(client);

        // Once we've got a movie, or person, or so on, we can display images.
        // TMDb follow the pattern shown in the following example
        // This example also shows an important feature of most of the Get-methods.
        await FetchImagesExample(client);

        Console.WriteLine("Done.");
        Console.ReadLine();
    }
    private static async Task FetchConfig(TMDbClient client)
    {
        var configJson = new FileInfo("config.json");

        Console.WriteLine("Config file: " + configJson.FullName + ", Exists: " + configJson.Exists);

        if (configJson.Exists && configJson.LastWriteTimeUtc >= DateTime.UtcNow.AddHours(-1))
        {
            Console.WriteLine("Using stored config");
            var json = await File.ReadAllTextAsync(configJson.FullName, Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);

            client.SetConfig(TMDbJsonSerializer.Instance.DeserializeFromString<TMDbConfig>(json)!);
        }
        else
        {
            Console.WriteLine("Getting new config");
            var config = await client.GetConfigAsync();

            Console.WriteLine("Storing config");
            var json = TMDbJsonSerializer.Instance.SerializeToString(config);
            await File.WriteAllTextAsync(configJson.FullName, json, Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
        }
        Spacer();
    }
    private static async Task FetchImagesExample(TMDbClient client)
    {
        const int MovieId = 76338; // Thor: The Dark World (2013)

        // In the call below, we're fetching the wanted movie from TMDb, but we're also doing something else.
        // We're requesting additional data, in this case: Images. This means that the Movie property "Images" will be populated (else it will be null).
        // We could combine these properties, requesting even more information in one go:
        //      client.GetMovieAsync(movieId, MovieMethods.Images);
        //      client.GetMovieAsync(movieId, MovieMethods.Images | MovieMethods.Releases);
        //      client.GetMovieAsync(movieId, MovieMethods.Images | MovieMethods.Trailers | MovieMethods.Translations);
        //
        // .. and so on..
        //
        // Note: Each method normally corresponds to a property on the resulting object. If you haven't requested the information, the property will most likely be null.

        // Also note, that while we could have used 'client.GetMovieImagesAsync()' - it was better to do it like this because we also wanted the Title of the movie.
        var movie = await client.GetMovieAsync(MovieId, MovieMethods.Images);

        if (movie is null)
        {
            Console.WriteLine("Movie not found");
            return;
        }

        Console.WriteLine("Fetching images for '" + movie.Title + "'");

        // Images come in three forms, each dispayed below
        Console.WriteLine("Displaying Backdrops");
        await ProcessImages(client, movie.Images!.Backdrops!.Take(3), client.Config.Images!.BackdropSizes!);
        Console.WriteLine();

        Console.WriteLine("Displaying Posters");
        await ProcessImages(client, movie.Images!.Posters!.Take(3), client.Config.Images!.PosterSizes!);
        Console.WriteLine();

        Console.WriteLine("Displaying Logos");
        await ProcessImages(client, movie.Images!.Logos!.Take(3), client.Config.Images!.LogoSizes!);
        Console.WriteLine();

        Spacer();
    }
    private static async Task ProcessImages(TMDbClient client, IEnumerable<ImageData> images, IEnumerable<string> sizes)
    {
        // Displays basic information about each image, as well as all the possible adresses for it.
        // All images should be available in all the sizes provided by the configuration.

        var imagesLst = images.ToList();
        var sizesLst = sizes.ToList();

        foreach (ImageData imageData in imagesLst)
        {
            Console.WriteLine(imageData.FilePath);
            Console.WriteLine("\t " + imageData.Width + "x" + imageData.Height);

            // Calculate the images path
            // There are multiple resizing available for each image, directly from TMDb.
            // There's always the "original" size if you're in doubt which to choose.
            foreach (string size in sizesLst)
            {
                var imageUri = client.GetImageUrl(size, imageData.FilePath!);
                Console.WriteLine("\t -> " + imageUri);
            }
            Console.WriteLine();
        }
        // Download an image for testing, uses the internal HttpClient in the API.
        Console.WriteLine("Downloading image for the first url, as a test");

        var testUrl = client.GetImageUrl(sizesLst.First(), imagesLst.First().FilePath!);
        var bts = await client.GetImageBytesAsync(sizesLst.First(), imagesLst.First().FilePath!);

        Console.WriteLine($"Downloaded {testUrl}: {bts.Length} bytes");
    }
    private static async Task FetchMovieExample(TMDbClient client)
    {
        var query = "Thor";

        // This example shows the fetching of a movie.
        // Say the user searches for "Thor" in order to find "Thor: The Dark World" or "Thor"
        var results = await client.SearchMovieAsync(query);

        // The results is a list, currently on page 1 because we didn't specify any page.
        if (results is null)
        {
            Console.WriteLine("Searched for movies: '" + query + "', no results found");
            return;
        }

        Console.WriteLine("Searched for movies: '" + query + "', found " + results.TotalResults + " results in " +
                          results.TotalPages + " pages");

        // Let's iterate the first few hits
        foreach (SearchMovie result in results.Results!.Take(3))
        {
            // Print out each hit
            Console.WriteLine(result.Id + ": " + result.Title);
            Console.WriteLine("\t Original Title: " + result.OriginalTitle);
            Console.WriteLine("\t Release date  : " + result.ReleaseDate);
            Console.WriteLine("\t Popularity    : " + result.Popularity);
            Console.WriteLine("\t Vote Average  : " + result.VoteAverage);
            Console.WriteLine("\t Vote Count    : " + result.VoteCount);
            Console.WriteLine();
            Console.WriteLine("\t Backdrop Path : " + result.BackdropPath);
            Console.WriteLine("\t Poster Path   : " + result.PosterPath);

            Console.WriteLine();
        }
        Spacer();
    }
    private static void Spacer()
    {
        Console.WriteLine();
        Console.WriteLine(" ----- ");
        Console.WriteLine();
    }
}
