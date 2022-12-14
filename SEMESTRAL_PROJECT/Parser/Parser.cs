using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Parser.Nodes;

namespace SemestralProject.Parser
{
    /// <summary>
    /// Class representing parser of tokens from source code
    /// </summary>
    class Parser
    {
        /// <summary>
        /// Counter of printed messages
        /// </summary>
        private static int COUNTER = 0;

        /// <summary>
        /// Counter of printed error messages
        /// </summary>
        private static int ERR_COUNTER = 0;

        /// <summary>
        /// Limit for error messages
        /// </summary>
        private static readonly int ERR_MAX = 8;

        /// <summary>
        /// Stream with tokens
        /// </summary>
        private readonly TokenStream tokens;

        /// <summary>
        /// Root node of whole AST
        /// </summary>
        private Nodes.Blocks.Program root;

        /// <summary>
        /// Creates new parser of tokens
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        public Parser(TokenStream tokens)
        {
            this.tokens = tokens;
        }

        /// <summary>
        /// Runs parser
        /// </summary>
        public void Run()
        {
            if (this.tokens.HasNext())
            {
                Token t = this.tokens.GetNext();
                if (t.GetTokenType() == TokenType.PROG_START)
                {
                    this.root = new Nodes.Blocks.Program(this.tokens, t, null);
                    this.root.Build();
                    if (this.tokens.HasNext())
                    {
                        t = this.tokens.GetNext();
                        if (t.GetTokenType() != TokenType.PROG_END)
                        {
                            Parser.PrintError("Unexpected end of program! End tag expected!", t);
                        }
                        else
                        {
                            Parser.Print("Parser finished.", t);
                        }
                    }
                    else
                    {
                        Parser.PrintError("Unexpected end of program! End tag expected!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected token! Expected start of program!", t);
                }
            }
            else
            {
                Parser.PrintError("Unexpected end of program! No token found!", null);
            }
        }

        /// <summary>
        /// Gets result of parser
        /// </summary>
        /// <returns>Result of parser</returns>
        public IExecutableNode GetResult()
        {
            return this.root;
        }

        /// <summary>
        /// Checks, whether parser finished successfully
        /// </summary>
        /// <returns>TRUE if parser finished without error, FALSE otherwise</returns>
        public bool Success()
        {
            return Parser.ERR_COUNTER == 0;
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
                Parser.COUNTER++;
                Console.Write("(" + String.Format("{00:0#}", Parser.COUNTER) + ") ");
                if (message != null)
                {
                    Console.WriteLine(message);
                    if (token != null)
                    {
                        Console.Write("      ");
                    }
                }
                if (token != null)
                {
                    Console.WriteLine(token.ToString());
                }
            }
        }

        /// <summary>
        /// Prints error message to console
        /// </summary>
        /// <param name="message">Message which will be printed to console</param>
        /// <param name="token">Token which caused error</param>
        public static void PrintError(string message, Token token)
        {
            if (Parser.ERR_COUNTER < Parser.ERR_MAX)
            {
                Parser.COUNTER++;
                Parser.ERR_COUNTER++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(" + String.Format("{00:0#}", Parser.COUNTER) + ") " + message);
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
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Maximum errors (" + Parser.ERR_MAX + ") printed! Program exits!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Program.Exit();
            }
        }
    }
}
