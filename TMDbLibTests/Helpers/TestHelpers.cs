using System;
using System.Diagnostics;
using System.Net;
using TMDbLib.Objects.General;
using Xunit;

namespace TMDbLibTests.Helpers
{
    public static class TestHelpers
    {
        public static bool InternetUriExists(Uri uri)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Method = "HEAD";

            try
            {
                using (req.GetResponseAsync().Sync())
                {
                    // It exists
                    return true;
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response == null)
                    Debug.WriteLine(ex.Status + ": " + uri);
                else
                    Debug.WriteLine(response.StatusCode + ": " + uri);
                return false;
            }
        }

        public static void SearchPages<T>(Func<int, SearchContainer<T>> getter)
        {
            // Check page 1
            SearchContainer<T> results = getter(1);

            Assert.NotNull(results);
            Assert.NotNull(results.Results);
            Assert.Equal(1, results.Page);
            Assert.True(results.Results.Count > 0);
            Assert.True(results.TotalResults > 0);
            Assert.True(results.TotalPages > 0);

            // Check page 2
            SearchContainer<T> results2 = getter(2);

            Assert.NotNull(results2);
            Assert.NotNull(results2.Results);
            Assert.Equal(2, results2.Page);
            // The page counts often don't match due to caching on the api
            //Assert.AreEqual(results.TotalResults, results2.TotalResults);
            //Assert.AreEqual(results.TotalPages, results2.TotalPages);

            if (results.Results.Count == results.TotalResults)
                Assert.Equal(0, results2.Results.Count);
            else
                Assert.NotEqual(0, results2.Results.Count);
        }
    }
}
