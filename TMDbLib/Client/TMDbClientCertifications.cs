using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Certifications;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<CertificationsContainer> GetMovieCertifications()
        {
            RestRequest req = new RestRequest("certification/movie/list");

            IRestResponse<CertificationsContainer> resp = await _client.ExecuteGetTaskAsync<CertificationsContainer>(req).ConfigureAwait(false);

            return resp.Data;
        }

        public async Task<CertificationsContainer> GetTvCertifications()
        {
            RestRequest req = new RestRequest("certification/tv/list");

            IRestResponse<CertificationsContainer> resp = await _client.ExecuteGetTaskAsync<CertificationsContainer>(req).ConfigureAwait(false);

            return resp.Data;
        }
    }
}
