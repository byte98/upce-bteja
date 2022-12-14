using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject
{
    /// <summary>
    /// Main class of program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Path to file with source code which will be parsed and executed
        /// </summary>
        private static readonly string FILE = "PROGRAM3";

        /// <summary>
        /// Flag, whether debug messages should be printed
        /// </summary>
        public static readonly bool DEBUG = false;

        /// <summary>
        /// Exits a program
        /// </summary>
        public static void Exit()
        {
            Console.WriteLine();
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
            
            byte[] content = (byte[]) SemestralProject.Resources.ResourceManager.GetObject(Program.FILE, SemestralProject.Resources.Culture);
            Lexer.Lexer lexer = new Lexer.Lexer(Encoding.Default.GetString(content), Program.FILE + ".PHP");
            lexer.Run();
            if (Program.DEBUG)
            {
                lexer.PrintResults();
                Console.WriteLine("");
            }
            Parser.Parser parser = new Parser.Parser(lexer.GetResults());
            parser.Run();
            if (Program.DEBUG)
            {
                Console.WriteLine("");
            }
            Interpreter.Interpreter interpreter = new Interpreter.Interpreter(parser.GetResult());
            interpreter.Run();
            Program.Exit();
        }
    }
}
