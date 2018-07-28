using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    /// <summary>
    /// This is the Resources class, it can be seen as the parent of all game resource holders.
    /// </summary>
    public class Resources
    {
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
            m_root = root;
            XmlData = new XmlData();
        }
    }

    /// <summary>
    /// This is the XmlData class, it stores all the parsed XML data of the game.
    /// </summary>
    public class XmlData
    {

    }
}
