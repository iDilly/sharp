using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.resources
{
    /// <summary>
    /// This is the XmlData class, it stores all the parsed XML data of the game.
    /// </summary>
    public class XmlData : IDisposable
    {
        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(XmlData));

        /// <summary>
        /// This string >> ushort dictionary enables you to quickly obtain an object type from a string if it exists.
        /// </summary>
        public Dictionary<string, ushort> IdToObjectType = new Dictionary<string, ushort>();

        /// <summary>
        /// This ushort >> string dictionary enables you to quickly obtain an object Id from a type if it exists.
        /// </summary>
        public Dictionary<ushort, string> TypeToObjectId = new Dictionary<ushort, string>();

        /// <summary>
        /// This ushort >> XmlObject dictionary enables you to quickly obtain an XML object from a type if it exists.
        /// </summary>
        public Dictionary<ushort, XmlObject> TypeToObject = new Dictionary<ushort, XmlObject>();

        /// <summary>
        /// This string >> ushort dictionary enables you to quickly obtain an object type from a string if it exists.
        /// </summary>
        public Dictionary<string, ushort> IdToPlayerType = new Dictionary<string, ushort>();

        /// <summary>
        /// This ushort >> string dictionary enables you to quickly obtain an object Id from a type if it exists.
        /// </summary>
        public Dictionary<ushort, string> TypeToPlayerId = new Dictionary<ushort, string>();

        /// <summary>
        /// This ushort >> XmlPlayer dictionary enables you to quickly obtain an XML player from a type if it exists.
        /// </summary>
        public Dictionary<ushort, XmlPlayer> TypeToPlayer = new Dictionary<ushort, XmlPlayer>();

        /// <summary>
        /// This is a private string variable which simply stores the root path given.
        /// </summary>
        string m_root;

        /// <summary>
        /// This is a private object variable which ensures thread safety across different parsing threads.
        /// </summary>
        object m_lock;

        /// <param name="root">Root path of your game data (XML) files.</param>
        public XmlData(string root)
        {
            log.Info("Loading XmlData...");
            m_root = root;
            m_lock = new object();

            List<Task<string>> ts = new List<Task<string>>();
            foreach (var i in Directory.GetFiles(m_root))
                ts.Add(Utils.ReadAsync(i));

            foreach (var i in ts)
                i.ContinueWith((t) => {
                    Utils.Invoke(true, () => {
                        XmlParse(XElement.Parse(t.Result));
                    });
                });

            Task.WaitAll(ts.ToArray());
            log.Info(string.Format("Parsed {0} Objects!", TypeToObject.Count));
            log.Info(string.Format("Parsed {0} Players!", TypeToPlayer.Count));
            log.Info("XmlData loaded...");
        }

        /// <summary>
        /// This is a private function which the class uses to parse XML files which contain information about the game.
        /// </summary>
        /// <param name="e">Root element of your game data file.</param>
        void XmlParse(XElement e)
        {
            ushort type;
            string id;
            string c;
            foreach (var i in e.Elements("Object"))
            {
                type = i.GetAttribute<ushort>("type");
                id = i.GetAttribute<string>("id");

                c = i.GetValue<string>("Class");
                object xml;

                switch (c)
                {
                    case "Player":
                        xml = new XmlPlayer(i, type, id);
                        lock (m_lock)
                        {
                            TypeToPlayer[type] = (XmlPlayer)xml;
                            TypeToPlayerId[type] = id;
                            IdToPlayerType[id] = type;
                        }
                        break;
                    default:
                        xml = new XmlObject(i, type, id);
                        lock (m_lock)
                        {
                            TypeToObject[type] = (XmlObject)xml;
                            TypeToObjectId[type] = id;
                            IdToObjectType[id] = type;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// This method is used for disposal of the class instance, as implemented by the IDisposable interface.
        /// </summary>
        public void Dispose()
        {
            IdToObjectType.Clear();
            TypeToObjectId.Clear();
            TypeToObject.Clear();
            log.Info("XmlData disposed...");
        }
    }
}
