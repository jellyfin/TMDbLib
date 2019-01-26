using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using TMDbLib.Client;
using TMDbLibTests.Exceptions;
using TMDbLibTests.TestFramework.HttpMocking;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace TMDbLibTests.TestFramework
{
    public class TestConfig : IDisposable
    {
        // This API key should only be used to test the library, for further use please request a dedicated key from the TMDb
        public const string APIKey = "c6b31d1cdad6a56a23f0c913e2482a31";

        // Required to be able to successfully run all authenticated tests
        public readonly string UserSessionId = "c413282cdadad9af972c06d9b13096a8b13ab1c1";
        public readonly string GuestTestSessionId = "d425468da2781d6799ba14c05f7327e7";

        private string _initializedAs;
        private TMDbClient _client;

        public TMDbClient Client
        {
            get => _client ?? throw new Exception("Initialize has not been called yet");
        }

        public string Username = "TMDbTestAccount";

        public string Password = "TJX6vP7bPC%!ZrJwAqtCU5FshHEKAwzr6YvR3%CU9s7BrjqUWmjC8AMuXju*eTEu524zsxDQK5ySY6EmjAC3e54B%WvkS9FNPE3K";

        public event Action<ErrorEventArgs> OnJsonError;

        public TestConfig()
        {
            if (APIKey.Length == 0)
                throw new ConfigurationErrorsException("You need to configure the API Key before running any tests. Look at the TestConfig class.");
        }

        public void Init(string name)
        {
            if (_initializedAs != null)
            {
                // We only initialize this once, and when we do, it must be for the same name as before (ClientMovieTests f.ex)
                if (name != _initializedAs)
                    throw new TestFrameworkException("Error in initializaton of config");

                return;
            }

            string dataDir = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))), "Data");

            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            _initializedAs = name;
            string file = Path.Combine(dataDir, $"data-{name}.json");

            //RecordingHandler handler = new RecordingHandler(file, new SocketsHttpHandler());
            ReplayingHandler handler = new ReplayingHandler(file);

            HttpClient httpClient = new HttpClient(handler);

            JsonSerializerSettings sett = new JsonSerializerSettings();

            sett.MissingMemberHandling = MissingMemberHandling.Error;
            sett.ContractResolver = new FailingContractResolver();
            sett.Error = Error;

            JsonSerializer serializer = JsonSerializer.Create(sett);

            _client = new TMDbClient(httpClient, APIKey, false, serializer: serializer);
        }

        private void Error(object sender, ErrorEventArgs errorEventArgs)
        {
            OnJsonError?.Invoke(errorEventArgs);
            errorEventArgs.ErrorContext.Handled = true;
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}