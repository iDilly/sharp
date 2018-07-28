﻿using common;
using common.resources;
using log4net;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Manager;

namespace server.requests.app
{
    /// <summary>
    /// This is the globalNews class, it is part of the requests handled by the HTTP server.
    /// </summary>
    public class globalNews : NancyRequest
    {
        public globalNews() : base("/app/globalNews")
        {
            print(string.Format("Dispatching request {0}...", Dispatch));
        }

        /// <summary>
        /// This is the string that is obtained after the request is handled/complete, it gets sent back to the client.
        /// </summary>
        /// <param name="nt">Type of the NancyRequest received. This is used commonly to jump between query types.</param>
        public override string Handle(NancyRequestType nt)
            => Manager.Resources.StaticFiles.Obtain(StaticFile.INIT);
    }
}
