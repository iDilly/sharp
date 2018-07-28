using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    /// <summary>
    /// This is the XmlData class, it stores all the parsed XML data of the game.
    /// </summary>
    public class XmlData
    {
        /// <summary>
        /// This is a private string variable which simply stores the root path given.
        /// </summary>
        string m_root;

        /// <param name="root">Root path of your resource files.</param>
        public XmlData(string root)
        {
            m_root = root;
        }
    }
}
