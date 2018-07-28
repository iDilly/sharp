using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common
{
    /// <summary>
    /// This is the static Utils partial class, it provides you with different useful methods and extensions.
    /// </summary>
    public static partial class Utils
    {
        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(Utils));

        /// <summary>
        /// The ReadAsync static method allows you to read documents in an asynchronous fasion.
        /// </summary>
        /// <param name="p">Path of the document that you want to read.</param>
        public static async Task<string> ReadAsync(string p)
        {
            var t1 = Task.Run(() => {
                string ret = "";
                Invoke(true, () => {
                    ret = File.ReadAllText(p);
                });
                return ret;
            });
            await t1;
            return t1.Result;
        }

        /// <summary>
        /// The ReadAfter static method allows you to read documents in an synchronous fasion with a callback attatched.
        /// </summary>
        /// <param name="p">Path of the document that you want to read.</param>
        /// <param name="c">Action to callback on after the file is read.</param>
        public static void ReadAfter(string p, Action<string> c)
        {
            string ret = "";
            Invoke(true, () => {
                ret = File.ReadAllText(p);
            });
            c(ret);
        }

        /// <summary>
        /// The Read static method allows you to read documents in an synchronous fasion.
        /// </summary>
        /// <param name="p">Path of the document that you want to read.</param>
        public static string Read(string p)
        {
            string ret = "";
            Invoke(true, () => {
                ret = File.ReadAllText(p);
            });
            return ret;
        }

        /// <summary>
        /// The Invoke static method allows you to easily perform error-safe actions.
        /// </summary>
        /// <param name="l">Whether you want to log the exception if one is caught.</param>
        /// <param name="a">Action that you want to perform.</param>
        public static bool Invoke(bool l, Action a)
        {
            try
            {
                a();
                return true;
            }
            catch (Exception ex)
            {
                if (l)
                    log.Error(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// The GetValue <typeparamref name="T"/> method allows you to parse data from a specific XML element.
        /// </summary>
        /// <param name="e">Root XML element you want to parse data from.</param>
        /// <param name="n">Name of the element you want to parse data from.</param>
        /// <param name="def">Default value to return incase the element is not found or the <typeparamref name="T"/> value type is not supported.</param>
        public static T GetValue<T>(this XElement e, string n, T def = default(T))
        {
            if (e.Element(n) == null)
                return def;

            string val = e.Element(n).Value;
            var t = typeof(T);
            if (t == typeof(string))
                return (T)Convert.ChangeType(val, t);
            else if (t == typeof(ushort))
                return (T)Convert.ChangeType(Convert.ToUInt16(val, 16), t);
            else if (t == typeof(int))
                return (T)Convert.ChangeType(int.Parse(val), t);
            else if (t == typeof(uint))
                return (T)Convert.ChangeType(Convert.ToUInt32(val, 16), t);
            else if (t == typeof(double))
                return (T)Convert.ChangeType(double.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(float))
                return (T)Convert.ChangeType(float.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(bool))
                return (T)Convert.ChangeType(string.IsNullOrWhiteSpace(val) || bool.Parse(val), t);

            log.Error(string.Format("Type of {0} is not supported by this method, returning default value: {1}...", t, def));
            return def;
        }

        /// <summary>
        /// The GetAttribute <typeparamref name="T"/> method allows you to parse data from a specific XML attribute.
        /// </summary>
        /// <param name="e">Root XML element you want to parse data from.</param>
        /// <param name="n">Name of the attribute you want to parse data from.</param>
        /// <param name="def">Default value to return incase the attribute is not found or the <typeparamref name="T"/> value type is not supported.</param>
        public static T GetAttribute<T>(this XElement e, string n, T def = default(T))
        {
            if (e.Attribute(n) == null)
                return def;

            string val = e.Attribute(n).Value;
            var t = typeof(T);
            if (t == typeof(string))
                return (T)Convert.ChangeType(val, t);
            else if (t == typeof(ushort))
                return (T)Convert.ChangeType(Convert.ToUInt16(val, 16), t);
            else if (t == typeof(int))
                return (T)Convert.ChangeType(int.Parse(val), t);
            else if (t == typeof(uint))
                return (T)Convert.ChangeType(Convert.ToUInt32(val, 16), t);
            else if (t == typeof(double))
                return (T)Convert.ChangeType(double.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(float))
                return (T)Convert.ChangeType(float.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(bool))
                return (T)Convert.ChangeType(string.IsNullOrWhiteSpace(val) || bool.Parse(val), t);

            log.Error(string.Format("Type of {0} is not supported by this method, returning default value: {1}...", t, def));
            return def;
        }
    }
}
