using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.Helpers;
using VerifyTests;
using VerifyXunit;

namespace TMDbLibTests.JsonHelpers
{
    public abstract class TestBase
    {            
        [ModuleInitializer]
        public static void Init() =>
            VerifyNewtonsoftJson.Initialize();
            
        private VerifySettings VerifySettings { get; }

        protected readonly TestConfig TestConfig;

        protected TMDbClient TMDbClient => TestConfig.Client;

        protected static ITMDbSerializer Serializer => TMDbJsonSerializer.Instance;

        protected TestBase()
        {
            VerifySettings = new VerifySettings();

            VerifySettings.UseDirectory("../Verification");

            // Ignore and simplify many dynamic properties
            VerifySettings.IgnoreProperty<SearchMovie>(x => x.VoteCount, x => x.Popularity, x => x.VoteAverage);
            VerifySettings.IgnoreProperty<SearchBase>(x => x.Popularity);
            VerifySettings.SimplifyProperty<SearchMovie>(x => x.BackdropPath, x => x.PosterPath);
            VerifySettings.SimplifyProperty<SearchPerson>(x => x.ProfilePath);
            VerifySettings.SimplifyProperty<SearchTvEpisode>(x => x.StillPath);
            VerifySettings.SimplifyProperty<ImageData>(x => x.FilePath);
            VerifySettings.SimplifyProperty<SearchCompany>(x => x.LogoPath);

            WebProxy proxy = null;
            //proxy = new WebProxy("http://127.0.0.1:8888");

            TestConfig = new TestConfig(serializer: null, proxy: proxy);
        }

        protected Task Verify<T>(T obj, Action<VerifySettings> configure = null)
        {
            VerifySettings settings = VerifySettings;

            if (configure is not null)
            {
                settings = new VerifySettings(VerifySettings);
                configure(settings);
            }

            return Verifier.Verify(obj, settings);
        }
    }
}
