using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using HW04.Lexer;

namespace HW04
{
    // <summary>
    /// Main class of program
    /// </summary>
    class Program
    {
        /// <summary>
        /// File which will be parsed
        /// </summary>
        private static readonly string FILE = "example.pl0";

        /// <summary>
        /// Flag, whether program should print debug messages
        /// </summary>
        public static bool DEBUG = false;

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
            Stopwatch sw = new Stopwatch();
            if (Program.DEBUG)
            {
                sw.Start();
            }
            Lexer.Lexer lexer = new Lexer.Lexer(Encoding.Default.GetString(HW04.Resources.example_pl0), Program.FILE);
            lexer.Run();
            if (Program.DEBUG)
            {
                sw.Stop();
                lexer.PrintResults();
                Console.WriteLine();
                Console.WriteLine("Lexer finished in " + sw.ElapsedMilliseconds + " ms");
                Console.WriteLine();
                sw.Restart();
            }
            Parser.Parser parser = new Parser.Parser(lexer.GetResults(), null);
            parser.Run();
            if (Program.DEBUG)
            {
                sw.Stop();
                Console.WriteLine();
                Console.WriteLine("Parser finished in " + sw.ElapsedMilliseconds + " ms");
                Console.WriteLine();
                Console.WriteLine("Executing program " + Program.FILE + "...");
                sw.Restart();
            }
            parser.Execute();
            if (Program.DEBUG)
            {
                Console.WriteLine("Program executed in " + sw.ElapsedMilliseconds + " ms");
            }
            Program.Exit();
        }
    }
}
