using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using TMDbLib.Client;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Person;

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



            //client.GetCompany(177, CompanyMethods.Movies);
            //client.GetCompanyMovies(177);
            //Movie movie = client.GetMovie(47964, extraMethods: Enum.GetValues(typeof(MovieMethods)).OfType<MovieMethods>().Aggregate((methods, movieMethods) => movieMethods | methods));

            //client.GetCollection(1570, extraMethods: CollectionMethods.Images);
            //client.GetCollectionImages(1570);

            //client.GetList(movie.Lists.Results.First().Id);
            //client.GetPerson(62, extraMethods: PersonMethods.Images | PersonMethods.Credits | PersonMethods.Changes);
            //client.GetPersonChanges(62);
            //client.GetPersonCredits(62);
            //client.GetPersonImages(62);

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

            //client.GetMovieLatest();

            //client.GetMovie(47964, extraMethods: Enum.GetValues(typeof(MovieMethods)).OfType<MovieMethods>().Aggregate((methods, movieMethods) => movieMethods | methods));

            //client.SearchMovie("A good day to die");
            //client.SearchCollection("Die h");
            //client.SearchKeyword("Action");
            //client.SearchList("to watch");
            //client.SearchCompany("Disney");
            //client.SearchPerson("Bruce");

            //client.GetChangesMovies();
            //client.GetChangesPeople();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}