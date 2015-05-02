using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace TMDbLib.Utilities
{
    public static class RestSharpExtensions
    {
        /// <summary>
        /// Executes a POST-style request asynchronously, authenticating if needed
        /// 
        /// </summary>
        /// <typeparam name="T">Target deserialization type</typeparam><param name="request">Request to be executed</param>
        public static Task<IRestResponse<T>> ExecuteDeleteTaskAsync<T>(this IRestClient client, IRestRequest request)
        {
            return client.ExecuteDeleteTaskAsync<T>(request, CancellationToken.None);
        }

        /// <summary>
        /// Executes a POST-style request asynchronously, authenticating if needed
        /// 
        /// </summary>
        /// <typeparam name="T">Target deserialization type</typeparam><param name="request">Request to be executed</param><param name="token">The cancellation token</param>
        public static Task<IRestResponse<T>> ExecuteDeleteTaskAsync<T>(this IRestClient client, IRestRequest request, CancellationToken token)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            request.Method = Method.DELETE;
            return client.ExecuteTaskAsync<T>(request, token);
        }
    }
}