using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.resources
{
    /// <summary>
    /// This is the Music class, it stores all sound effects and music files.
    /// </summary>
    public class Music : IDisposable
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
        /// This is the string >> byte[] (array) dictionary, it stores all cached static sfx/music data.
        /// </summary>
        Dictionary<string, byte[]> _map = new Dictionary<string, byte[]>();

        /// <param name="root">Root path of your music/SFX files data.</param>
        public Music(string root)
        {
            log.Info("Loading Music/SFX...");
            m_root = root;
            var sep = new string[] { "sfx" };
            foreach (var i in Directory.GetFiles(m_root, "*.mp3", SearchOption.AllDirectories))
            {
                var id = "/" + sep[0] + i.Split(sep, StringSplitOptions.None)[1].Replace(@"\", @"/");
                if (id.Contains("music"))
                    id = id.Replace("/sfx", string.Empty);
                Map(id, i);
            }
            log.Info("Music/SFX loaded...");
        }

        /// <summary>
        /// This is the Map function. You can use it to cache additional data inside the string >> byte[] (array) dictionary.
        /// </summary>
        /// <param name="p">Unique Id of the file to map with.</param>
        /// <param name="s">Path of the file you want to extract bytes from.</param>
        void Map(string p, string s) =>
            _map[p] = Utils.ReadBytes(s);

        /// <summary>
        /// This is the Obtain function. You can use it to get the cached/saved data.
        /// </summary>
        /// <param name="s">Type of the file that you want to retrieve from the cache.</param>
        public byte[] Obtain(string s)
        {
            if (_map.TryGetValue(s, out var v))
                return v;
            log.Error(string.Format("Music/SFX file of Id {0} not found/cached, returning nothing...", s.ToString()));
            return new byte[0];
        } 
        
        /// <summary>
        /// This method is used for disposal of the class instance, as implemented by the IDisposable interface.
        /// </summary>
        public void Dispose()
        {
            log.Info("Disposing Music/SFX...");
            _map.Clear();
            log.Info("Music/SFX disposed...");
        }
    }
}
