using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    /// <summary>
    /// This is the Program class, it contains the static Main method, which is seen as the start-up of the entire program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// This is a private static ManualResetEvent varaible, it is used to handle termination of the program.
        /// </summary>
        static ManualResetEvent m_reset;

        /// <summary>
        /// This is the static Main method, it is used as the initial start-up method by the program.
        /// </summary>
        /// <param name="args">String arguments passed along when loading the executable file.</param>
        static void Main(string[] args)
        {
            m_reset = new ManualResetEvent(false);
            Console.CancelKeyPress += OnCancelKeyPress;
            m_reset.WaitOne();
        }

        /// <summary>
        /// This is the event handler for the ConsoleCancelEventArgs event type, it is used to terminate the program.
        /// </summary>
        /// <param name="sender">Object value passed as the object which has triggered the event.</param>
        /// <param name="e">The event information that sent from the event handler.</param>
        static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            m_reset.Set();
        }
    }
}
