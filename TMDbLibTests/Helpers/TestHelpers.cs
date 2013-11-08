using System;
using System.Diagnostics;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;

namespace TMDbLibTests.Helpers
{
    public static class TestHelpers
    {
        public static bool InternetUriExists(Uri uri)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uri);
            req.Method = "HEAD";

            try
            {
                using (req.GetResponse())
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

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Results);
            Assert.AreEqual(1, results.Page);
            Assert.IsTrue(results.Results.Count > 0);
            Assert.IsTrue(results.TotalResults > 0);
            Assert.IsTrue(results.TotalPages > 0);

            // Check page 2
            SearchContainer<T> results2 = getter(2);

            Assert.IsNotNull(results2);
            Assert.IsNotNull(results2.Results);
            Assert.AreEqual(2, results2.Page);
            Assert.AreEqual(results.TotalResults, results2.TotalResults);
            Assert.AreEqual(results.TotalPages, results2.TotalPages);

            if (results.Results.Count == results.TotalResults)
                Assert.AreEqual(0, results2.Results.Count);
            else
                Assert.AreNotEqual(0, results2.Results.Count);

        }
    }
}
