using System.Threading.Tasks;
using TMDbLib.Objects.Certifications;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<CertificationsContainer> GetMovieCertifications()
        {
            TmdbRestRequest req =  _client2.Create("certification/movie/list");

            TmdbRestResponse<CertificationsContainer> resp = await req.ExecuteGet<CertificationsContainer>().ConfigureAwait(false);

            return resp;}

        public async Task<CertificationsContainer> GetTvCertifications()
        {
            TmdbRestRequest req =  _client2.Create("certification/tv/list");

            TmdbRestResponse<CertificationsContainer> resp = await req.ExecuteGet<CertificationsContainer>().ConfigureAwait(false);

            return resp;
        }
    }
}
