using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.resources
{
    /// <summary>
    /// This is the StaticFile enum, it makes up the different identifiers for different types of static files.
    /// </summary>
    public enum StaticFile
    {
        CROSSDOMAIN,
        GLOBALNEWS,
        ITEMCOSTS,
        INIT,
        DE,
        EN,
        ES,
        FR,
        IT,
        RU
    }

    /// <summary>
    /// This is the StaticFiles class, it stores all the static documents (crossdomain as an example).
    /// </summary>
    public class StaticFiles : IDisposable
    {
        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(StaticFiles));

        /// <summary>
        /// This is a private string variable which simply stores the root path given.
        /// </summary>
        string m_root;

        /// <summary>
        /// This is the StaticFile >> string dictionary, it stores all cached static file data.
        /// </summary>
        Dictionary<StaticFile, string> _map = new Dictionary<StaticFile, string>();

        /// <param name="root">Root path of your static files data.</param>
        public StaticFiles(string root)
        {
            log.Info("Loading StaticFiles...");
            m_root = root;
            Map(string.Format("{0}/crossdomain.xml", m_root), StaticFile.CROSSDOMAIN);
            Map(string.Format("{0}/globalnews.json", m_root), StaticFile.GLOBALNEWS);
            Map(string.Format("{0}/itemcosts.xml", m_root), StaticFile.ITEMCOSTS);
            Map(string.Format("{0}/init.xml", m_root), StaticFile.INIT);
            Map(string.Format("{0}/en.json", m_root), StaticFile.EN);
            Map(string.Format("{0}/de.json", m_root), StaticFile.DE);
            Map(string.Format("{0}/es.json", m_root), StaticFile.ES);
            Map(string.Format("{0}/fr.json", m_root), StaticFile.FR);
            Map(string.Format("{0}/it.json", m_root), StaticFile.IT);
            Map(string.Format("{0}/ru.json", m_root), StaticFile.RU);
            log.Info("StaticFiles loaded...");
        }

        /// <summary>
        /// This is the Map function. You can use it to cache additional data inside the StaticFile >> string dictionary.
        /// </summary>
        /// <param name="p">Path of the static file you want to read from.</param>
        /// <param name="s">Type of the StaticFile you're reading.</param>
        void Map(string p, StaticFile s) =>
            _map[s] = Utils.Read(p);

        /// <summary>
        /// This is the Obtain function. You can use it to get the cached/saved data.
        /// </summary>
        /// <param name="s">Type of the StaticFile that you want to retrieve from the cache.</param>
        public string Obtain(StaticFile s)
        {
            if (_map.TryGetValue(s, out var v))
                return v;
            log.Error(string.Format("Static file of type {0} not found/cached, returning nothing...", s.ToString()));
            return "";
        } 
        
        /// <summary>
        /// This method is used for disposal of the class instance, as implemented by the IDisposable interface.
        /// </summary>
        public void Dispose()
        {
            log.Info("Disposing StaticFiles...");
            _map.Clear();
            log.Info("StaticFiles disposed...");
        }
    }
}
