﻿using common;
using common.resources;
using log4net;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ErrorHandling;
using Nancy.Extensions;
using Nancy.Hosting.Self;
using server.requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Manager;

namespace server
{
    /// <summary>
    /// This is the Manager class, it simply holds some public static fields which are accessed by requests.
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// This is the NancyRequestType enum, it stores different types of requests (as an example: GET).
        /// </summary>
        public enum NancyRequestType
        {
            GET,
            POST
        }

        /// <summary>
        /// This is a public static Resources variable, it is used to store all necessary resources of the game, took as a reference from Server.cs.
        /// </summary>
        public static Resources Resources;

        /// <summary>
        /// This is a public static Database variable, it is used as the connector to MongoDB, and it provides many useful storage based functions.
        /// </summary>
        public static Database Database;

        /// <summary>
        /// This is a public static bool variable, it determines whether the server is ready to log request dispatching.
        /// </summary>
        public static bool Dispatch;
    }

    /// <summary>
    /// This is the NancyUnknown class, it overides the default error return mechanic of Nancy.
    /// </summary>
    public class NancyUnknown : IStatusCodeHandler
    {
        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(NancyUnknown));

        /// <summary>
        /// This is the private IRootPathProvider variable, it is part of the IStatusCodeHandler interface from Nancy.
        /// </summary>
        readonly IRootPathProvider m_root;

        /// <summary>
        /// This is the private byte array variable, it is a simple byte array encoded string, which returns the error you get upon sending an unknown/invalid request.
        /// </summary>
        byte[] m_error => Encoding.UTF8.GetBytes(m_code == HttpStatusCode.NotFound ? 
            "Unknown request!" : 
            "Internal server error!");

        /// <summary>
        /// Stores the HttpStatusCode obtained when handling the request.
        /// </summary>
        HttpStatusCode m_code;

        /// <param name="root">This is the root path provider that the IStatusCodeHandler interface will use.</param>
        public NancyUnknown(IRootPathProvider root)
        {
            m_root = root;
        }

        /// <summary>
        /// This is a public function that is used to define the HttpStatusCode of the request, it is part of the IStatusCodeHandler interface from Nancy.
        /// </summary>
        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            m_code = statusCode;
            return m_code == HttpStatusCode.NotFound ||
                   m_code == HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// This is a public function that is used to handle the unknown request, it is part of the IStatusCodeHandler interface from Nancy.
        /// </summary>
        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var p = context.Request.Path;
            var sfx = p.Contains("sfx") || p.Contains("music");
            if (!sfx)
                log.Info(string.Format("Unknown request received {0}...", p));
            byte[] d = sfx ? 
                Manager.Resources.Music.Obtain(p): 
                m_error;
            context.Response.Contents = stream =>
            {
                stream.Write(d, 0, d.Length);
                stream.Close();
            };
        }
    }

    /// <summary>
    /// This is the NancyRequest class, it is a very simple class to make request handling easier.
    /// </summary>
    public abstract class NancyRequest : NancyModule
    {
        /// <param name="a">Address to process requests from (what gets sent to the server).</param>
        /// <param name="get">Whether the request is type of GET or type of POST.</param>
        public NancyRequest(string a)
        {
            Get[a] = p => {
                string s = "";
                Utils.Invoke(true, () => {
                    s = Handle(NancyRequestType.GET);
                });
                return Response.AsText(s);
            };

            Post[a] = p => {
                string s = "";
                Utils.Invoke(true, () => {
                    s = qHandle(NancyRequestType.POST);
                });
                return Response.AsText(s);
            };

            Dispatch = a;
            print(string.Format("Dispatching request {0}...", Dispatch));
        }

        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(NancyRequest));

        /// <summary>
        /// Public string variable, saves the type of request into a string.
        /// </summary>
        public string Dispatch;

        /// <summary>
        /// This is the string that is obtained after the request is handled/complete, it gets sent back to the client.
        /// </summary>
        /// <param name="nt">Type of the NancyRequest received. This is used commonly to jump between query types.</param>
        abstract public string Handle(NancyRequestType nt);

        /// <summary>
        /// This is a query expansion to the normal request handling.
        /// </summary>
        /// <param name="nt">Type of the NancyRequest received. This is used commonly to jump between query types.</param>
        public string qHandle(NancyRequestType nt)
        {
            foreach (var i in Request.Body.AsString().Split('&'))
            {
                var d = i.Split('=');
                if (d.Length >= 0)
                    Request.Query[d[0]] = d[1];
            }
            return Handle(nt);
        }

        /// <summary>
        /// Allows for simple access to the NancyRequest logger outside of the parent class.
        /// </summary>
        public void print(object m)
        {
            if (Manager.Dispatch)
                log.Info(m);
        }
    }

    /// <summary>
    /// This is the Server class, it handles POST/GET requests using the Nancy framework.
    /// </summary>
    public class Server : IDisposable
    {
        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(Server));

        /// <summary>
        /// This is a private static NancyHost variable, it is the parent HTTP server/handler of this class.
        /// </summary>
        NancyHost m_nancy;        
        
        /// <summary>
        /// This is a private Resources variable, it is used to store all necessary resources of the game, took as a reference from Program.cs.
        /// </summary>
        Resources m_resources;

        /// <param name="resources">This represents the game data/Resources which store all necessary information that needs to be accessed by requests.</param>
        /// <param name="bind">This is the bind address that NancyHost uses to initialize the server.</param>
        /// <param name="port">This is the bind port that NancyHost uses to initialize the server.</param>
        public Server(Resources resources, Database db, string bind, int port)
        {
            HostConfiguration config = new HostConfiguration();
            config.UrlReservations.CreateAutomatically = true;

            m_resources = resources;
            log.Info("Starting Server...");
            bool s = Utils.Invoke(true, () => {
                Manager.Resources = m_resources;
                Manager.Database = db;
                m_nancy = new NancyHost(config, new Uri(string.Format("http://{0}:{1}", bind, port)));
                m_nancy.Start();
                Dispatch = true;
            });

            log.Info(string.Format("Server {0} at address <{1}:{2}>...", 
                s ? "started" : "failed to start", bind, port));
        }

        /// <summary>
        /// This method is used for disposal of the class instance, as implemented by the IDisposable interface.
        /// </summary>
        public void Dispose()
        {
            log.Info("Stopping & disposing Server...");
            Utils.Invoke(false, () => {
                Dispatch = false;
                m_nancy.Stop();
                m_nancy.Dispose();
            });
            log.Info("Server stopped & disposed...");
        }
    }
}
