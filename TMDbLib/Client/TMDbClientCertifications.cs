using RestSharp;
using TMDbLib.Objects.Certifications;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public CertificationsContainer GetMovieCertifications()
        {
            RestRequest req = new RestRequest("certification/movie/list");

            IRestResponse<CertificationsContainer> resp = _client.Get<CertificationsContainer>(req);

            return resp.Data;
        }

        public CertificationsContainer GetTvCertifications()
        {
            RestRequest req = new RestRequest("certification/tv/list");

            IRestResponse<CertificationsContainer> resp = _client.Get<CertificationsContainer>(req);

            return resp.Data;
        }
    }
}
