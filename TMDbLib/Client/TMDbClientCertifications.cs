using System.Threading.Tasks;
using TMDbLib.Objects.Certifications;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<CertificationsContainer> GetMovieCertifications()
        {
            RestRequest req =  _client2.Create("certification/movie/list");

            RestResponse<CertificationsContainer> resp = await req.ExecuteGet<CertificationsContainer>().ConfigureAwait(false);

            return resp;}

        public async Task<CertificationsContainer> GetTvCertifications()
        {
            RestRequest req =  _client2.Create("certification/tv/list");

            RestResponse<CertificationsContainer> resp = await req.ExecuteGet<CertificationsContainer>().ConfigureAwait(false);

            return resp;
        }
    }
}
