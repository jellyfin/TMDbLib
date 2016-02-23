using System.Threading.Tasks;
using TMDbLib.Objects.Certifications;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<CertificationsContainer> GetMovieCertificationsAsync()
        {
            RestRequest req =  _client.Create("certification/movie/list");

            RestResponse<CertificationsContainer> resp = await req.ExecuteGet<CertificationsContainer>().ConfigureAwait(false);

            return resp;}

        public async Task<CertificationsContainer> GetTvCertificationsAsync()
        {
            RestRequest req =  _client.Create("certification/tv/list");

            RestResponse<CertificationsContainer> resp = await req.ExecuteGet<CertificationsContainer>().ConfigureAwait(false);

            return resp;
        }
    }
}
