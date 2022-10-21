using System;
using System.IO;
using HW03.Lexer;

namespace HW03
{
    /// <summary>
    /// Main class of program
    /// </summary>
    class Program
    {
        /// <summary>
        /// File which will be parsed
        /// </summary>
        private static readonly string FILE = "example.pl0";
        

        /// <summary>
        /// Entrypoint of program
        /// </summary>
        /// <param name="args">Arguments of program</param>
        static void Main(string[] args)
        {

            string content = File.ReadAllText(Program.FILE);
            Lexer.Lexer lexer = new Lexer.Lexer(content, "EXAMPLE.PL0");
            lexer.run();
            Parser.Parser parser = new Parser.Parser(lexer.GetResults());
            parser.Build();
        }
    }
}
