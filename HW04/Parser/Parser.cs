using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HW04.Lexer;

namespace HW04.Parser
{
    /// <summary>
    /// Class representing parser of language
    /// </summary>
    class Parser
    {
        /// <summary>
        /// Stream of tokens from lexer
        /// </summary>
        private TokenStream tokens;

        /// <summary>
        /// Counter of printed lines
        /// </summary>
        private static int counter = 1;

        /// <summary>
        /// Program node as root of AST
        /// </summary>
        private Nodes.Program program;

        /// <summary>
        /// Creates new parser of language
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        public Parser(TokenStream tokens)
        {
            this.tokens = tokens;
        }

        /// <summary>
        /// Runs a parser
        /// </summary>
        public void Run()
        {
            this.program = new Nodes.Program(this.tokens, null);
            this.program.Build();
        }

        /// <summary>
        /// Executes parsed program
        /// </summary>
        public void Execute()
        {
            if (this.program == null)
            {
                Parser.PrintError("Cannot execute program (parser haven't found anything to execute)!", null);
            }
            else
            {
                this.program.Execute();
            }
        }

        /// <summary>
        /// Prints message to console
        /// </summary>
        /// <param name="message">Message which will be printed to console</param>
        /// <param name="token">Token providing more information</param>
        public static void Print(string message, Token token)
        {
            if (Program.DEBUG)
            {
                Console.WriteLine("(" + String.Format("{00:0#}", Parser.counter) + ") " + message);
                if (token != null)
                {
                    Console.WriteLine("      " + token.ToString());
                }
                Parser.counter++;
            }
        }

        /// <summary>
        /// Prints error message to console
        /// </summary>
        /// <param name="message">Message which will be printed to console</param>
        /// <param name="token">Token which caused error</param>
        public static void PrintError(string message, Token token)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("(" + String.Format("{00:0#}", Parser.counter) + ") " + message);
            if (token != null)
            {
                Console.WriteLine("      " + token.ToString());
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Parser.counter++;
        }
    }
}
