using System;
using System.Diagnostics;
using System.Net;

namespace TMDbLibTests.Helpers
{
	public static class TestHelpers
	{
		public static bool InternetUriExists(Uri uri)
		{
			var req = WebRequest.Create(uri) as HttpWebRequest;
			req.Method = "GET";
			req.Timeout = 5000;

			try
			{
				req.GetResponse();
				// It exists
				return true;
			}
			catch (WebException ex)
			{
				var response = (HttpWebResponse)ex.Response;
				if (response == null)
					Debug.WriteLine(ex.Status + ": " + uri);
				else
					Debug.WriteLine(response.StatusCode + ": " + uri);
				return false;
			}
		}
	}
}
