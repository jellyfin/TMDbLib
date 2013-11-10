using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace TMDbLib
{
	internal class TMDbRestClient : RestClient
	{
		public int MaxRetryCount { get; set; }
		public int RetryWaitTimeInSeconds { get; set; }

		public TMDbRestClient()
			: base()
		{
			InitializeDefaults();
		}

		public TMDbRestClient(string baseUrl)
			: base(baseUrl)
		{
			InitializeDefaults();
		}

		private void InitializeDefaults()
		{
			MaxRetryCount = 0;
			RetryWaitTimeInSeconds = 10;
		}

		/// <summary>
		/// Executes the specified request and deserializes the response content using the appropriate content handler
		/// </summary>
		/// <typeparam name="T">Target deserialization type</typeparam>
		/// <param name="request">Request to execute</param>
		/// <returns>RestResponse[[T]] with deserialized data in Data property</returns>
        /// <exception cref="UnauthorizedAccessException">Can be thrown if either to provided API key is invalid or when relavant the provided session id does not grant to required access</exception>
		public override IRestResponse<T> Execute<T>(IRestRequest request)
		{
			IRestResponse<T> response = base.Execute<T>(request);

			if (response.ErrorException != null)
			{
				if (MaxRetryCount >= request.Attempts && response.ErrorException.GetType() == typeof(WebException))
				{
					var webException = (WebException)response.ErrorException;
					// Retry the call after waiting the configured ammount of time, it gets progressively longer every retry
					Thread.Sleep(request.Attempts * RetryWaitTimeInSeconds * 1000);
					return this.Execute<T>(request);
				}
				else
				{
					throw response.ErrorException;
				}
			}

			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");
			}

			return response;
		}
	}
}
