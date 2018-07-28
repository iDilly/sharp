using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    /// <summary>
    /// This is the static Utils partial class, it provides you with different useful methods and extensions.
    /// </summary>
    public static partial class Utils
    {
        /// <summary>
        /// The ReadAsync static method allows you to read documents in an asynchronous fasion.
        /// </summary>
        /// <param name="p">Path of the document that you want to read.</param>
        public static async Task<string> ReadAsync(string p)
        {
            var t1 = Task.Run(() => {
                string ret = "";
                Invoke(() => {
                    ret = File.ReadAllText(p);
                });
                return ret;
            });
            await t1;
            return t1.Result;
        }

        /// <summary>
        /// The Invoke static method allows you to easily perform error-safe actions.
        /// </summary>
        /// <param name="a">Action that you want to perform.</param>
        public static bool Invoke(Action a)
        {
            try
            {
                a();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
