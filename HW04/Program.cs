using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static bool DEBUG = true;

        /// <summary>
        /// Entrypoint of program
        /// </summary>
        /// <param name="args">Arguments of program</param>
        static void Main(string[] args)
        {
            Lexer.Lexer lexer = new Lexer.Lexer(Encoding.Default.GetString(HW04.Resources.example_pl0), Program.FILE);
            lexer.Run();
            if (Program.DEBUG)
            {
                lexer.PrintResults();
                Console.WriteLine();
            }
            Parser.Parser parser = new Parser.Parser(lexer.GetResults());
            parser.Run();
            if (Program.DEBUG)
            {
                Console.WriteLine();
                Console.WriteLine("Executing program " + Program.FILE);
            }
            parser.Execute();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
