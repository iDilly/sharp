﻿using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.requests
{
    public class sfx : NancyModule
    {
        public sfx()
        {
            Get["/sfx"] = p => 
            {
                return p;
            };
        }
    }
}
