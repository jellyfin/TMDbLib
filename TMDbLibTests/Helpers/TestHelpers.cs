using System;
using System.Diagnostics;
using System.Net;

namespace TMDbLibTests.Helpers
{
    public static class TestHelpers
    {
        public static bool InternetUriExists(Uri uri)
        {
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            req.Method = "HEAD";

            try
            {
                req.GetResponse();
                // It exists
                return true;
            }
            catch (WebException ex)
            {
                Debug.WriteLine(((HttpWebResponse)ex.Response).StatusCode + ": " + uri);
                return false;
            }
        }
    }
}
