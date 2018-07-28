using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    /// <summary>
    /// This is the XmlData class, it stores information parsed from the setting(s).json files.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// This is a public server variable, which stores information used by the App server.
        /// </summary>
        public s_server Server;

        /// <summary>
        /// This is a public server variable, which stores information used by the Database (connecting).
        /// </summary>
        public s_common Common;

        public Settings()
        {
            var t1 = Utils.ReadAsync("server.json").ContinueWith((t) => {
                Server = JsonConvert.DeserializeObject<s_server>(t.Result);
            });

            var t2 = Utils.ReadAsync("common.json").ContinueWith((t) => {
                Common = JsonConvert.DeserializeObject<s_common>(t.Result);
            });

            Task.WaitAll(t1, t2);
        }
    }

    /// <summary>
    /// This is the s_server class, it stores information about the App server.
    /// </summary>
    public class s_server
    {
        /// <summary>
        /// This is public bind variable, the App server uses it when it starts up its Http server.
        /// </summary>
        [JsonProperty("bind")]
        public string Bind { get; set; } = "sss";

        /// <summary>
        /// This is public port variable, the App server uses it when it starts up its Http server.
        /// </summary>
        [JsonProperty("port")]
        public int Port { get; set; } = 80;
    }

    /// <summary>
    /// This is the s_common class, it stores information about the Database connector and more.
    /// </summary>
    public class s_common
    {
        /// <summary>
        /// This is public bind variable, this is what the Database connector uses when initializing its connection (address).
        /// </summary>
        [JsonProperty("bind")]
        public string Bind { get; set; } = "localhost";

        /// <summary>
        /// This is public port variable, this is what the Database connector uses when initializing its connection (port).
        /// </summary>
        /// 
        [JsonProperty("port")]
        public int Port { get; set; } = 27017;

        /// <summary>
        /// This is public id variable, this is what the Database connector uses when initializing its connection (unique database Id).
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = "sharp";
    }
}
