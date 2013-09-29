using System;
using System.Diagnostics;
using System.Net;

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
    }
}
