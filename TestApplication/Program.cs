using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace TestApplication
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // Instantiate a new client, all that's needed is an API key, but it's possible to 
            // also specify if SSL should be used, and if another server address should be used.
            TMDbClient client = new TMDbClient("APIKEY");

            // We need the config from TMDb in case we want to get stuff like images
            // The config needs to be fetched for each new client we create, but we can cache it to a file (as in this example).
            FetchConfig(client);

            // Try fetching a movie
            FetchMovieExample(client);

            // Once we've got a movie, or person, or so on, we can display images. 
            // TMDb follow the pattern shown in the following example
            // This example also shows an important feature of most of the Get-methods.
            FetchImagesExample(client);

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static void FetchConfig(TMDbClient client)
        {
            FileInfo configXml = new FileInfo("config.xml");

            Console.WriteLine("Config file: " + configXml.FullName + ", Exists: " + configXml.Exists);

            if (configXml.Exists && configXml.LastWriteTimeUtc >= DateTime.UtcNow.AddHours(-1))
            {
                Console.WriteLine("Using stored config");
                string xml = File.ReadAllText(configXml.FullName, Encoding.Unicode);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                client.SetConfig(Serializer.Deserialize<TMDbConfig>(xmlDoc));
            }
            else
            {
                Console.WriteLine("Getting new config");
                client.GetConfig();

                Console.WriteLine("Storing config");
                XmlDocument xmlDoc = Serializer.Serialize(client.Config);
                File.WriteAllText(configXml.FullName, xmlDoc.OuterXml, Encoding.Unicode);
            }

            Spacer();
        }

        private static void FetchImagesExample(TMDbClient client)
        {
            const int movieId = 76338; // Thor: The Dark World (2013)

            // In the call below, we're fetching the wanted movie from TMDb, but we're also doing something else.
            // We're requesting additional data, in this case: Images. This means that the Movie property "Images" will be populated (else it will be null).
            // We could combine these properties, requesting even more information in one go:
            //      client.GetMovie(movieId, MovieMethods.Images);
            //      client.GetMovie(movieId, MovieMethods.Images | MovieMethods.Releases);
            //      client.GetMovie(movieId, MovieMethods.Images | MovieMethods.Trailers | MovieMethods.Translations);
            //
            // .. and so on..
            // 
            // Note: Each method normally corresponds to a property on the resulting object. If you haven't requested the information, the property will most likely be null.

            // Also note, that while we could have used 'client.GetMovieImages()' - it was better to do it like this because we also wanted the Title of the movie.
            Movie movie = client.GetMovie(movieId, MovieMethods.Images);

            Console.WriteLine("Fetching images for '" + movie.Title + "'");

            // Images come in two forms, each dispayed below
            Console.WriteLine("Displaying Backdrops");
            ProcessImages(client, movie.Images.Backdrops.Take(3), client.Config.Images.BackdropSizes);
            Console.WriteLine();

            Console.WriteLine("Displaying Posters");
            ProcessImages(client, movie.Images.Posters.Take(3), client.Config.Images.PosterSizes);
            Console.WriteLine();

            Spacer();
        }

        private static void ProcessImages(TMDbClient client, IEnumerable<ImageData> images, IEnumerable<string> sizes)
        {
            // Displays basic information about each image, as well as all the possible adresses for it.
            // All images should be available in all the sizes provided by the configuration.

            foreach (ImageData imageData in images)
            {
                Console.WriteLine(imageData.FilePath);
                Console.WriteLine("\t " + imageData.Width + "x" + imageData.Height);

                // Calculate the images path
                // There are multiple resizing available for each image, directly from TMDb.
                // There's always the "original" size if you're in doubt which to choose.
                foreach (string size in sizes)
                {
                    Uri imageUri = client.GetImageUrl(size, imageData.FilePath);
                    Console.WriteLine("\t -> " + imageUri);
                }

                Console.WriteLine();
            }
        }

        private static void FetchMovieExample(TMDbClient client)
        {
            string query = "Thor";

            // This example shows the fetching of a movie.
            // Say the user searches for "Thor" in order to find "Thor: The Dark World" or "Thor"
            SearchContainer<SearchMovie> results = client.SearchMovie(query);

            // The results is a list, currently on page 1 because we didn't specify any page.
            Console.WriteLine("Searched for movies: '" + query + "', found " + results.TotalResults + " results in " +
                              results.TotalPages + " pages");

            // Let's iterate the first few hits
            foreach (SearchMovie result in results.Results.Take(3))
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
}