using System;
using System.IO;
using System.Text;
using System.Xml;
using TMDbLib.Client;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            TMDbClient client = new TMDbClient("APIKEY");

            FileInfo configXml = new FileInfo("config.xml");

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

            // 
            client.GetCollection(1570, extraMethods: CollectionMethods.Images);
            client.GetCollectionImages(1570);

            //client.GetMovieList(MovieListType.NowPlaying);
            //client.GetMovieList(MovieListType.Popular);
            //client.GetMovieList(MovieListType.TopRated);
            //client.GetMovieAlternativeTitles(47964);
            //client.GetMovieCasts(47964);
            //client.GetMovieImages(47964);
            //client.GetMovieKeywords(47964);
            //client.GetMovieReleases(47964);
            //client.GetMovieTrailers(47964);
            //client.GetMovieTranslations(47964);
            //client.GetMovieSimilarMovies(47964);
            //client.GetMovieLists(47964);
            //client.GetMovieChanges(47964);

            //client.GetMovie(47964, extraMethods: Enum.GetValues(typeof(MovieMethods)).OfType<MovieMethods>().Aggregate((methods, movieMethods) => movieMethods | methods));

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
