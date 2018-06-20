using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TMDbLib.Objects.Exceptions
{
    public class UnhandledException : TMDbHttpException
    {
        public UnhandledException(HttpStatusCode httpStatusCode, TMDbStatusMessage statusMessage)
                       : base("TMDb returned an unexpected error", httpStatusCode, statusMessage)
        {
        }
    }
}