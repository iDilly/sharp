using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.resources
{
    /// <summary>
    /// This is the Resources class, it can be seen as the parent of all game resource holders.
    /// </summary>
    public class Resources : IDisposable
    {
        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(Resources));

        /// <summary>
        /// This is a private string variable which simply stores the root path given.
        /// </summary>
        string m_root;

        /// <summary>
        /// This public variable stores all the parsed XML data of the game, inside a class: XmlData.cs.
        /// </summary>
        public XmlData XmlData;

        /// <param name="root">Root path of your resource files.</param>
        public Resources(string root)
        {
            log.Info("Loading Resources...");
            m_root = root;
            XmlData = new XmlData($"{root}/xml");
            log.Info("Resources loaded...");
        }

        /// <summary>
        /// This method is used for disposal of the class instance, as implemented by the IDisposable interface.
        /// </summary>
        public void Dispose()
        {
            XmlData.Dispose();
            log.Info("Resources disposed...");
        }
    }
}
