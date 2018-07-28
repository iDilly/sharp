using log4net;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    /// <summary>
    /// This is the Database class, it uses MongoDB as its client and provides you with many useful functions which make up the whole storage engine of the source.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(Database));

        /// <summary>
        /// This is the private MongoClient variable, it is the client instance attatched to the Database class.
        /// </summary>
        MongoClient m_client;

        /// <summary>
        /// This is the private IMongoDatabase variable, it is the database instance attatched to the MongoClient.
        /// </summary>
        IMongoDatabase m_db;

        /// <param name="settings">The settings reference which is used to connect to MongoDB (the address, port and index).</param>
        public Database(Settings settings)
        {
            log.Info("Loading Database...");
            string conn = string.Format("mongodb://{0}:{1}", 
                settings.Common.Bind, settings.Common.Port);

            m_client = new MongoClient(conn);
            m_db = m_client.GetDatabase(settings.Common.Id);
            log.Info("Database loaded...");
        }
    }
}
