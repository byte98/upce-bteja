using System;
using System.IO;

namespace HW02
{
    /// <summary>
    /// Main class of program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Path to file which will be parsed through lexer
        /// </summary>
        private static readonly string PATH = "example.pl0";

        /// <summary>
        /// Entrypoint of program
        /// </summary>
        /// <param name="args">Arguments of program</param>
        static void Main(string[] args)
        {
            string content = File.ReadAllText(Program.PATH);
            Lexer lexer = new Lexer(content, Program.PATH);
            lexer.run();
            Console.Write("Press any key to finish...");
            Console.ReadKey();
        }
    }
}
