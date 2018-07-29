﻿using common;
using common.resources;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    /// <summary>
    /// This is the Program class, it contains the static Main method, which is seen as the start-up of the entire program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// This is a private static Resources variable, it is used to store all necessary resources of the game.
        /// </summary>
        static Resources m_resources;

        /// <summary>
        /// This is a private static ManualResetEvent variable, it is used to handle termination of the program.
        /// </summary>
        static ManualResetEvent m_reset;

        /// <summary>
        /// This is a private static Settings variable, it is used to store a variety of different configuration options.
        /// </summary>
        static Settings m_settings;

        /// <summary>
        /// This is a private static Server variable, it is used as the parent for all the HTTP POST/GET request handling.
        /// </summary>
        static Server m_server;

        /// <summary>
        /// This is a private static Database variable, it connects to the MongoDB database and provides you with many useful functions.
        /// </summary>
        static Database m_database;

        /// <summary>
        /// This is a private static object variable, it is used to ensure that all the necessary resources load first before you attempt to terminate the program.
        /// </summary>
        static object m_lock;

        /// <summary>
        /// This is the static Main method, it is used as the initial start-up method by the program.
        /// </summary>
        /// <param name="args">String arguments passed along when loading the executable file.</param>
        static void Main(string[] args)
        {
            m_reset = new ManualResetEvent(false);
            m_lock = new object();

            XmlConfigurator.Configure(new FileInfo("server.config"));
            Console.CancelKeyPress += OnCancelKeyPress;

            lock (m_lock)
            {
                string root = args.Length > 0 ? args[0] : "resources";
                m_resources = new Resources(root);
                m_settings = new Settings();
                m_database = new Database(m_settings);
                m_server = new Server(m_resources, m_database, m_settings.Server.Bind, m_settings.Server.Port);
            }

            m_reset.WaitOne();
        }

        /// <summary>
        /// This is the event handler for the ConsoleCancelEventArgs event type, it is used to terminate the program.
        /// </summary>
        /// <param name="sender">Object value passed as the object which has triggered the event.</param>
        /// <param name="e">The event information that sent from the event handler.</param>
        static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            lock (m_lock)
            {
                m_reset.Set();
                log.Info("Terminating program...");
                m_resources.Dispose();
                m_server.Dispose();
                log.Info("Program terminated...");
            }
        }
    }
}
