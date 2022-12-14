using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HW05.Lexer;

namespace HW05.Parser
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
        /// Counter of printed errors
        /// </summary>
        private static int errCounter = 0;

        /// <summary>
        /// Number of maximum printed errors
        /// </summary>
        private static readonly int MAX_ERR = 5;

        /// <summary>
        /// Program node as root of AST
        /// </summary>
        private Nodes.Program program;

        /// <summary>
        /// Handler of progress of parser
        /// </summary>
        private static IProgressHandler progressHandler;

        /// <summary>
        /// Creates new parser of language
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="progressHandler">Handler of progress of parser</param>
        public Parser(TokenStream tokens, IProgressHandler progressHandler)
        {
            this.tokens = tokens;
            Parser.progressHandler = progressHandler;
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
        /// Gets executable node
        /// </summary>
        /// <returns>Root node which is executable</returns>
        public HW05.Parser.Nodes.ExecutableBlockNode GetExecutable()
        {
            return this.program;
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
            if (Parser.errCounter < Parser.MAX_ERR)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(" + String.Format("{00:0#}", Parser.counter) + ") " + message);
                if (token != null)
                {
                    string line = "     File " + token.GetFile() + " [line: " + token.GetRow() + "; column: " + token.GetColumn() + "]: ";
                    Console.WriteLine(line + token.GetLine());
                    for (int i = 0; i < line.Length; i++)
                    {
                        Console.Write(" ");
                    }
                    for (int i = 0; i < token.GetColumn() - 1; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write("˄");
                    for (int i = 1; i < token.GetContent().Length; i++)
                    {
                        Console.Write("ˉ");
                    }
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Parser.counter++;
                Parser.errCounter++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Maximum errors (" + Parser.MAX_ERR + ") printed! Program exits!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Program.Exit();
            }
            
        }

        /// <summary>
        /// Sets progress to progress handler
        /// </summary>
        /// <param name="progress">New value of progress from interval [0; 100]</param>
        public static void SetProgress(float progress)
        {
            if (Parser.progressHandler != null)
            {
                Parser.progressHandler.UpdateProgress(progress);
            }
        }
    }
}
