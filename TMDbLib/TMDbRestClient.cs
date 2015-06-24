using System.Threading.Tasks;
using RestSharp;
using System;
﻿using System.Linq;
using System.Net;
using System.Threading;
﻿using TMDbLib.Objects.Exceptions;

namespace TMDbLib
{
    internal class TMDbRestClient : RestClient
    {
        public int MaxRetryCount { get; set; }
        public int RetryWaitTimeInSeconds { get; set; }
        public bool ThrowErrorOnExeedingMaxCalls { get; set; }

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
            ThrowErrorOnExeedingMaxCalls = false;
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
                    WebException webException = (WebException)response.ErrorException;

                    // Retry the call after waiting the configured ammount of time, it gets progressively longer every retry
                    Thread.Sleep(request.Attempts * RetryWaitTimeInSeconds * 1000);
                    return Execute<T>(request);
                }

                throw response.ErrorException;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");
            }

            if (response.StatusCode == (HttpStatusCode)429)
            {
                if (!ThrowErrorOnExeedingMaxCalls)
                {
                    Parameter retryAfterParam = response.Headers.FirstOrDefault(header => header.Name.Equals("retry-after", StringComparison.OrdinalIgnoreCase));
                    if (retryAfterParam != null)
                    {
                        int retryAfter;
                        if (Int32.TryParse(retryAfterParam.Value.ToString().Trim(), out retryAfter))
                        {
                            Thread.Sleep(retryAfter * 1000);
                            return Execute<T>(request);
                        }
                    }
                }

                // We don't wish to wait or no valid retry-after header was present
                throw new RequestLimitExceededException();
            }

            return response;
        }

        public override async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            IRestResponse<T> response = await base.ExecuteTaskAsync<T>(request, token).ConfigureAwait(false);

            if (response.ErrorException != null)
            {
                if (MaxRetryCount >= request.Attempts && response.ErrorException.GetType() == typeof(WebException))
                {
                    WebException webException = (WebException)response.ErrorException;

                    // Retry the call after waiting the configured ammount of time, it gets progressively longer every retry
                    Thread.Sleep(request.Attempts * RetryWaitTimeInSeconds * 1000);
                    return await ExecuteTaskAsync<T>(request, token).ConfigureAwait(false);
                }

                throw response.ErrorException;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");
            }

            if (response.StatusCode == (HttpStatusCode)429)
            {
                if (!ThrowErrorOnExeedingMaxCalls)
                {
                    Parameter retryAfterParam = response.Headers.FirstOrDefault(header => header.Name.Equals("retry-after", StringComparison.OrdinalIgnoreCase));
                    if (retryAfterParam != null)
                    {
                        int retryAfter;
                        if (Int32.TryParse(retryAfterParam.Value.ToString().Trim(), out retryAfter))
                        {
                            Thread.Sleep(retryAfter * 1000);
                            return await ExecuteTaskAsync<T>(request, token).ConfigureAwait(false);
                        }
                    }
                }

                // We don't wish to wait or no valid retry-after header was present
                throw new RequestLimitExceededException();
            }

            return response;
        }

    }
}
