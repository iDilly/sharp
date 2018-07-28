using common;
using common.resources;
using log4net;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Manager;

namespace server.requests._char
{
    /// <summary>
    /// This is the list class, it is part of the requests handled by the HTTP server.
    /// </summary>
    public class list : NancyRequest
    {
        public list() : base("/char/list") { }

        /// <summary>
        /// This is the string that is obtained after the request is handled/complete, it gets sent back to the client.
        /// </summary>
        /// <param name="nt">Type of the NancyRequest received. This is used commonly to jump between query types.</param>
        public override string Handle(NancyRequestType nt)
            => "";
    }
}
