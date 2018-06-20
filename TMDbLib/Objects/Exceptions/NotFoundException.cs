using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TMDbLib.Objects.Exceptions
{
    public class NotFoundException : TMDbHttpException
    {
        public NotFoundException(HttpStatusCode httpStatusCode, TMDbStatusMessage statusMessage)
                        : base("The requested item was not found", httpStatusCode, statusMessage)
        {
        }
    }
}