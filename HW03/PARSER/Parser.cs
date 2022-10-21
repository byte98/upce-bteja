using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser
{
    /// <summary>
    /// Class representing parser of program
    /// </summary>
    class Parser
    {
        /// <summary>
        /// Counter of printed lines
        /// </summary>
        private static int OUTPUT_COUNTER = 1;

        /// <summary>
        /// Browser of tokens
        /// </summary>
        private Lexer.Lexer.LexerResults tokens;

        /// <summary>
        /// Root of AST
        /// </summary>
        private Nodes.Program root;

        /// <summary>
        /// Creates new parser
        /// </summary>
        /// <param name="tokens">Browser of tokens</param>
        public Parser(Lexer.Lexer.LexerResults tokens)
        {
            this.tokens = tokens;
        }

        /// <summary>
        /// Prints message
        /// </summary>
        /// <param name="message">Message which will be printed</param>
        public static void Print(string message)
        {
            Console.WriteLine("(" + String.Format("{00:0#}", Parser.OUTPUT_COUNTER) + ") " + message);
            Parser.OUTPUT_COUNTER++;
        }

        /// <summary>
        /// Builds AST
        /// </summary>
        public void Build()
        {
            this.root = new Nodes.Program(this.tokens);
            this.root.Build();
        }
    }
}
