using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using HW06.Lexer;

namespace HW06
{
    // <summary>
    /// Main class of program
    /// </summary>
    class Program
    {

        /// <summary>
        /// Flag, whether debugging messages should be printed or not
        /// </summary>
        public static readonly bool DEBUG = true;

        /// <summary>
        /// Exits a program
        /// </summary>
        public static void Exit()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            System.Environment.Exit(0);
        }

        /// <summary>
        /// Entrypoint of program
        /// </summary>
        /// <param name="args">Arguments of program</param>
        static void Main(string[] args)
        {
            Program.Exit();
        }
    }
}
