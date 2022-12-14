using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Parser.Nodes;

namespace SemestralProject.Interpreter
{
    /// <summary>
    /// Class representing in
    /// </summary>
    class Interpreter
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
        /// Executable node as result of parser
        /// </summary>
        private readonly IExecutableNode exec;

        /// <summary>
        /// Creates new interpereter of source code
        /// </summary>
        /// <param name="exec">Result of parser</param>
        public Interpreter(IExecutableNode exec)
        {
            this.exec = exec;
        }

        /// <summary>
        /// Runs interpreter
        /// </summary>
        public void Run()
        {
            this.exec.Execute();
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
                Interpreter.COUNTER++;
                Console.Write("(" + String.Format("{00:0#}", Interpreter.COUNTER) + ") ");
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
            if (Interpreter.ERR_COUNTER < Interpreter.ERR_MAX)
            {
                Interpreter.COUNTER++;
                Interpreter.ERR_COUNTER++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(" + String.Format("{00:0#}", Interpreter.COUNTER) + ") " + message);
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
                Console.WriteLine("Maximum errors (" + Interpreter.ERR_MAX + ") printed! Program exits!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Program.Exit();
            }
        }
    }
}
